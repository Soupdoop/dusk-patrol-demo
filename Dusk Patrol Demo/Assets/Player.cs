using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float speed;

	public float health;

	TimeController t;

	List<MovementPoint> pastMoves = new List<MovementPoint>();

	List<MovementPoint> neededMoves = new List<MovementPoint> ();

	public float granularity; //determines how closely we pay attention to speed

	bool wasForwards;

	// Use this for initialization
	void Start () {
		t = FindObjectOfType<TimeController> ();
		wasForwards = true;
	}
	
	// Update is called once per frame
	void Update () {

		updateMovementLog ();

		if (health <= 0) {
			Destroy (gameObject);
		}

		if (t.timeFactor > 0) {
			wasForwards = true;
			Vector2 movement = new Vector2 (0, 0);

			if (Input.GetKey (KeyCode.W)) { //Wupwards
				movement += new Vector2 (0, 1);
			}
			if (Input.GetKey (KeyCode.S)) { //Sownwards
				movement += new Vector2 (0, -1);
			}
			if (Input.GetKey (KeyCode.A)) { //Am Going Left
				movement += new Vector2 (-1, 0);
			}
			if (Input.GetKey (KeyCode.D)) { //Dright
				movement += new Vector2 (1, 0);
			}

			if (Input.GetKeyDown (KeyCode.Space)) {
				shootBullet ();
			}

			addToMovementLog (movement * speed);
			GetComponent<Rigidbody2D> ().velocity = movement * speed;
		} else if (t.timeFactor == 0) {
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		} else {
			//gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			if (wasForwards) {
				gameObject.GetComponent<Rigidbody2D> ().velocity *= t.timeFactor;
				wasForwards = false;
			}
			foreach (MovementPoint m in neededMoves) {
				gameObject.GetComponent<Rigidbody2D> ().velocity = m.move * t.timeFactor;
			}
			neededMoves.Clear ();
		}
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

	void updateMovementLog() {
		for(int i = pastMoves.Count - 1; i > -1; i--) {
			MovementPoint point = pastMoves [i];
			point.time += Time.deltaTime * t.timeFactor;
			if (point.time < 0) {
				neededMoves.Add (point);
				pastMoves.Remove (point);
			} else if (point.time > t.maxTimeBack) {
				pastMoves.Remove (point);
			}
		}
	}

	void addToMovementLog(Vector2 currentMovement) {
		MovementPoint lastMovement = getLatestMovement ();
		if (lastMovement == null) {
			pastMoves.Add (new MovementPoint(currentMovement));
			return;
		}
		if ((lastMovement.move - currentMovement).magnitude > granularity || (currentMovement.magnitude == 0 && lastMovement.move.magnitude != 0) || (lastMovement.time > granularity)) {
			MovementPoint newMovement = new MovementPoint (currentMovement);
			pastMoves.Add (newMovement);
		}
	}

	MovementPoint getLatestMovement() {
		if (pastMoves.Count == 0)
			return null;
		MovementPoint latest = pastMoves [0];
		foreach (MovementPoint p in pastMoves) {
			if (p.time < latest.time) {
				latest = p;
			}
		}
		return latest;
	}
}
