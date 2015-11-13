using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour {

	// Este script se puede usar para crear rotacion aleatoria en los objetos, como los asteroides, o restos de naves, por ejemplo.
	public float tumble;

	void Start () 
	{
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
	}

}
