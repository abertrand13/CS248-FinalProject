using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controller : MonoBehaviour {
	// constants
	const int NUM_FISH = 200;
	const float NEIGHBOR_DIST = 3.0f;
	
	// vars we set in editor
	public Transform fish; // why is it a Transform? I have no idea.
	public Transform targetPointPrefab;

	// private stuff
	private GameObject[] school;
	private List<GameObject> obstacles;
	private GameObject targetPoint;
	private Vector3 motion;
	private float time;

	// Use this for initialization
	void Start () {
		Transform tmp = Instantiate(targetPointPrefab, new Vector3(0.0f, 20.0f, 0.0f), Quaternion.identity) as Transform;
		targetPoint = tmp.gameObject;

		obstacles = new List<GameObject>();	
		
		school = new GameObject[NUM_FISH];
		for(int i = 0;i < NUM_FISH; i++) {
			Transform t = Instantiate(fish, new Vector3(Random.Range(-5.0f,5.0f),Random.Range(-5.0f,5.0f),Random.Range(-5.0f,5.0f)), Quaternion.identity) as Transform;
			school[i] = t.gameObject;
		}

		time = 0;
		motion = new Vector3(0.0f, 0.0f, 0.0f);

		obstacles.Add(GameObject.Find("obstacle1"));
		obstacles.Add(GameObject.Find("floor"));
		obstacles.Add(GameObject.Find("wallN"));
		obstacles.Add(GameObject.Find("wallE"));
		obstacles.Add(GameObject.Find("wallW"));
		obstacles.Add(GameObject.Find("wallS"));
		obstacles.Add(GameObject.Find("GreatWhite"));
	}
	
	// Update is called once per frame
	void Update () {
		time += .035f;
		// time += .02f;

			
		motion.x = 10*Mathf.Cos(time);
		motion.z = 10*Mathf.Sin(time);
		motion.y = 10;
		// motion.y = 20 + -10*Mathf.Sin(time/10f);
		targetPoint.transform.position = motion;
		
		// do {
		// 	motion = (motion.normalized + Random.insideUnitSphere) * .3f;
		// } while(((targetPoint.transform.position + motion) - new Vector3(0.0f, 20.0f, 0.0f)).magnitude > 17.0f);
		// targetPoint.transform.Translate(motion);
		
		for(int i = 0; i < NUM_FISH; i++) {
			GameObject thisFish = school[i];
			List<GameObject> neighbors = new List<GameObject>();
			for(int j = 0; j < NUM_FISH; j++) {
				GameObject thatFish = school[j];
				if(Vector3.Distance(thisFish.transform.position, thatFish.transform.position) < NEIGHBOR_DIST && i != j) {
					neighbors.Add(thatFish);
				}
			}
			school[i].GetComponent<FishBehavior>().ReactToNeighbors(neighbors, obstacles, targetPoint.transform.position);
			neighbors = null;
		}
	}
}
