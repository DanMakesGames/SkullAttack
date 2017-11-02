using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAttackGameMode : MonoBehaviour {
	Enemy[] enemyArray;
	// Use this for initialization
	void Start () {
		enemyArray = GameObject.FindObjectsOfType<Enemy> ();
	}
	
	// Update is called once per frame
	void Update () {
		int count = 0;
		for(int index = 0; index < enemyArray.Length; index++) {
			if (enemyArray [index] != null)
				count++;
		}

		if (count == 0) {
			//Application.Quit ();
		}
		
	}
}
