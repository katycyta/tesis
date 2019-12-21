using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarArea : MonoBehaviour {
	int myCarArea;


	// Use this for initialization
	void Start () {
		myCarArea = int.Parse(gameObject.name [gameObject.name.Length - 1].ToString());
		Debug.Log (myCarArea);


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Car") {
			if (myCarArea < transform.parent.GetComponent<Track> ().maxCarArea)
				transform.parent.GetComponent<Track> ().maxCarArea = myCarArea;
		}

	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Car") {
			if (myCarArea == transform.parent.GetComponent<Track> ().maxCarArea) {
				if (myCarArea < 8) {
					++transform.parent.GetComponent<Track> ().maxCarArea;
				}
			}

		}
	}
}
