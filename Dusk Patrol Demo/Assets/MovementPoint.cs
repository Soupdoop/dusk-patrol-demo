using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPoint {
	public Vector2 move;
	public float time;

	public MovementPoint(Vector2 m) {
		move = m;
		time = 0;
	}

	public MovementPoint(Vector2 m, float t) {
		move = m;
		time = t;
	}
}
