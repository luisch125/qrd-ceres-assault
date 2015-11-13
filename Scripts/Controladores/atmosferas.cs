using UnityEngine;
using System.Collections;

public class atmosferas : MonoBehaviour {

	public Transform target;				// el objeto al cual va a seguir la camara

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(target);
	}
}
