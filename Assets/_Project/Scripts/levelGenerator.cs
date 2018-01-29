using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGenerator : MonoBehaviour {

	//List<Floor> world = new List<Floor>();
    public GameObject levelHolder;
    
	public void Initialize (Style s) {
        levelHolder = new GameObject("Floor" + GameManager.Instance.currentFloor);
        
		generateFloor(s);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            //int count = levelHolder.transform.childCount;
            //for (int i = 0; i < count; i++)
                Destroy(levelHolder);
            levelHolder = new GameObject("Floor" + GameManager.Instance.currentFloor);
            generateFloor((Style)1);
        }
    }

    bool hasFinished = true;
    public Floor generateFloor(Style s){
		Floor f = new Floor();


        f.Add(new Room(s, Vector2.zero));


        //
        int winPathCount = UnityEngine.Random.Range(6,12);

		Vector2 cursorPosition = Vector2.zero;
		Direction directionProd = 0;
        //generate the ideal path
        for (int winPath = 1; winPath < winPathCount; winPath++){
			//set direction test
			directionProd = (Direction)UnityEngine.Random.Range(0,4);


			bool acceptable = false;
			while(!acceptable)
            {
                iterateDirection(ref directionProd);

                //edge case: stuck in a corner
                if (f.checkDoorNum(cursorPosition) == 4)
                {
                    f[winPath].isDestination = true;
                    hasFinished = true;
                    winPath = winPathCount;
                    acceptable = true;
                    //don't add a room if you're stuck
                    break;
                }

                //if(there's nothing where the directionProd is)
                if (!f.checkDirections(cursorPosition)[(int)directionProd])
                {
                    acceptable = true;
                }

                //if we've passed, but it isn't the end of the line, just add
                if (acceptable)
                {
                    cursorPosition += directionToVector(directionProd);
                    f.Add(new Room(s, cursorPosition));

                    //bool[] debugDirs = f.checkDirections(cursorPosition);
                    //Debug.Log("lcoation:" + cursorPosition + " direction: " + directionProd + " sides: " + debugDirs[0] + " " + debugDirs[1] + " " + debugDirs[2] + " " + debugDirs[3]);
                }
            }

            if (UnityEngine.Random.Range(0f, 1f) > .8f && winPath>0)
            {
                f[winPath].isDestination = true;
                hasFinished = true;
            }

            if (winPath == winPathCount-1 && !hasFinished)
                f[winPath].isDestination = true;
        }


        //Add sister rooms
        /*
        for (int i = 0; i < winPathCount * 2 / 3; i++)
        {
            Direction prod = (Direction)UnityEngine.Random.Range(0, 4);
            int potentialSister = 0;
            while (f.checkDirections(potentialSister)[prod])
            {
                potentialSister = UnityEngine.Random.Range(0, f.Count);
            }
        }*/


        //set door count and directions
        for(int i  = 0; i<f.Count; i++)
        {
            f[i].doorCount = f.checkDoorNum(f[i].location);
            f.updateSiblings(i);
            switch (f[i].doorCount)
            {
                case 1:
                    if (f[i].siblings[1]) f[i].direction = Direction.SOUTH;
                    if (f[i].siblings[2]) f[i].direction = Direction.EAST;
                    if (f[i].siblings[3]) f[i].direction = Direction.WEST;
                    break;
                case 2:
                    if (f[i].siblings[0] && f[i].siblings[1])
                        f[i].direction = Direction.CROSS;
                    else if (f[i].siblings[2] && f[i].siblings[3])
                        f[i].direction = Direction.HCROSS;
                    else if (f[i].siblings[0] && f[i].siblings[3])
                        f[i].direction = Direction.NORTH;
                    else if (f[i].siblings[0] && f[i].siblings[2])
                        f[i].direction = Direction.SOUTH;
                    else if (f[i].siblings[1] && f[i].siblings[2])
                        f[i].direction = Direction.EAST;
                    else if (f[i].siblings[1] && f[i].siblings[3])
                        f[i].direction = Direction.WEST;

                    else Debug.Log("error");
                        
                    break;
                case 3:
                    if (!f[i].siblings[1]) f[i].direction = Direction.SOUTH;
                    if (!f[i].siblings[2]) f[i].direction = Direction.EAST;
                    if (!f[i].siblings[3]) f[i].direction = Direction.WEST;
                    break;
            }
        }

        //instantiate path
        foreach (Room r in f)
        {
            GameObject m;
            //TODO: randomize loads
            string stylename = Enum.GetName(typeof(Style), r.style);
            if (r.direction != Direction.CROSS && r.direction != Direction.HCROSS)
                m = Instantiate(Resources.Load<GameObject>(stylename + "/room" + r.doorCount + "-" + (int)UnityEngine.Random.Range(1,3))); 
            else
                m = Instantiate(Resources.Load<GameObject>(stylename + "/room5-" + (int)UnityEngine.Random.Range(1, 3)));
            m.transform.position = new Vector3(r.location.x * -6.4f*2, -10, r.location.y * -6.4f*2);
            m.name = r.location.x + " " + r.location.y;

            //Add grabbable components
            for(int i = 0; i< m.transform.childCount; i++)
            {
                GameObject g = m.transform.GetChild(i).gameObject;
                g.AddComponent<VRTK.VRTK_InteractableObject>().isGrabbable = true;
                VRTK.GrabAttachMechanics.VRTK_FixedJointGrabAttach c = g.AddComponent<VRTK.GrabAttachMechanics.VRTK_FixedJointGrabAttach>();
                c.precisionGrab = true;
                c.throwMultiplier = 5;
                g.AddComponent<BoxCollider>();
                g.layer = LayerMask.NameToLayer("Throwables");
            }

            //set room directions
            switch (r.direction)
            {
                case Direction.NORTH:
                    break;
                case Direction.SOUTH:
                    switch (r.doorCount)
                    {
                        case 1:
                            m.transform.rotation = Quaternion.Euler(-90, 180, 0);
                            break;
                        case 2:
                            m.transform.rotation = Quaternion.Euler(-90, 90, 0);
                            break;
                        case 3:
                            m.transform.rotation = Quaternion.Euler(-90, 180, 0);
                            break;
                    }
                    break;
                case Direction.EAST:
                    switch (r.doorCount)
                    {
                        case 1:
                            m.transform.rotation = Quaternion.Euler(-90, 90, 0);
                            break;
                        case 2:
                            m.transform.rotation = Quaternion.Euler(-90, 180, 0);
                            break;
                        case 3:
                            m.transform.rotation = Quaternion.Euler(-90, 90, 0);
                            break;
                    }
                    break;
                case Direction.WEST:
                    switch (r.doorCount)
                    {
                        case 1:
                            m.transform.rotation = Quaternion.Euler(-90, -90, 0);
                            break;
                        case 2:
                            m.transform.rotation = Quaternion.Euler(-90, -90, 0);
                            break;
                        case 3:
                            m.transform.rotation = Quaternion.Euler(-90, -90, 0);
                            break;
                    }
                    break;
                case Direction.CROSS:
                    break;
                case Direction.HCROSS:
                    m.transform.rotation = Quaternion.Euler(-90, 90, 0);
                    break;
            }

            m.transform.parent = levelHolder.transform;
            
            if (!r.isDestination)
            {
                try
                {
                    if (r.location != Vector2.zero)
                    {
                        m.transform.Find("TV").gameObject.SetActive(false);
                        m.transform.Find("SecretBook").gameObject.SetActive(false);
                    }
                    }
                catch (Exception e) { }

            }
            else
            {
                try { 
                    m.transform.Find("TV").gameObject.AddComponent<TV>();
                    m.transform.Find("TV").gameObject.AddComponent<BoxCollider>();
                }
                catch (Exception e) { }
            }

        }

        //next floor style
        //s = (Style)((int)s + 1);
        return f;

	}


	void iterateDirection(ref Direction d){
		if((int)d < 3)
			d++;
		else
			d = 0;
	}

    Vector2 directionToVector(Direction d)
    {
        switch (d)
        {
            case Direction.NORTH:
                return Vector2.up;
            case Direction.SOUTH:
                return Vector2.down;
            case Direction.EAST:
                return Vector2.right;
            case Direction.WEST:
                return Vector2.left;
        }
        return Vector2.zero;
    }

}

public class Room{

	public byte doorCount = 0;
	public Direction direction = Direction.NORTH;
    public bool[] siblings = new bool[4];
	public Style style = Style.western;
	public Vector2 location = Vector2.zero;
	public bool isDestination = false;

	bool linearTwo = false;

	public Room(Room r){
        doorCount = r.doorCount;
        direction = r.direction;
        siblings = r.siblings;
        style = r.style;
        location = r.location;
        isDestination = r.isDestination;
    }
	public Room(Style style, Vector2 location){
		//this.doors = doors;
		this.style = style;
		this.location = location;
	}


}

public class Floor : List<Room>{

	public bool checkLocation(Vector2 loc){
		foreach(Room r in this)
			if(r.location == loc)
				return true;
		return false;
	}

	public byte checkDoorNum(Vector2 source){
		byte rooms = 0;
		if(checkLocation(source + new Vector2(0,1)))
			rooms++;
		if(checkLocation(source + new Vector2(0,-1)))
			rooms++;
		if(checkLocation(source + new Vector2(-1,0)))
			rooms++;
		if(checkLocation(source + new Vector2(1,0)))
			rooms++;

		return rooms;
	}

    //check for all tiles adjacent to given source
	public bool[] checkDirections(Vector2 source){
		bool[] dirs = new bool[4];
		if(checkLocation(source + new Vector2(0,1)))
			dirs[0] = true;
		if(checkLocation(source + new Vector2(0,-1)))
			dirs[1] = true;
		if(checkLocation(source + new Vector2(-1,0)))
			dirs[3] = true;
		if(checkLocation(source + new Vector2(1,0)))
			dirs[2] = true;

		return dirs;
	}

    //may actually not work
    public void updateSiblings(int i)
    {
        this[i].siblings = checkDirections(this[i].location);
    }

}

public enum Direction{
	NORTH = 0,
	SOUTH = 1,
	EAST = 2,
	WEST = 3,
    CROSS = 4,
    HCROSS = 5
}

public enum Style{
	mansion = 0,
	western = 1

}