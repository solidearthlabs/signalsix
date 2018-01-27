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

		//generate the ideal path
		int winPathCount = UnityEngine.Random.Range(6,12);
		Vector2 cursorPosition = Vector2.zero;
		Direction directionProd = 0;
		Room filler = null;
		for(int winPath = 0; winPath < winPathCount; winPath++){
			//set direction test
			directionProd = (Direction)UnityEngine.Random.Range(0,3);
			byte doorCount = f.checkDoorNum(cursorPosition);
			filler = null;

			bool noAdd = true;
			while(noAdd){
				switch(doorCount){
					case 0:
						filler = new Room(0,new Vector2(0,0));
						break;
					case 1:
						if(f.checkDirections(cursorPosition)[(int)directionProd])
							break;
						break;
					case 2:
						if(f.checkDirections(cursorPosition)[0] && directionProd == (Direction.NORTH))
							filler = new Room(0, new Vector2(0,0));
						break;
					case 3:
						break;
					case 4:
						f[winPath].isDestination = true;
						winPath = winPathCount;
						filler = new Room(0, new Vector2(0,0));
						break;
				}
				iterateDirection(ref directionProd);
			}

			//if(filler.location!=null)
				f.Add(filler);


		}

		return f;

	}


	void iterateDirection(ref Direction d){
		if((int)d < 3)
			d++;
		else
			d = 0;
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