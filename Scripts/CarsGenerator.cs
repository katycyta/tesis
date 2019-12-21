using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsGenerator : MonoBehaviour {
	public GameObject carPrefab;

	public Transform[] paths;
	public float time=0f;

	const float intervalTime = 1f;

	float countDown;

	int carCount;

	bool startCountDown;

	// Use this for initialization
	void Start () {

		countDown = intervalTime;
		startCountDown = false;
		carCount = 1;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (countDown > 0f) {
			countDown -= Time.deltaTime;
		} else {
			//if (Random.Range (0, 10) > 5) {
			int random = Random.Range (0, paths.Length);
			if (checkIfPosEmpty(paths [random].GetComponentsInChildren<Transform> () [1].position,random)) {
				

				GameObject newCar = Instantiate (carPrefab, paths [random].GetComponentsInChildren<Transform> () [1].position, Quaternion.identity) as GameObject;
				newCar.name = "car" + carCount.ToString ();
				newCar.GetComponent<CarEngine> ().path = paths [random];

				/*if (carCount == 1) {
					newCar.GetComponent<CarEngine> ().currentTrack.GetComponent<Track> ().trafficLight.GetComponent<SmartTrafficLight> ().isInactive = false;
					newCar.GetComponent<CarEngine> ().currentTrack.GetComponent<Track> ().trafficLight.GetComponent<SmartTrafficLight> ().junctionAgent.GetComponent<SmartTrafficLight> ().isTransition = true;
				}*/
				++carCount;
			}

			//}

			countDown = intervalTime;
		}
	}

	public bool checkIfPosEmpty(Vector3 targetPos, int rand)
	{
		GameObject[] cars = GameObject.FindGameObjectsWithTag ("Car");
		foreach (GameObject current in cars) {
			if (Vector3.Distance (current.transform.position, targetPos) < 7f) {
				if (Vector3.Distance(current.GetComponent<CarEngine>().path.GetComponentsInChildren<Transform> () [1].position, paths [rand].GetComponentsInChildren<Transform> () [1].position)< 1f)
					return false;
			}
		}
		return true;
	}
}
