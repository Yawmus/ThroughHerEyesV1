using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class HomeInvasionExt : MonoBehaviour {

	public GameObject doorlock;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		//camera.GetComponent<Blur> ().enabled = true;
		Camera.main.GetComponent<MotionBlur> ().enabled = true;
		doorlock.GetComponent<Interactable> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
