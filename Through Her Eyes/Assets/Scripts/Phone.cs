using UnityEngine;
using System.Collections;

public class Phone : MonoBehaviour {

	const int SIZE = 12;
	public string[] valid;
	bool[] called;
	string num;
	float[] delays;
	GameObject[] numpad;

	// Use this for initialization
	void Awake () {
		numpad = new GameObject[SIZE];
		delays = new float[SIZE];
		called = new bool[valid.Length];
		for (int i=0; i<called.Length; i++)
			called[i] = false;
		for(int i=0; i<SIZE-2; i++)
			numpad[i] = transform.FindChild(i + "").gameObject;
		numpad [SIZE - 2] = transform.FindChild ("clear").gameObject;
		numpad [SIZE - 1] = transform.FindChild ("call").gameObject;

		for (int i=0; i<SIZE; i++)
			delays[i] = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<SIZE - 1; i++) {
			if(numpad[i].GetComponent<Interactable>().IsInteracted() && delays[i] >= .75f)
			{
				if(i == 10)
					num = "";
				else
					num += i;
				delays[i] = 0f;
			}
			delays[i] += Time.deltaTime;
		}

		if(numpad[11].GetComponent<Interactable>().IsInteracted())
		{
			for(int i=0; i<valid.Length; i++)
			{
				if(valid[i] == num){
					called[i] = true;
				}
			}
			num = "";
		}
	}
	public bool Called(string num)
	{
		for (int i=0; i<valid.Length; i++) {
			if (valid [i] == num)
				return called [i];
		}
		return false;
	}
}
