using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class CubeBehavior : MonoBehaviour {
	public GameObject cubePrefab;
	public int myX, myY;

	// When the cube is clicked it'll refer to the function in my MainScript.
	void OnMouseDown () {
		MainScript.ProcessClick (gameObject, myX, myY);
	}
}