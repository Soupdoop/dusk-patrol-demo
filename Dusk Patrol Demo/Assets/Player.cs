using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed;

	public float health;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (health <= 0) {
			Destroy (gameObject);
		}

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

		if (Input.GetKeyDown (KeyCode.Space)) {
			shootBullet ();
		}
		GetComponent<Rigidbody2D> ().velocity = movement * speed;
	}

	void take(float d) {
		health -= d;
	}

	void OnTriggerEnter2D(Collider2D other) {
		Bullet bscript = other.gameObject.GetComponent<Bullet> ();
		if (bscript) {
			this.take (bscript.damage);
			bscript.die ();
		}
	}

	void shootBullet() {
		GameObject bulletPrefab = Resources.Load ("Bullet") as GameObject;
		GameObject newBullet = GameObject.Instantiate (bulletPrefab);
		newBullet.transform.position = gameObject.transform.position + new Vector3 (0, 1, 0);
		Bullet bscript = newBullet.GetComponent<Bullet> ();
		bscript.xspeed = 0;
		bscript.yspeed = 2;
	}
}
