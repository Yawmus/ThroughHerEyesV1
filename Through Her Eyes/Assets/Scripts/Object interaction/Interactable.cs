using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {

	public int currentState;
	public List<Transition> transitions;
	public Dialogue dialogue;
	Transition currentTransition, hoveredTransition;
	bool interacted;
	bool skip = true, started = false;
	int mod = 1;
	float timer = .20f;
	Color endColor;
	
	// Use this for initialization
	void Start () 
	{
		foreach (Transition t in transitions)
			t.Start ();
	}

	void Awake()
	{
		endColor = new Color (0f, 0f, 1f);
	}

	void Update(){
		if (!skip && hoveredTransition != null) {
			hoveredTransition.ResetHighlightedMeshes ();
			hoveredTransition = null;
			timer = .20f;
			mod = 1;
		} else
			skip = false;
		
		if (interacted && !started) {
			currentTransition = transitions [currentState];
			currentState = currentTransition.endingState;
			currentTransition.ResetHighlightedMeshes();
			currentTransition.Awake ();
			started = true;
			if(GetComponent<AudioSource>() != null && currentTransition.audio != null)
			{
				GetComponent<AudioSource>().clip = currentTransition.audio;
				GetComponent<AudioSource>().Play();
			}
		} else if (interacted) {
			if (currentTransition.IsFinished ()) {
				currentTransition.EndTransition (dialogue);
				interacted = false;
				started = false;
			} else {
				currentTransition.DoTransition ();

			}
		} else {

		}
	}

	public void Hover () {
		timer += Time.deltaTime * mod;
		if (timer >= .85f || timer <= .15f)
			mod *= -1;

		hoveredTransition = transitions [currentState];
		hoveredTransition.UpdateHighlightedMeshes (endColor, timer);
		skip = true;
	}

	public bool CanInteract()
	{
		if (interacted)
			return false;
		if (currentState >= transitions.Count)
			return false;
		foreach (PrereqState p in transitions[currentState].prereqs)
			if (p.gameObject.GetComponent<Interactable> ().currentState != p.state)
				return false;

		return true;
	}

	public bool IsInteracted(){
		return interacted;
	}
	
	public void SetInteracted(bool i)
	{
		interacted = i;
	}

	public string GetDiscription()
	{
		return transitions[currentState].description;
	}

	[System.Serializable]
	public class Transition{
		public AudioClip audio;
		public string description;
		public int endingState;
		public List<PrereqState> prereqs;
		public List<ResultState> results;
		public List<MeshRenderer> highlightedMeshes;
		public List<Movement> movements;
		public List<string> say;

		List<Color> startColors = new List<Color>();
		bool finished = false;

		public void Start(){
			for(int j=0; j<highlightedMeshes.Count; j++)
				for (int i=0; i<highlightedMeshes[j].materials.Length; i++) {
					startColors.Add (highlightedMeshes[j].materials[i].color);
				}
		}

		public void Awake(){
			foreach (Movement m in movements)
				m.Awake ();
		}
		
		public void ResetHighlightedMeshes(){
			int z = 0;
			for(int j=0; j<highlightedMeshes.Count; j++)
				for(int i=0; i<highlightedMeshes[j].materials.Length; i++)
					highlightedMeshes[j].materials[i].color = startColors[z++];
		}
		
		public void UpdateHighlightedMeshes(Color endColor, float timer){
			int z = 0;
			for(int j=0; j<highlightedMeshes.Count; j++)
				for(int i=0; i<highlightedMeshes[j].materials.Length; i++)
					highlightedMeshes[j].materials[i].color = Color.Lerp (startColors[z++], endColor, timer);
		}

		public void DoTransition(){
			bool temp = true;
			foreach (Movement m in movements) {
				if(!m.IsFinished()){
					temp = false;
					m.Move ();
				}
			}
			finished = temp;
		}

		public bool IsFinished(){
			return finished;
		}

		public void EndTransition(Dialogue dialogue){
			finished = false;
			foreach (ResultState r in results)
				r.gameObject.GetComponent<Interactable> ().currentState = r.state;
			foreach (Movement m in movements)
				m.EndMovement ();
			
			for(int j=0; j<say.Count; j++)
				dialogue.Say(say[j]);
		}
	}
	
	[System.Serializable]
	public class Movement{
		public GameObject gameObject;
		public float speed = 2f;
		public Vector3 endingPositionOffset;
		public Vector3 endingRotationOffset;
		public bool recur = false;
		Transform transform;
		Vector3 startingPos, endingPos, startingRot, endingRot;
		float moveTimer = 0f;
		bool finished = false;
		int mod = 1;
		
		public void Awake(){
			transform = gameObject.transform;
			
			startingPos = transform.localPosition;
			startingRot = transform.localEulerAngles;
			endingPos = startingPos + endingPositionOffset;
			endingRot =  startingRot + endingRotationOffset;
		}
		
		public void Move(){
			moveTimer += Time.deltaTime * mod;
			transform.localEulerAngles = Vector3.Lerp (startingRot, endingRot, moveTimer / speed);
			transform.localPosition = Vector3.Lerp (startingPos, endingPos, moveTimer / speed);
			
			if (recur) {
				if(moveTimer > speed){
					moveTimer = speed;
					mod = -1;
				}
				if(moveTimer < 0)
				{
					moveTimer = 0;
					mod = 1;
					finished = true;
				}
			} else {
				if (moveTimer > speed) {
					moveTimer = 0f;
					finished = true;
				}
			}
		}
		
		public void EndMovement(){
			if (recur) {
				gameObject.transform.localEulerAngles = startingRot;
				gameObject.transform.localPosition = startingPos;
			}
			else {
				gameObject.transform.localEulerAngles = endingRot;
				gameObject.transform.localPosition = endingPos;
			}
			finished = false;
		}
		
		public bool IsFinished(){
			return finished;
		}
	}

	[RequireComponent (typeof(Interactable))]
	[System.Serializable]
	public class PrereqState{
		public GameObject gameObject;
		public int state;
	}
	
	[RequireComponent (typeof(Interactable))]
	[System.Serializable]
	public class ResultState{
		public GameObject gameObject;
		public int state;
	}
}
