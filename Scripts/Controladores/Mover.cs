using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour 
{
	// Este script controla la velocidad del disparo del dron del jugador y de los asteroides. Se puede reutilizar para cualquiero objeto que se mueva en linea recta
	// por la pantalla, para controlar su velocidad
	public float speed;

	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
	
}
