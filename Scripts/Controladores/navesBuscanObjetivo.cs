using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class navesBuscanObjetivo : MonoBehaviour {

	//Este script se coloca en la camara de la intro del nivel 1 para controlar que siempre mire al dron del jugador.
	//Tambien controla las funciones de audio y si tocamos la pantalla para saltar la intro y cargar el nivel.

	//GameObject drones;						
	public Transform target;				// el objeto al cual va a seguir la camara

	private float speed = 60;
	Vector3 direction;						// La variable que asignamos para la direccion del disparo

	public void Awake () 
	{
		direction = new Vector3 (target.position.x, target.position.y, target.position.z);
		transform.LookAt(direction);
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
		StartCoroutine(Desbandada());
	}

	void Update () 
	{
		// Estas lineas controlan que la camara mire al objeto "target" (en este caso, el dron definido como "player", seleccionado en el inspector de Unity)
		direction = new Vector3 (target.position.x, target.position.y, target.position.z);
	}

	IEnumerator Desbandada()
	{
		yield return new WaitForSeconds(40);
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 0.4f;
	}
}