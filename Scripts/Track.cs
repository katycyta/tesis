using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

	public bool hasTrafficLight;
	public int carsCount;
	public int currentCars;
	public int maxCarArea;
	public GameObject trafficLight;

	public float timeTotal;
	public float timeAverage;
	public float stopTotal;
	public float stopAverage;

	// Use this for initialization
	void Start () {
		if (trafficLight)
			hasTrafficLight = true;
		else
			hasTrafficLight = false;
		carsCount = 0;
		timeAverage= 0f;
		timeTotal= 0f;

		stopTotal= 0f;
		stopAverage = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Car") {
			++carsCount;
			++currentCars;

			if(hasTrafficLight)
				trafficLight.GetComponent<SmartTrafficLight> ().addTimePerCar ();
		}
			
	}

	void OnCollisionExit(Collision col){
		if (col.gameObject.tag == "Car") {
			--currentCars;
			timeTotal += col.gameObject.GetComponent<CarEngine> ().timeWaiting;
			timeAverage = timeTotal / (carsCount-currentCars);

			stopTotal += col.gameObject.GetComponent<CarEngine> ().NumStop;
			stopAverage = stopTotal / (carsCount-currentCars);
		}
	}

}
