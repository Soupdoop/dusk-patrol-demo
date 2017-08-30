using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float xspeed;
	public float yspeed;

	public float damage;

	bool ded;
	float timeSinceDeath,timeSinceBirth;

	TimeController t;

	// Use this for initialization
	void Start () {
		t = FindObjectOfType<TimeController> ();
		ded = false;
		timeSinceDeath = 0;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D> ().velocity = new Vector2 (xspeed, yspeed) * t.timeFactor;
		if (ded) {
			if (timeSinceDeath > t.maxTimeBack) {
				Destroy (gameObject); //Can't return
			}
			if (timeSinceDeath <= 0) {
				respawn ();
			}
			timeSinceDeath += Time.deltaTime * t.timeFactor;
		}
		timeSinceBirth += Time.deltaTime * t.timeFactor;
		if (timeSinceBirth < 0) {
			Destroy (gameObject);
		}
	}

	public void die() {
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<BoxCollider2D> ().enabled = false;
		ded = true;
	}

	public void respawn() {
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		ded = false;
	}
}
