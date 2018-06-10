using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishBehavior : MonoBehaviour {

	const float MAX_SPEED = 0.5f;
	const float MAX_ACCEL = MAX_SPEED * .05f;
	
	private Vector3 v;

	
	// Use this for initialization
	void Start () {
		// initialize a random velocity
		v = new Vector3(Random.Range(-0.1f, 0.1f),
		 				Random.Range(-0.1f, 0.1f),
		 				Random.Range(-0.1f, 0.1f));
		// v = new Vector3(.1f, .1f, .1f);
		// v = new Vector3(0f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		// if we only store velocity then we can set heading based on velocity every frame
		transform.rotation = Quaternion.LookRotation(v);
		transform.Translate(0f, 0f, v.magnitude);

	}

	public void ReactToNeighbors(List<GameObject> neighbors, List<GameObject> obstacles, Vector3 targetPoint) {
		
		Vector3 avgPosition = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 avgVelocity = new Vector3(0.0f, 0.0f, 0.0f);
		
		Vector3 repulsionForce = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 avoidanceForce = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 attractionForce = new Vector3(0.0f, 0.0f, 0.0f);
		
		// Vector3 avgOrientation = new Vector3();
		// Vector3 avoidingForce = new Vector3();
		foreach(GameObject fish in neighbors) {
			Vector3 fishPos = fish.GetComponent<FishBehavior>().GetPosition();

			avgPosition += fishPos;
			avgVelocity += fish.GetComponent<FishBehavior>().GetVelocity();

			// check if we're about to collide with this fish
			// TODO define all these constants
			Vector3 posVector = fishPos - transform.position;
			// if(Mathf.Abs(Vector3.SignedAngle(posVector, v, Vector3.up)) < 20.0f) {
			// if(Vector3.Project(v, posVector).magnitude > (posVector.magnitude * .8f)) {
				// generate acceleration in the opposite direction
				repulsionForce -= posVector * .05f;
			// }


			// avgOrientation += fish.GetComponent<FishBehavior>().GetOrientation();
			// avoidingForce += (transform.position - fish.GetComponent<FishBehavior>().GetPosition());
		}

		// Profiler.BeginSample("ObstacleChecking");
		foreach(GameObject obstacle in obstacles) {
			if(v.magnitude == 0) continue;	
			// avoid super hard
			Ray ray = new Ray(transform.position, v.normalized);
			RaycastHit hit;
			if(obstacle.GetComponent<Collider>().Raycast(ray, out hit, 3.0f)) {
				// print(obstacle.name);
				if(obstacle.name == "GreatWhite") {
					// shark is less of a wall and more of a 'thing'
					avoidanceForce +=  (transform.position - obstacle.transform.position) * .05f;
				} else {
					Vector3 projection = Vector3.Project(hit.normal, -v);
					avoidanceForce += (hit.normal - projection) * .2f;
				}

				// check if this is going to be a head on collision
				// if((v - hit.normal).magnitude < .2f) {
				// 	print("happening");
				// 	// hard right! or left. One of the two
				// 	avoidanceForce += Vector3.Cross(v, Vector3.up) * 5.0f;
				// } else {
					// avoidanceForce += hit.normal * 5.0f;
					// avoidanceForce += (transform.position - obstacle.transform.position) * .01f;
					// print(avoidanceForce);
				// }
					
			}
			// if(avoidanceForce.magnitude != 0f) print("okay " + avoidanceForce);
		}
		// Profiler.EndSample();

		// avgOrientation /= neighbors.Count;
		if(neighbors.Count > 0) {
			avgPosition /= neighbors.Count;
			avgVelocity /= neighbors.Count;
			attractionForce = (avgPosition - transform.position) * .1f;
		}

		// generate acceleration force based on average velocity of neighbors
		Vector3 velocityAccel = (avgVelocity - v) * .05f;
		// generate force based on staying away from neighbors
		repulsionForce *= .5f;
		// generate force to get every boid pointed towards target (subtley)	
		Vector3 targetForce = (targetPoint - transform.position).normalized * .1f;
		
		Vector3 accel;	
		if(avoidanceForce.magnitude != 0.0f) {
			// accel = avoidanceForce;
			v += Vector3.ClampMagnitude(avoidanceForce, MAX_ACCEL * 3f);
		} else {
			accel = velocityAccel;
			accel += repulsionForce; 
			accel += attractionForce;
			accel += targetForce;
			accel = Vector3.ClampMagnitude(accel, MAX_ACCEL);
			v += accel;
		}


		// need to find a way to reduce jitter	
		// separate turn acceleration and speed acceleration

		// print(v);
		// print("after accel " + v);

		v = Vector3.ClampMagnitude(v, MAX_SPEED);
		// print("after clamp " + v);
	}

	public Vector3 GetVelocity () {
		return v;	
	}

	public Quaternion GetOrientation () {
		return transform.rotation;
	}

	public Vector3 GetPosition () {
		return transform.position;
	}
}
