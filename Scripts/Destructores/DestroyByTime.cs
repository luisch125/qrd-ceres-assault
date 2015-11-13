using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

	// Este script se puede utilizar para destruir ciertos objetos despues de un tiempo determinado

	public float lifetime;

	void Start () 
	{
		Destroy (gameObject, lifetime);
	}

}
