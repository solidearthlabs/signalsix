using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGenerator : MonoBehaviour {

	List<Floor> world = new List<Floor>();
    public GameObject levelHolder;

    int currentFloor = 0;
	void Start () {
        levelHolder = new GameObject();

        for (currentFloor = 0; currentFloor<1; currentFloor++)
		    world.Add(generateFloor(0));
	}
	


	Floor generateFloor(Style s){
		Floor f = new Floor();

        f.Add(new Room(0, Vector2.zero));


        //
        int winPathCount = UnityEngine.Random.Range(6,10);
        Debug.Log(winPathCount);

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
                    f.Add(new Room(0, cursorPosition));

                    //bool[] debugDirs = f.checkDirections(cursorPosition);
                    //Debug.Log("lcoation:" + cursorPosition + " direction: " + directionProd + " sides: " + debugDirs[0] + " " + debugDirs[1] + " " + debugDirs[2] + " " + debugDirs[3]);
                }
            }
            
		}


        //Add sister rooms
        /*
        foreach (Room r in f)
        {
            if (UnityEngine.Random.Range(0, 1) <= .7f)
            {
                Direction prod = Random
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
                    if (f[i].siblings[0] && f[i].siblings[1] || f[i].siblings[2] && f[i].siblings[3])
                        f[i].direction = Direction.CROSS;
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
            //randomize loads
            GameObject m = Instantiate(Resources.Load<GameObject>("testAssetDONOTUSE"));
            m.transform.position = new Vector3(r.location.x * -6.4f, 40*currentFloor, r.location.y * -6.4f);
            m.name = r.location.x + " " + r.location.y;
            //set room directions
            m.transform.rotation = Quaternion.Euler(-90,0,0);

            m.transform.parent = levelHolder.transform;
            
        }

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
	public Style style = Style.DEFAULT;
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
    CROSS = 4
}

public enum Style{
	DEFAULT = 0,
	SOMETHINGELSE = 1
}