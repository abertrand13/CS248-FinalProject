using UnityEngine;
using System.Collections;

public class SharkBehavior : MonoBehaviour {

	const float speed = .2f;
	const float rot_speed = 3.0f;
	Vector3 rotate_amt = new Vector3(rot_speed, rot_speed, rot_speed);
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("up")) {
			transform.Rotate(Vector3.Scale(rotate_amt, Vector3.left));
		} else if(Input.GetKey("down")) {
			transform.Rotate(Vector3.Scale(rotate_amt, Vector3.right));
		}
		if(Input.GetKey("left")) {
			transform.Rotate(Vector3.Scale(rotate_amt, Vector3.down));
		} else if(Input.GetKey("right")) {
			transform.Rotate(Vector3.Scale(rotate_amt, Vector3.up));
		}

		if(Input.GetKey("space")) {
			transform.Translate(new Vector3(0.0f, 0.0f, speed));
		}
	}
}
