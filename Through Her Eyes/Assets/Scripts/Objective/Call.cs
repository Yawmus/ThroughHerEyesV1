using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Call : Objective {

	public Phone phone;
	public string number;

	void Awake() {
	}
	
	// Update is called once per frame
	void Update () {
		completed = phone.Called (number);
	}
}
