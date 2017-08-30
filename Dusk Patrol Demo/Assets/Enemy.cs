using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Player mainPlayer;

	public float speed;

	public float health;

	TimeController t;

	List<MovementPoint> pastMoves = new List<MovementPoint>();

	List<MovementPoint> neededMoves = new List<MovementPoint> ();

	public float granularity; //determines how closely we pay attention to speed

	// Use this for initialization
	void Start () {
		t = FindObjectOfType<TimeController> ();
	}
	
	// Update is called once per frame
	void Update () {
		updateMovementLog ();
		if (t.timeFactor > 0) {
			Vector3 playerLoc = mainPlayer.gameObject.transform.position;
			Vector2 movement = new Vector2 ((playerLoc - gameObject.transform.position).x * speed * (1.1f - Random.value * 0.2f), 0);
			if (movement.magnitude > 3) {
				movement = movement.normalized * 3;
			}
			gameObject.GetComponent<Rigidbody2D> ().velocity = movement * t.timeFactor;
			addToMovementLog (movement);
			if (Mathf.Abs ((gameObject.transform.position - playerLoc).x) < 0.1) {
				shoot ();
			}
		} else if (t.timeFactor < 0) {
			foreach (MovementPoint m in neededMoves) {
				gameObject.GetComponent<Rigidbody2D> ().velocity = m.move * t.timeFactor;
			}
			neededMoves.Clear ();
		} else {
			gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		}
	}

	void shoot() {
		GameObject bulletPrefab = Resources.Load ("Bullet") as GameObject;
		GameObject newBullet = GameObject.Instantiate (bulletPrefab);
		newBullet.transform.position = gameObject.transform.position + new Vector3 (0, -1, 0);
		Bullet bscript = newBullet.GetComponent<Bullet> ();
		bscript.xspeed = 0;
		bscript.yspeed = -2;
	}

	void OnTriggerEnter2D(Collider2D other) {
		Bullet bscript = other.gameObject.GetComponent<Bullet> ();
		if (bscript) {
			this.take (bscript.damage);
			bscript.die ();
		}
	}

	void take(float d) {
		health -= d;
		if (health <= 0) {
			Destroy (gameObject);
		}
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
		if ((lastMovement.move - currentMovement).magnitude > granularity || (currentMovement.magnitude == 0 && lastMovement.move.magnitude != 0)) {
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