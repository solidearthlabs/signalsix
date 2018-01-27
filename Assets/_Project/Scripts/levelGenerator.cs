using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGenerator : MonoBehaviour {

	List<Floor> world = new List<Floor>();


	void Start () {
		world.Add(generateFloor(0));
	}
	


	Floor generateFloor(Style s){
		Floor f = new Floor();

        //
		int winPathCount = UnityEngine.Random.Range(6,12);
		Vector2 cursorPosition = Vector2.zero;
		Direction directionProd = 0;
        //generate the ideal path
        for (int winPath = 0; winPath < winPathCount; winPath++){
			//set direction test
			directionProd = (Direction)UnityEngine.Random.Range(0,4);

            f.Add(new Room(0, Vector2.zero));

			bool acceptable = false;
			while(!acceptable)
            {
                iterateDirection(ref directionProd);

                //edge case: stuck in a corner
                if (f.checkDoorNum(cursorPosition) == 4)
                {
                    f[winPath].isDestination = true;
                    winPath = winPathCount;
                    //don't add a room if you're stuck
                    break;
                }

                //if(there's nothing where the directionProd is)
                if (!f.checkDirections(cursorPosition)[(int)directionProd])
                    acceptable = true;

                //if we've passed, but it isn't the end of the line, just add
                if (acceptable)
                {
                    cursorPosition += directionToVector(directionProd);
                    f.Add(new Room(0, cursorPosition));
                }
            }
            
		}

        //debug path
        foreach (Room r in f)
            Debug.Log(r.location);

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
	public Style style = Style.DEFAULT;
	public Vector2 location = Vector2.zero;
	public bool isDestination = false;

	bool linearTwo = false;

	public Room(){}
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
			dirs[2] = true;
		if(checkLocation(source + new Vector2(1,0)))
			dirs[3] = true;

		return dirs;
	}
}

public enum Direction{
	NORTH = 0,
	SOUTH = 1,
	EAST = 2,
	WEST = 3
        
}

public enum Style{
	DEFAULT = 0,
	SOMETHINGELSE = 1
}