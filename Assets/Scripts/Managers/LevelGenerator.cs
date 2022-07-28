using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour {
	enum gridSpace {empty, floor, wall, vertical, border};
	public Transform environmentParent;
	gridSpace[,] grid;
	int roomHeight, roomWidth;
	public int borderWidth;
	public Vector2 roomSizeWorldUnits = new Vector2(30, 30);
	public float worldUnitsInOneGridCell = 3;
	struct walker{
		public Vector2 dir;
		public Vector2 pos;
	}
	List<walker> walkers;
	public float chanceWalkerChangeDir = 0.5f, chanceWalkerSpawn = 0.05f;
	public float chanceWalkerDestoy = 0.05f;
	public int maxWalkers = 10;
	public float percentToFill = 0.2f; 

	public int enemyNum, powerUpNum;
	public GameObject verticalObj, playerObj;
	public GameObject[] wallObjs, floorObjs, borderObjs;
	private GameObject wallToSpawn, floorToSpawn, borderToSpawn;
	public float enemySpawnChance, powerUpSpawnChance, minDistanceToPlayer, distanceToPlayer;
	public GameObject[] enemies, powerUps, portals, enemiesToSpawn;
	private bool playerSpawned = false, portalSpawned = false;
	private int enemyToSpawn, powerUpToSpawn;
	public Vector2 playerPos;

	void Awake() {
		if (GameManager.Instance.isVeg) {
			enemiesToSpawn = new GameObject[] {enemies[0], enemies[1], enemies[2]};
			wallToSpawn = wallObjs[0];
			floorToSpawn = floorObjs[0];
			borderToSpawn = borderObjs[0];
		} else {
			enemiesToSpawn = new GameObject[] {enemies[3], enemies[4], enemies[5]};
			wallToSpawn = wallObjs[1];
			floorToSpawn = floorObjs[1];
			borderToSpawn = borderObjs[1];
		}
	}
	void Start () {
		StartGeneration();
	}
	void StartGeneration() {
		enemyToSpawn = enemyNum;
		powerUpToSpawn = powerUpNum;
		Setup();
		CreateFloors();
		CreateWalls();
		RemoveSingleWalls();
		SpawnLevel();
	}
	void Setup(){
		//find grid size
		roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
		roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);
		//create grid
		grid = new gridSpace[roomWidth,roomHeight];
		//set grid's default state
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
				//make every cell "empty"
				grid[x,y] = gridSpace.empty;
			}
		}
		//set first walker
		//init list
		walkers = new List<walker>();
		//create a walker 
		walker newWalker = new walker();
		newWalker.dir = RandomDirection();
		//find center of grid
		Vector2 spawnPos = new Vector2(Mathf.RoundToInt(roomWidth/ 2.0f),
										Mathf.RoundToInt(roomHeight/ 2.0f));
		newWalker.pos = spawnPos;
		//add walker to list
		walkers.Add(newWalker);
	}
	void CreateFloors(){
		int iterations = 0;//loop will not run forever
		do{
			//create floor at position of every walker
			foreach (walker myWalker in walkers){
				grid[(int)myWalker.pos.x,(int)myWalker.pos.y] = gridSpace.floor;
				// grid[(int)myWalker.pos.x + 1,(int)myWalker.pos.y + 1] = gridSpace.floor;
				// grid[(int)myWalker.pos.x - 1,(int)myWalker.pos.y - 1] = gridSpace.floor;
			}
			//chance: destroy walker
			int numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++){
				//only if its not the only one, and at a low chance
				if (Random.value < chanceWalkerDestoy && walkers.Count > 1){
					walkers.RemoveAt(i);
					break; //only destroy one per iteration
				}
			}
			//chance: walker pick new direction
			for (int i = 0; i < walkers.Count; i++){
				if (Random.value < chanceWalkerChangeDir){
					walker thisWalker = walkers[i];
					thisWalker.dir = RandomDirection();
					walkers[i] = thisWalker;
				}
			}
			//chance: spawn new walker
			numberChecks = walkers.Count; //might modify count while in this loop
			for (int i = 0; i < numberChecks; i++){
				//only if # of walkers < max, and at a low chance
				if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers){
					//create a walker 
					walker newWalker = new walker();
					newWalker.dir = RandomDirection();
					newWalker.pos = walkers[i].pos;
					walkers.Add(newWalker);
				}
			}
			//move walkers
			for (int i = 0; i < walkers.Count; i++){
				walker thisWalker = walkers[i];
				thisWalker.pos += thisWalker.dir;
				walkers[i] = thisWalker;				
			}
			//avoid boarder of grid
			for (int i =0; i < walkers.Count; i++){
				walker thisWalker = walkers[i];
				//clamp x,y to leave a 1 space boarder: leave room for walls
				thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, borderWidth, roomWidth-2-borderWidth);
				thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, borderWidth, roomHeight-2-borderWidth);
				walkers[i] = thisWalker;
			}
			//check to exit loop
			if ((float)NumberOfFloors() / (float)grid.Length > percentToFill){
				break;
			}
			iterations++;
		}while(iterations < 100000);
	}
	
	void CreateWalls(){
		//loop though every grid space
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
				//if theres a floor, check the spaces around it
				if (grid[x,y] == gridSpace.floor){
					//if any surrounding spaces are empty, place a wall
					if (grid[x,y+1] == gridSpace.empty){
						 grid[x,y+1] = gridSpace.wall;
					}
					if (grid[x,y-1] == gridSpace.empty){
						grid[x,y-1] = gridSpace.wall;
					}
					if (grid[x+1,y] == gridSpace.empty){
						grid[x+1,y] = gridSpace.wall;
					}
					if (grid[x-1,y] == gridSpace.empty){
						grid[x-1,y] = gridSpace.wall;
					}
				}
			}
		}
	}
	void RemoveSingleWalls(){
		//loop though every grid space
		for (int x = 0; x < roomWidth-1; x++){
			for (int y = 0; y < roomHeight-1; y++){
				//if theres a wall, check the spaces around it
				if (grid[x,y] == gridSpace.wall){
					//assume all space around wall are floors
					bool allFloors = true;
					//check each side to see if they are all floors
					for (int checkX = -1; checkX <= 1 ; checkX++){
						for (int checkY = -1; checkY <= 1; checkY++){
							if (x + checkX < 0 || x + checkX > roomWidth - 1 || 
								y + checkY < 0 || y + checkY > roomHeight - 1){
								//skip checks that are out of range
								continue;
							}
							if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0)){
								//skip corners and center
								continue;
							}
							if (grid[x + checkX,y+checkY] != gridSpace.floor){
								allFloors = false;
							}
						}
					}
					if (allFloors){
						grid[x,y] = gridSpace.floor;
					}
				}
			}
		}
	}
	void SpawnOnFloor(float x, float y) {
		
		Spawn(x, y, floorToSpawn, true);

		distanceToPlayer = Vector2.Distance(new Vector2(x, y), playerPos);
		if (!playerSpawned) {
			playerSpawned = true;
			playerPos = new Vector2(x, y);
			// player.GetComponent<Transform>().position = new Vector3(playerPos.x, playerPos.y, 0);
			Spawn(x, y, playerObj);
		}
		if (!portalSpawned && distanceToPlayer > minDistanceToPlayer * 1.5f) {
			Spawn(x, y, portals[0], true);
			portalSpawned = true;
			return;
		}
		if (Random.value < enemySpawnChance && enemyToSpawn > 0 && distanceToPlayer > minDistanceToPlayer) {
			int randomIndex = Random.Range(0, enemiesToSpawn.Length);
			Spawn(x, y, enemiesToSpawn[randomIndex], true);
			enemyToSpawn--;
			enemySpawnChance = 0.000f;
			return;
		}
		if (Random.value < powerUpSpawnChance && powerUpToSpawn > 0 && distanceToPlayer > minDistanceToPlayer * 1f) {
			int randomIndex = Random.Range(0, powerUps.Length);
			Spawn(x, y, powerUps[randomIndex], true);
			powerUpToSpawn--;
			powerUpSpawnChance = 0.000f;
			return;
		}

		powerUpSpawnChance += 0.01f;
		enemySpawnChance += 0.05f;
		
	}
	void SpawnLevel(){
		for (int x = 0; x < roomWidth; x++){
			for (int y = 0; y < roomHeight; y++){
				switch(grid[x,y]){
					case gridSpace.empty:
						if (x == 0 || x == roomWidth - 1 || y == 0 || y == roomHeight - 1) {
							Spawn(x, y, borderToSpawn, true);
						} else {
							Spawn(x,y, wallToSpawn, true);
						}
						break;
					case gridSpace.floor:
						SpawnOnFloor(x, y);
						break;
					case gridSpace.wall:
						Spawn(x,y, wallToSpawn, true);
						break;
				}
			}
		}
	}
	Vector2 RandomDirection(){
		//pick random int between 0 and 3
		int choice = Mathf.FloorToInt(Random.value * 3.99f);
		//use that int to chose a direction
		switch (choice){
			case 0:
				return Vector2.down;
			case 1:
				return Vector2.left;
			case 2:
				return Vector2.up;
			default:
				return Vector2.right;
		}
	}
	int NumberOfFloors(){
		int count = 0;
		foreach (gridSpace space in grid){
			if (space == gridSpace.floor){
				count++;
			}
		}
		return count;
	}
	void Spawn(float x, float y, GameObject toSpawn, bool isChild = false){
		//find the position to spawn
		Vector2 offset = roomSizeWorldUnits / 2.0f;
		Vector2 spawnPos = new Vector2(x,y) * worldUnitsInOneGridCell - offset;
		//spawn object
		if (isChild) {
			Instantiate(toSpawn, spawnPos, Quaternion.identity, environmentParent);
		} else {
			Instantiate(toSpawn, spawnPos, Quaternion.identity);
		}
	}
}