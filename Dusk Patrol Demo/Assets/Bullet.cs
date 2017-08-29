using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float xspeed;
	public float yspeed;

	public float damage;

	TimeController t;

	// Use this for initialization
	void Start () {
		t = FindObjectOfType<TimeController> ();
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (xspeed, yspeed) * t.timeFactor;
	}

	public void die() {
		Destroy (gameObject);
	}
}
