using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Objective : MonoBehaviour {

	// Objective base info
	public Objective next;
	public List<string> sayBefore, sayAfter;
	public List<Objective> prereqs = new List<Objective> ();
	public bool completed = false;
	protected bool active = false;

	// Objective specific info
	public string description;

	// Use this for initialization
	void Start () {
	
	}

}
