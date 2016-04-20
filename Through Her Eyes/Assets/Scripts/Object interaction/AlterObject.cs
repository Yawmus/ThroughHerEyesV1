using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Interactable))]
public class AlterObject : MonoBehaviour {

	public List<ChangeActive> setActive;
	public List<ChangeColor> colors;
	Interactable i;

	// Use this for initialization
	void Awake () {
		i = GetComponent<Interactable> ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (ChangeActive a in setActive) {
			if(a.reqState == i.currentState){
				if(a.onInteraction)
					if(!i.IsInteracted())
						continue;
				a.target.SetActive(a.active);
			}
		}
		foreach (ChangeColor c in colors) {
			if(c.reqState == i.currentState){
				if(c.onInteraction)
					if(!i.IsInteracted())
						continue;
				Zone z = c.target.GetComponent<Zone>();
				if(z == null)
					c.target.GetComponent<Grabable>().SetColor(c.color);
				else{
					foreach(GameObject g in z.GetContainedObjects()){
						//Color t = g.GetComponent<Renderer>().material.color;
						g.GetComponent<Grabable>().SetColor(c.color);
					}
				}
			}
		}

	}
	
	[System.Serializable]
	public class ChangeActive{
		public int reqState;
		public bool onInteraction;
		public GameObject target;
		public bool active;
	}
	
	[System.Serializable]
	public class ChangeColor{
		public int reqState;
		public bool onInteraction;
		public GameObject target;
		public Color color;
	}
}
