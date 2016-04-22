using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Zone : MonoBehaviour {

	List<GameObject> objects = new List<GameObject>();
	Queue<GameObject> removeObjects = new Queue<GameObject>();
	BoxCollider b;

	// Use this for initialization
	void Start () {
		b = GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {

		foreach (GameObject g in objects) {
			if (!b.bounds.Intersects (g.GetComponent<Collider> ().bounds)) {
				removeObjects.Enqueue (g);
			}
		}
		while (removeObjects.Count > 0)
			objects.Remove(removeObjects.Dequeue ());
	
	}
	void OnTriggerEnter(Collider other){
		Grabable g = other.gameObject.GetComponent<Grabable> ();
		if (g == null)
			return;
		objects.Add (g.gameObject);
	}

	public List<GameObject> GetContainedObjects(){
		return objects;
	}
}
