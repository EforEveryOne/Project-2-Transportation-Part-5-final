using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour {
	public GameObject cubePrefab;
	public Text ScoreText;
	Vector3 cubePosition;
	public static GameObject clickedCube;
	public static GameObject airplaneCube;
	static int airplaneX, airplaneY, startX, startY, depositX, depositY;
	int storedCargo, maxCargo, loadCargo;
	public float turnTime, turnCounter;
	int score;
	public int x, y;
	static bool activeAirPlane;
	static GameObject[,] grid;
	int gridX, gridY;
	static int targetX, targetY;
	int moveY, moveX;

// this is a built in function, everthing starts to happen after this.
	void Start () {
		
		gridX = 16;
		gridY = 9;
		grid = new GameObject [gridX, gridY];

		storedCargo = 0;
		maxCargo = 90;
		loadCargo = 10;

		score = 0;

		moveY = 0;
		moveX = 0;

		turnTime = 1.5f;
		turnCounter = turnTime;

		for (int y = 0; y < gridY; y++) {
			for (int x = 0; x < gridX; x++) {
				cubePosition = new Vector3 (x * 2, y * 2, 0);
				grid[x,y] = Instantiate (cubePrefab, cubePosition, Quaternion.identity);
				grid [x, y].GetComponent<CubeBehavior> ().myX = x;
				grid [x, y].GetComponent<CubeBehavior> ().myY = y;
			}
		}

//starting coordinates for a plane
		startX = 0;
	    startY = 8;
		airplaneX = startX;
		airplaneY = startY;
		targetX = airplaneX;
		targetY = airplaneY;

			grid[airplaneX, airplaneY].GetComponent<Renderer> ().material.color = Color.red;
		    activeAirPlane = false;

//Drop off site location + style
		depositX = 15;
		depositY = 0;
		grid[depositX, depositY].GetComponent<Renderer> ().material.color = Color.black;
		}

// This is a function. No more confusion between functions and variables! When the mouse is clicked on a cube (checked by the script on each cube) the following will happen in order
	public static void ProcessClick (GameObject clickedCube, int x, int y) {
//checking if airplane is clicked
		if (x == airplaneX && y == airplaneY) {

//if it's active, make it inactive, turn red, and shrink.
			if (activeAirPlane) {
				activeAirPlane = false;
				clickedCube.transform.localScale -= new Vector3 (0.2f, 0.2f, 0.2f);
				clickedCube.GetComponent<Renderer> ().material.color = Color.red;
			}
//if it's inactive, make it active, turn green, and grow.
			else {
				activeAirPlane = true;
				clickedCube.transform.localScale += new Vector3 (0.2f, 0.2f, 0.2f);
				clickedCube.GetComponent<Renderer> ().material.color = Color.green;
			}
		}
		else if (activeAirPlane) {
			targetX = x;
			targetY = y;
		}
	}

	void CalculateDirection () {
		if (airplaneY > targetY) {
			moveY = -1;
		} else if (airplaneY < targetY) {
			moveY = 1;
		}
		else {
			moveY = 0;
		}

		if (airplaneX < targetX) {
			moveX = 1;
		} else if (airplaneX > targetX) {
			moveX = -1;
		}
		else {
			moveX = 0;
		}
	}

	void MoveAirplane () {
		CalculateDirection ();

		if (airplaneX + moveX > gridX) {
			moveX = 0;
		}

		if (activeAirPlane) {

				grid [airplaneX, airplaneY].GetComponent<Renderer> ().material.color = Color.white;
				grid [airplaneX, airplaneY].transform.localScale -= new Vector3 (0.2f, 0.2f, 0.2f);
				grid[depositX, depositY].GetComponent<Renderer> ().material.color = Color.black;

			//put airplane in new position
			airplaneX = airplaneX + moveX;
			airplaneY = airplaneY + moveY;

			//keep airplane on the grid.
			if (airplaneX >= gridX) {
				airplaneX = gridX - 1;
			} 
			else if (airplaneX < 0) {
				airplaneX = 0;
			}


			if (airplaneY >= gridY) {
				airplaneY = gridY - 1;
			} 
			else if (airplaneY < 0) {
				airplaneY = 0;
			}
				
				grid [airplaneX, airplaneY].GetComponent<Renderer> ().material.color = Color.green;
				grid [airplaneX, airplaneY].transform.localScale += new Vector3 (0.2f, 0.2f, 0.2f);
			}
		}

	//This section is keeping track of turns which determine the rate the player acquires cargo and how often they can move.
   void Update () {
		if (Time.time > turnCounter) {
			MoveAirplane ();

			if (airplaneX == startX && airplaneY == startY && activeAirPlane == true && storedCargo < maxCargo)
				
				storedCargo += loadCargo;
			ScoreText.text = ("Cargo: " + storedCargo + "  Score: " + score);

			turnCounter += turnTime;
	}
			if (airplaneX == depositX && airplaneY == depositY && activeAirPlane == true && storedCargo > 0) {
				score += storedCargo;
				storedCargo = 0;
			}
	}
}