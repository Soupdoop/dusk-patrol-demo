using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

	public float timeFactor;
	public float maxTimeBack;

	float timeBack;

	// Use this for initialization
	void Start () {
		timeBack = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			if (timeBack < maxTimeBack) {
				timeFactor = -1;
				timeBack += Time.deltaTime;
			} else {
				timeFactor = 0;
			}
		} else {
			timeFactor = 1;
			timeBack = Mathf.Max (0, timeBack - Time.deltaTime);
		}
	}
}
