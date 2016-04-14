using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveTree : MonoBehaviour {
	
	public GameObject UIObjectivePrefab;
	public List<GameObject> startingObjectives;
	public GameObject nextObjectiveTree;
	public Dialogue dialogue;
	List<Objective> currentObjectives = new List<Objective>();
	Queue<Objective> remove = new Queue<Objective>();
	GameObject treeUI;
	int objectiveCounter;
	bool finished = false;
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<currentObjectives.Count; i++) {
			if(currentObjectives[i].completed){
				Objective next = currentObjectives[i].next;
				
				treeUI.transform.FindChild("ObjectiveList").FindChild(currentObjectives[i].name)
					.FindChild("Status").FindChild("Check").gameObject.SetActive(true);
				remove.Enqueue(currentObjectives[i]);
				if(next == null){
					//Debug.Log ("End of objective chain");
					continue;
				}
				next.prereqs.Remove (currentObjectives[i]);
				if(next.prereqs.Count == 0 && !currentObjectives.Contains(next)){
					//Debug.Log ("Added another objective");
					AddObjectiveToUI(next.gameObject);
					
					for(int j=0; j<next.sayBefore.Count; j++)
						dialogue.Say(next.sayBefore[j]);
				}
			}
		}
		for (int i=0; i<remove.Count; i++) {
			Objective t = remove.Dequeue ();
			for(int j=0; j<t.sayAfter.Count; j++)
				dialogue.Say(t.sayAfter[j]);
			currentObjectives.Remove (t);
		}
		
		if (currentObjectives.Count == 0)
			finished = true;
	
	}

	public void Begin(GameObject treeUI){
		this.treeUI = treeUI;
		objectiveCounter = 0;
		foreach (GameObject g in startingObjectives)
			AddObjectiveToUI(g);
		gameObject.SetActive (true);
	}

	void AddObjectiveToUI(GameObject objective){
		Vector3 mod = new Vector3 (0, -objectiveCounter * 40, 0);

		GameObject objectiveUI = Instantiate(UIObjectivePrefab);
		string description = objective.GetComponent<Objective>().description;

		objectiveUI.name = objective.name;
		objectiveUI.transform.FindChild ("Description").FindChild ("Text").GetComponent<Text> ().text = description;
		objectiveUI.transform.SetParent(treeUI.transform.FindChild("ObjectiveList"), false);

		// Skip the first one
		if (objectiveCounter != 0) {
			objectiveUI.transform.localPosition = objectiveUI.transform.localPosition + mod;
			RectTransform r = treeUI.GetComponent<RectTransform> ();
			r.offsetMin = new Vector2 (r.offsetMin.x, r.offsetMin.y - 40);
		}
		objectiveCounter++;

		currentObjectives.Add (objective.GetComponent<Objective>());
	}

	public bool IsFinished(){
		return finished;
	}

	public GameObject GetTreeUI(){
		return treeUI;
	}
}
