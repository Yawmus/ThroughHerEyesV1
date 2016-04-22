using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	[Range(1, 3)]
	public int moveSpeed = 2;
	CharacterController cc;
	RaycastHit hit;
	public GameObject itemUI;

	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;
	float rotationY = 0F;

	GameObject grabbedObject;
	
	static float oldX;

	// Use this for initialization
	void Awake () {
        cc = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
		//Debug.Log (mX + ", " + mY);
		//transform.Rotate ((transform.up * mX + -transform.right * mY) * 2f);
		//transform.Rotate (transform.right * mY * 2f);
		cc.SimpleMove((transform.forward * v + transform.right * h) * moveSpeed * 1.5f);

		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
		
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		itemUI.SetActive(false);


		// Hovers and grabs objects
		if (grabbedObject == null) {
			Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
			// The raycase distance is the same as the character hight!!!!!!!!!!!!!!!!!!!!! just fyi
			if (Physics.Raycast (ray, out hit, 1.5f)) {
				GameObject go = hit.collider.gameObject;
				if (go.GetComponent<Grabable> () != null) {
					go.GetComponent<Grabable> ().Hover ();
					string t = go.GetComponent<Grabable> ().GetDiscription();
					itemUI.transform.FindChild("Text").GetComponent<Text>().text = t;
					itemUI.SetActive(true);
					if (Input.GetMouseButtonDown (0)) {
						grabbedObject = go;

						Grabable g = grabbedObject.GetComponent<Grabable>();
						foreach(Collider c in grabbedObject.GetComponents<Collider>())
							c.enabled = false;
						g.rigidbody.useGravity = false;
						g.rigidbody.Sleep ();
					}
				}
				// For interact objectives
				else if(go.GetComponent<Interactable>() != null && go.GetComponent<Interactable>().CanInteract())
				{
					go.GetComponent<Interactable> ().Hover ();
					string t = go.GetComponent<Interactable> ().GetDiscription();
					
					if(t != null && t != "")
					{
						itemUI.transform.FindChild("Text").GetComponent<Text>().text = t;
						itemUI.SetActive(true);
					}

					if(Input.GetMouseButtonDown(0)){
						go.GetComponent<Interactable>().SetInteracted(true);
					}
				}
			}
		} else {
			float x = -(h * 20);
			if(x == 0 && oldX != 0)
				x = oldX / 2;
			grabbedObject.transform.position = Camera.main.transform.position + (Camera.main.transform.forward * .6f);
			grabbedObject.transform.rotation = Quaternion.Euler(x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);

			if (Input.GetMouseButtonDown (0)) {
				Vector3 pos = Camera.main.transform.position + (Camera.main.transform.forward * .35f);
				Grabable g = grabbedObject.GetComponent<Grabable>();
				grabbedObject.transform.position = pos;
				g.rigidbody.WakeUp();
				g.rigidbody.useGravity = true;
				g.rigidbody.isKinematic = false;
				g.rigidbody.velocity = cc.velocity;
				foreach(Collider c in grabbedObject.GetComponents<Collider>())
					c.enabled = true;
				grabbedObject = null;
			}
			oldX = x;
		}
		Camera.main.transform.localPosition = new Vector3(0, cc.height/4, -.2f);
		//cc.height;
	}

	public GameObject HeldObject(){
		return grabbedObject;
	}

	void FixedUpdate()
	{
		//Debug.DrawLine(transform.position, Camera.main.transform.forward, Color.blue, true);
	}
}
