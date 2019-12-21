using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junction : MonoBehaviour {

	public GameObject[] mAgents=new GameObject[2];
	bool init=false;

	// Use this for initialization
	void Start () {
		init = false;

	}
	
	// Update is called once per frame
	void Update () {

		if (!init) {
			if (mAgents [0].GetComponent<SmartTrafficLight> ().track.GetComponent<Track> ().currentCars > 0) {

				mAgents [1].GetComponent<SmartTrafficLight> ().countDown = 1f;
				mAgents [1].GetComponent<SmartTrafficLight> ().isActive = true;
				mAgents [0].GetComponent<SmartTrafficLight> ().isInactive = true;

				init = true;
			}
			else if (mAgents [1].GetComponent<SmartTrafficLight> ().track.GetComponent<Track> ().currentCars > 0) {

				mAgents [0].GetComponent<SmartTrafficLight> ().countDown = 1f;

				mAgents [0].GetComponent<SmartTrafficLight> ().isActive = true;
				mAgents [1].GetComponent<SmartTrafficLight> ().isInactive = true;

				init = true;
			}
		}
		
		
	}
}
