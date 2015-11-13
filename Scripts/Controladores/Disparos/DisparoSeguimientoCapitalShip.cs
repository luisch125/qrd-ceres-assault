using UnityEngine;
using System.Collections;

public class DisparoSeguimientoCapitalShip : MonoBehaviour {

	// Este script controla el disparo basico de la nave capital.

	private float speed = 6;				// esta variable controla la velocidad del disparo

	GameObject target;						// El objeto al que apunta el disparo
	Vector3 direction;						// La variable que asignamos para la direccion del disparo

	// Esta funcion genera el disparo en la direccion del dron
	public void Awake () 
	{
        target = GameObject.FindGameObjectWithTag ("TargetDummy1");
		direction = new Vector3 (target.GetComponent<Rigidbody>().position.x, target.GetComponent<Rigidbody>().position.y, target.GetComponent<Rigidbody>().position.z);

		transform.LookAt(direction);
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}

	// Esta funcion controla lo que ocurre cuando el disparo choca con el jugador, e indica a la funcion que resta salud al dron cuanto daño le ha hecho
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "TargetDummy1")
		{
			Destroy(gameObject);
		}

	}
}
