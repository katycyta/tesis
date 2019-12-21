using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarEngine : MonoBehaviour {
	
    public Transform path;
	public GameObject currentTrack;

	public float timeWaiting;
	public int NumStop=0;
	public bool incStop=false;
   

    [Header("Sensors")]
    public float sensorLength = 3f;
    public Vector3 frontSensorPosition = new Vector3(0f, 0.1f, 0.5f);
    public float frontSideSensorPosition = 0.2f;
    public float frontSensorAngle = 30f;

    private List<Transform> nodes;
    public int currentNode = 0;
	private bool stopping = false;

    private void Start() {
		timeWaiting = 0f;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++) {
            if (pathTransforms[i] != path.transform) {
                nodes.Add(pathTransforms[i]);
            }
        }

    }

    private void Update() {
		timeWaiting += Time.deltaTime;
        
        ApplySteer();
		Sensors();
        Drive();
        CheckWaypointDistance();
    }

    private void Sensors() {
		//if (!inCross) {
			if (currentTrack) {
				if (currentTrack.GetComponent<Track> ().hasTrafficLight) {
					//if (!currentTrack.GetComponent<Track> ().trafficLight.GetComponent<TrafficLight> ().mActive [0] && stopping) {
					if (!currentTrack.GetComponent<Track> ().trafficLight.GetComponent<SmartTrafficLight> ().isActive && stopping) {
					if (!incStop) {
						++NumStop;
						incStop = true;
					}
					return;
					} 
				}
			}
		//}
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
		stopping = false;

        //front right sensor
        sensorStartPos += transform.right * frontSideSensorPosition;


        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
			if (hit.collider.CompareTag("Car")) {
				Debug.DrawLine(sensorStartPos, hit.point);
				stopping = true;
					//Debug.Log (gameObject.name+" collision of right sensor: "+hit.collider.gameObject.name);
				

				//Debug.Log (gameObject.name+" collision of right sensor: "+hit.collider.gameObject.name);

			}


        }

        //front right angle sensor
       /* else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) {
			if (hit.collider.CompareTag("Car")) {
				Debug.DrawLine(sensorStartPos, hit.point);
				stopping = true;

				//	Debug.Log (gameObject.name + " collision of right angle sensor: " + hit.collider.gameObject.name);

			}
        }*/

        //front left sensor
        sensorStartPos -= transform.right * frontSideSensorPosition * 2;
		if (Physics.Raycast (sensorStartPos, transform.forward, out hit, sensorLength)) {
			if (hit.collider.CompareTag ("Car")) {
				Debug.DrawLine (sensorStartPos, hit.point);
				stopping = true;

				//	Debug.Log (gameObject.name + " collision of left sensor: " + hit.collider.gameObject.name);

			}
		}

        //front left angle sensor
        /*else if (Physics.Raycast (sensorStartPos, Quaternion.AngleAxis (-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)) {
			if (hit.collider.CompareTag ("Car")) {
				Debug.DrawLine (sensorStartPos, hit.point);
				stopping = true;

				//Debug.Log (gameObject.name + " collision of left angle sensor: " + hit.collider.gameObject.name);

			} 
		} */

		incStop = false;
			
    }

    private void ApplySteer() {
		Vector3 targetDir = nodes [currentNode].position -  transform.position;
		targetDir.y = 0f;
		transform.eulerAngles = new Vector3(0f,Vector3.SignedAngle(Vector3.forward,targetDir,Vector3.up),0f);



    }

    private void Drive() {
		if (stopping) 
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		else
			GetComponent<Rigidbody>().velocity = transform.forward*8f;
		
    }

    private void CheckWaypointDistance() {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 1f) {
            if (currentNode == nodes.Count - 1) {
				Destroy (gameObject);
            } else {
                currentNode++;
            }
        }
    }

   
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Track") {
			//Debug.Log (gameObject.name+" Enter of : "+col.gameObject.name);
			currentTrack = col.gameObject;
			timeWaiting = 0;
			NumStop = 0;

		}
		else if (col.gameObject.tag == "Cross") {
			//Debug.Log (gameObject.name+" Enter of : "+col.gameObject.name);
			if (currentTrack.GetComponent<Track> ().hasTrafficLight) {
				//if (currentTrack.GetComponent<Track> ().trafficLight.GetComponent<TrafficLight> ().mActive [2] ||
				//	currentTrack.GetComponent<Track> ().trafficLight.GetComponent<TrafficLight> ().mActive [1] ) {
				if (currentTrack.GetComponent<Track> ().trafficLight.GetComponent<SmartTrafficLight> ().isInactive ||
					currentTrack.GetComponent<Track> ().trafficLight.GetComponent<SmartTrafficLight> ().isTransition ) {

					stopping = true;
					++NumStop;
					incStop = true;


				}

				++currentTrack.GetComponent<Track> ().trafficLight.GetComponent<SmartTrafficLight> ().queue;
				//else {
					
				//}

			}
		}
	}

	void OnCollisionExit(Collision col){
		if (col.gameObject.tag == "Track") {
			//Debug.Log (gameObject.name+" Exit of : "+col.gameObject.name);
			if (currentTrack.GetComponent<Track> ().hasTrafficLight) {

				--currentTrack.GetComponent<Track> ().trafficLight.GetComponent<SmartTrafficLight> ().queue;
			}
			currentTrack =null;


		}
		else if (col.gameObject.tag == "Cross") {
			//Debug.Log (gameObject.name+" Exit of : "+col.gameObject.name);


		}
	}
}
