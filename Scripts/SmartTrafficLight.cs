using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SmartTrafficLight : MonoBehaviour {

	public GameObject track;

	public int queue = 0;

	public float maxGreenTime;
	public float minGreenTime;

	public GameObject[] mLight = new GameObject[3];
	public Color[] mColor = new Color[4];


	public GameObject junctionAgent;

	const float timePerCar = 6f;
	const int  numOfAreas = 9;

	const float maxTime = 30f;
	const float minTime = 2f;
	const float transitionTime = 3f;



	public float countDown;
	public float countTotalGreen =0f;

	public float totalTime;
	public float InitialTime;

	public bool isActive;
	public bool isTransition;
	public bool isInactive;


	public bool sendReactive = false;


	// Use this for initialization
	void Start () {
		
		isActive = false;
		isInactive = false;
		isTransition = false;

		countDown = minTime;
		countTotalGreen = 0f;
		totalTime = 0f;
		InitialTime = minTime;

		maxGreenTime = 0f;
		minGreenTime = 100f;

		mColor [0] = Color.green;
		mColor [1] = Color.yellow;
		mColor [2] = Color.red;
		mColor [3] = Color.white;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (isActive) {
			countTotalGreen += Time.deltaTime;
			sendReactive = false;
			mLight [0].GetComponent<Renderer> ().material.color = mColor [0];
			mLight [1].GetComponent<Renderer> ().material.color = mColor [3];
			mLight [2].GetComponent<Renderer> ().material.color = mColor [3];



			if (countDown > 0) {
				if (junctionAgent.GetComponent<SmartTrafficLight> ().track.GetComponent<Track> ().currentCars > 0) {
					if ((track.GetComponent<Track> ().currentCars < 3) && (queue == 0) && (track.GetComponent<Track> ().maxCarArea < 2)

						&& ((junctionAgent.GetComponent<SmartTrafficLight> ().computeTime () + transitionTime < computeTime ())
							|| (junctionAgent.GetComponent<SmartTrafficLight> ().queue > 0))) {

						if (countTotalGreen < minGreenTime) {
							if (countTotalGreen>1)
								minGreenTime = countTotalGreen;
						}
						if (countTotalGreen > maxGreenTime) {
							maxGreenTime = countTotalGreen;
						}

						countTotalGreen = 0f;


						countDown = transitionTime;
						isActive = false;
						isTransition = true;
						isInactive = false;
						totalTime = 0;


					}
				}


				countDown -= Time.deltaTime;


			} else {
				if (countTotalGreen < minGreenTime) {
					if (countTotalGreen>1)
						minGreenTime = countTotalGreen;
				}
				if (countTotalGreen > maxGreenTime) {
					maxGreenTime = countTotalGreen;
				}
				countTotalGreen = 0f;


				countDown = transitionTime;
				isActive = false;
				isTransition = true;
				isInactive = false;
				totalTime = 0;


			}




		} else if (isTransition) {
			mLight[0].GetComponent<Renderer>().material.color = mColor [3];
			mLight[1].GetComponent<Renderer>().material.color = mColor [1];
			mLight[2].GetComponent<Renderer>().material.color = mColor [3];

			if (countDown > 0) {
				countDown -= Time.deltaTime;
			} else {
				isActive = false;
				isTransition = false;
				isInactive = true;
				countDown = junctionAgent.GetComponent<SmartTrafficLight> ().InitialTime;

				junctionAgent.GetComponent<SmartTrafficLight> ().isActive = true;
				junctionAgent.GetComponent<SmartTrafficLight> ().isTransition = false;
				junctionAgent.GetComponent<SmartTrafficLight> ().isInactive = false;

				junctionAgent.GetComponent<SmartTrafficLight> ().InitialTime=junctionAgent.GetComponent<SmartTrafficLight> ().computeTime();
				if (junctionAgent.GetComponent<SmartTrafficLight> ().InitialTime < minTime) 
					junctionAgent.GetComponent<SmartTrafficLight> ().InitialTime = minTime;
				

				junctionAgent.GetComponent<SmartTrafficLight> ().totalTime = junctionAgent.GetComponent<SmartTrafficLight> ().InitialTime;
				if (junctionAgent.GetComponent<SmartTrafficLight> ().InitialTime<= maxTime)
					junctionAgent.GetComponent<SmartTrafficLight> ().countDown = junctionAgent.GetComponent<SmartTrafficLight> ().InitialTime;
				else
					junctionAgent.GetComponent<SmartTrafficLight> ().countDown = maxTime;
			}
		}
		else if (isInactive){
			mLight[0].GetComponent<Renderer>().material.color = mColor [3];
			mLight[1].GetComponent<Renderer>().material.color = mColor [3];
			mLight[2].GetComponent<Renderer>().material.color = mColor [2];

			InitialTime=timePerCar - (junctionAgent.GetComponent<SmartTrafficLight> ().track.GetComponent<Track>().maxCarArea*(timePerCar / numOfAreas));

			if (countDown > 0) {
				countDown -= Time.deltaTime;
			} else {
				if (!sendReactive) {
					if ((queue > 0) &&(junctionAgent.GetComponent<SmartTrafficLight> ().queue == 0)) {
						Debug.Log (gameObject.name+" Primer if: ");

						junctionAgent.GetComponent<SmartTrafficLight> ().isActive = false;
						junctionAgent.GetComponent<SmartTrafficLight> ().isTransition = true;
						junctionAgent.GetComponent<SmartTrafficLight> ().isInactive = false;

						junctionAgent.GetComponent<SmartTrafficLight> ().countDown = transitionTime;
						//junctionAgent.GetComponent<SmartTrafficLight> ().countDown = 0;

						sendReactive = true;
					}
					//else if ((queue > 0) && (InitialTime +transitionTime  < junctionAgent.GetComponent<SmartTrafficLight> ().totalTime - junctionAgent.GetComponent<SmartTrafficLight> ().InitialTime)) {
					//else if ((queue > 0) && (track.GetComponent<Track>().maxCarArea > junctionAgent.GetComponent<SmartTrafficLight> ().track.GetComponent<Track>().maxCarArea)) {
					else if ((queue > 0) && (computeTime()+transitionTime < junctionAgent.GetComponent<SmartTrafficLight> ().computeTime())) {
						Debug.Log (gameObject.name+" segundo if: ");
						junctionAgent.GetComponent<SmartTrafficLight> ().isActive = false;
						junctionAgent.GetComponent<SmartTrafficLight> ().isTransition = true;
						junctionAgent.GetComponent<SmartTrafficLight> ().isInactive = false;
						junctionAgent.GetComponent<SmartTrafficLight> ().countDown = transitionTime;
						sendReactive = true;
						//junctionAgent.GetComponent<SmartTrafficLight> ().countDown = 0;
					}


					else if ((queue == 0) && (junctionAgent.GetComponent<SmartTrafficLight> ().queue == 0)) {
						Debug.Log (gameObject.name+" tercero if: ");
						//if (InitialTime + transitionTime > junctionAgent.GetComponent<SmartTrafficLight> ().totalTime - junctionAgent.GetComponent<SmartTrafficLight> ().InitialTime) {
						//if (track.GetComponent<Track>().maxCarArea > junctionAgent.GetComponent<SmartTrafficLight> ().track.GetComponent<Track>().maxCarArea){
						if ((track.GetComponent<Track>().currentCars>0) && ((computeTime()+transitionTime) < junctionAgent.GetComponent<SmartTrafficLight> ().computeTime())){
							Debug.Log (gameObject.name+" Primer if del  if: ");
							junctionAgent.GetComponent<SmartTrafficLight> ().isActive = false;
							junctionAgent.GetComponent<SmartTrafficLight> ().isTransition = true;
							junctionAgent.GetComponent<SmartTrafficLight> ().isInactive = false;
							junctionAgent.GetComponent<SmartTrafficLight> ().countDown = transitionTime;
							
							//junctionAgent.GetComponent<SmartTrafficLight> ().countDown = 0;
						
							sendReactive = true;
						}
					}

				}
			}


				 
		}

	}

	public void addTimePerCar(){
		if (isActive) {
			totalTime += timePerCar;

			if (totalTime <= maxTime) {
				countDown += timePerCar;
			}
			else if(junctionAgent.GetComponent<SmartTrafficLight>().track.GetComponent<Track>().currentCars==0){
				countDown += timePerCar;
			}
		}

/*		else {
			++InitialCars;
			//InitialTime += timePerCar*(timePerCar/numOfAreas);
		}*/
	}

	public float computeTime (){

		return timePerCar - (track.GetComponent<Track>().maxCarArea*(timePerCar / numOfAreas));
	}


}
