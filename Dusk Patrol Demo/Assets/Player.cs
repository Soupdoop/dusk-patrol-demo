using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 movement = new Vector2(0,0);

		if (Input.GetKey (KeyCode.W)) { //Wupwards
			movement += new Vector2(0,1);
		}
		if (Input.GetKey (KeyCode.S)) { //Sownwards
			movement += new Vector2(0,-1);
		}
		if (Input.GetKey (KeyCode.A)) { //Am Going Left
			movement += new Vector2(-1,0);
		}
		if (Input.GetKey (KeyCode.D)) { //Dright
			movement += new Vector2(1, 0);
		}
		GetComponent<Rigidbody2D> ().velocity = movement * speed;
	}
}
