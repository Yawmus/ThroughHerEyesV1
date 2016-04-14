using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour {

	Queue<string> buffer = new Queue<string>();
	Text t;
	GameObject bg;
	float timer = 0;

	// Use this for initialization
	void Awake () {
		bg = transform.FindChild ("Background").gameObject;
		t = bg.transform.FindChild ("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= 2.7f)
		{
			if(buffer.Count != 0)
			{
				bg.SetActive(true);
				t.text = buffer.Dequeue ();
				timer = 0;
			}
			else
			{
				bg.SetActive(false);
			}
		}
	}

	public void Say(string message)
	{
		buffer.Enqueue (message);
	}
}
