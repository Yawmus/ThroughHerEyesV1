using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Fade : MonoBehaviour {

	public int mission;
	public float outDur = 2f, inDur = 3f;
	public List<RectTransform> UIElements;
	public RectTransform title;

	Image i;
	float timer, dur;
	Color start, end;
	bool fade, fIn;

	// Use this for initialization
	void Awake () {
		i = GetComponent<Image>();
		FadeIn ();
	}
	
	// Update is called once per frame
	void Update () {
		if (fade) {
			timer += Time.deltaTime;

			i.color = Color.Lerp(start, end, timer/dur);

			if(timer >= dur)
			{
				fade = false;
				if(fIn)
				{
					for (int j=0; j<UIElements.Count; j++)
						UIElements [j].gameObject.SetActive (true);
					title.gameObject.SetActive(false);
				}
				else
				{
					switch(mission)
					{
					case 1:
						Application.LoadLevel("Mission 2");
						break;
					case 2:
						Application.LoadLevel ("Mission 3");
						break;
					case 3:
						Application.LoadLevel("MainMenu");
						break;
					}
				}
			}
		}
	}

	public void FadeIn()
	{
		fIn = true;
		for (int i=0; i<UIElements.Count; i++)
			UIElements [i].gameObject.SetActive (false);
		start = new Color(0, 0, 0, 1);
		end = new Color(0, 0, 0, 0);
		fade = true;
		timer = 0f;
		dur = inDur;
	}

	public void FadeOut()
	{
		timer = 0f;
		fIn = false;
		start = new Color(0, 0, 0, 0);
		end = new Color(0, 0, 0, 1);
		fade = true;
		dur = outDur;
		for (int j=0; j<UIElements.Count; j++)
			UIElements [j].gameObject.SetActive (false);
	}
}
