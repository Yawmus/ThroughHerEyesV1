using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class UIManager : MonoBehaviour
{
	public enum Age
	{
		CHILD,
		TEENAGER,
		ADULT
	}
	public GameObject PausePanel;
	public GameObject DebugPanel;
	public GameObject SpotPanel;
    public bool paused;
	public int spot = 1;
	public Age age = Age.TEENAGER;
	float ageDelay = .3f, spotDelay = .3f, restDelay = .3f;
	// Use this for initialization
	void Start ()
    {
        paused = false;
	}
	void Awake(){
		if (Application.loadedLevel != 0) {
			SpotPanel.transform.FindChild ("1").gameObject.SetActive (false);
			SpotPanel.transform.FindChild ("2").gameObject.SetActive (false);
			if (spot != 0)
				SpotPanel.transform.FindChild (spot + "").gameObject.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(paused)
        {
            gamePaused(true);
        }
        else
        {
            gamePaused(false);
        }
        if(Input.GetButtonDown("Menu"))
        {
            pauseSwitch();
		}

		// Spot Handler
		spotDelay += Time.deltaTime;
		if (Input.GetButton ("Spot") && spotDelay > .3f) {
			SpotPanel.transform.FindChild("1").gameObject.SetActive(false);
			SpotPanel.transform.FindChild("2").gameObject.SetActive(false);
			spot = (spot + 1) % 2;
			if(spot != 0)
				SpotPanel.transform.FindChild(spot + "").gameObject.SetActive(true);
			spotDelay = 0;
		}
		DebugPanel.transform.FindChild ("Spot Intensity").GetComponent<Text> ().text = "Spot Intensity: " + spot;

		restDelay += Time.deltaTime;
		if (Input.GetButton ("Restart") && restDelay > .3f) {
			Application.LoadLevel(Application.loadedLevel);
		}
		if (Input.GetButton ("m1") ) {
			Application.LoadLevel("Mission 1");
		}
		if (Input.GetButton ("m2") ) {
			Application.LoadLevel("Mission 2");
		}
		if (Input.GetButton ("m3") ) {
			Application.LoadLevel("Mission 3");
		}


		ageDelay += Time.deltaTime;
		if (Input.GetButton ("Age") && ageDelay > .3f) {
			Camera.main.GetComponent<Blur>().enabled = !Camera.main.GetComponent<Blur>().enabled;

			/*CharacterController cc = GameObject.Find ("Player").gameObject.GetComponent<CharacterController>();
			switch(age){
			case Age.CHILD:
				/cc.height = 1.4f;
				age = Age.TEENAGER;
				DebugPanel.transform.FindChild ("Age").GetComponent<Text> ().text = "Age: Teenager";
				break;
			case Age.TEENAGER:
				cc.height = 1.7f;
				age = Age.ADULT;
				DebugPanel.transform.FindChild ("Age").GetComponent<Text> ().text = "Age: Adult";
				break;
			case Age.ADULT:
				cc.height = 1.1f;
				age = Age.CHILD;
				DebugPanel.transform.FindChild ("Age").GetComponent<Text> ().text = "Age: Child";
				break;
			}*/
			ageDelay = 0;
		}
		//SpotPanel.transform.FindChild(

		// Debug Handler
		DebugPanel.SetActive (Input.GetButton("Debug"));
	}

    // Pauses
    private void gamePaused(bool state)
    {
        if(state)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
	        Time.timeScale = 0.0f;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1.0f;
        }
        PausePanel.SetActive(state);
    }

    public void pauseSwitch()
    {
        if(paused)
        {
            paused = false;
        }
        else
        {
            paused = true;
        }
    }

    public void LoadLevel(string level)
    {
        Application.LoadLevel(level);
    }
}
