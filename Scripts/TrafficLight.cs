using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour {

	public GameObject[] mLight = new GameObject[3];

	public Color[] mColor = new Color[3];


	float mTimer = 0.0f;

	// 0 =G  1=Y  2=R
	float[] mTime = new float[3];

	public int initial;
	// 0 =G  1=Y  2=R
	public bool[] mActive = new bool[3];

	// Use this for initialization
	void Start () {
		mActive[0] =false;
		mActive[1] =false;
		mActive[2] =false;

		mTime [0] = 25f;
		mTime [1] = 3f;
		mTime [2] = 28f;

		mColor [0] = Color.green;
		mColor [1] = Color.yellow;
		mColor [2] = Color.red;


		mActive[initial]=true;

		mTimer = mTime [initial];

		mLight[initial].GetComponent<Renderer>().material.color= mColor [initial];

	}
	
	// Update is called once per frame
	void Update () {




		if (mTimer > 0) {
			mTimer -= Time.deltaTime;

	
		}

		else if (mTimer <= 0) {
			if (mActive[0]) {
				mActive[0] = false;
				mActive[1] = true;
				mTimer = mTime [1];

				//mGreen.GetComponent<Renderer>().material.shader = Shader.Find("Specular");
				mLight[0].GetComponent<Renderer>().material.color=Color.white;

				//mYellow.GetComponent<Renderer>().material.shader = Shader.Find("Specular");
				mLight[1].GetComponent<Renderer>().material.color=mColor [1];

			}

			else if (mActive[1]) {
				mActive[1] = false;
				mActive[2] = true;
				mTimer = mTime [2];	

				mLight[1].GetComponent<Renderer>().material.color = Color.white;

				mLight[2].GetComponent<Renderer>().material.color=mColor [2];


			}

			else if (mActive[2]) {
				mActive[2] = false;
				mActive[0] = true;
				mTimer = mTime [0];


				mLight[2].GetComponent<Renderer>().material.color=Color.white;

				mLight[0].GetComponent<Renderer>().material.color= mColor [0];
			}
				


		}

	}
}

