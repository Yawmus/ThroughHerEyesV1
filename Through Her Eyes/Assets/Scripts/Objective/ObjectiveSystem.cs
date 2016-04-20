using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectiveSystem : MonoBehaviour{

	public GameObject UITreePrefab;
	public GameObject ObjectivePanel;
	public GameObject UIFinishedTreePrefab;
	public GameObject fade;
	public List<GameObject> startingObjectiveTrees;
	public bool lastLevel = false;
	List<ObjectiveTree> currentObjectiveTrees = new List<ObjectiveTree>();
	List<GameObject> finishedObjectiveTrees = new List<GameObject>();
	Queue<ObjectiveTree> remove = new Queue<ObjectiveTree>();

	// Use this for initialization
	void Awake () {
		for (int i=0; i<startingObjectiveTrees.Count; i++) {
			AddTreeToUI (startingObjectiveTrees[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<currentObjectiveTrees.Count; i++) {
			if(currentObjectiveTrees[i].IsFinished())
			{
				GameObject next = currentObjectiveTrees[i].nextObjectiveTree;

				remove.Enqueue(currentObjectiveTrees[i]);
				if(next == null){
					Debug.Log ("End of level");
					fade.GetComponent<Fade>().FadeOut();
					if(lastLevel)
					{
						transform.FindChild ("Siren").gameObject.SetActive(true);
					}
					continue;
				}
				AddTreeToUI(next);
			}
		}

		for (int i=0; i<remove.Count; i++) {
			GameObject finishedUI = Instantiate(UIFinishedTreePrefab);
			finishedUI.transform.FindChild("Title").GetComponent<Text>().text = remove.Peek ().name;
			finishedUI.transform.SetParent(ObjectivePanel.transform, false);
			finishedUI.name = "Finished " + remove.Peek ().name;
			finishedObjectiveTrees.Add (finishedUI);

			GameObject.Destroy(remove.Peek ().GetTreeUI());
			currentObjectiveTrees.Remove (remove.Dequeue ());
		}

		if (currentObjectiveTrees.Count == 0) {
			ObjectivePanel.transform.FindChild ("Complete").gameObject.SetActive (true);
		}
		
		SpaceTrees ();
	}

	public void SpaceTrees(){
		// Keeps the objective trees spaced
		//float totalBot = 0, totalTop = 0;
		float baseY = 0;
		foreach (GameObject finishedTreeUI in finishedObjectiveTrees) {
			RectTransform r = finishedTreeUI.GetComponent<RectTransform> ();
			
			r.anchoredPosition = new Vector2 (r.anchoredPosition.x, baseY - r.sizeDelta.y/2);
			baseY -= r.sizeDelta.y;
		}
		foreach (ObjectiveTree tree in currentObjectiveTrees) {
			RectTransform r = tree.GetTreeUI().GetComponent<RectTransform> ();

			r.anchoredPosition = new Vector2 (r.anchoredPosition.x, baseY - r.sizeDelta.y/2);
			baseY -= r.sizeDelta.y;
		}
	}

	void AddTreeToUI(GameObject tree)
	{
		GameObject treeUI = Instantiate(UITreePrefab);
		//Vector3 mod = new Vector3 (0, -counter * 40, 0);

		treeUI.name = tree.name;
		treeUI.transform.FindChild("Title").GetComponent<Text>().text = tree.name;
		treeUI.transform.SetParent(ObjectivePanel.transform, false);

		currentObjectiveTrees.Add (tree.GetComponent<ObjectiveTree>());
		tree.GetComponent<ObjectiveTree> ().Begin(treeUI);
	}
}
