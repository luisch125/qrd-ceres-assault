using UnityEngine;
using System.Collections;

public class DisparoSeguimientoScript : MonoBehaviour {

	// Este script controla el disparo basico de los enemigos. Este disparo se dirige siempre a la posicion del jugador en el momento de ser disparado

	public GameObject explosion;			// El objeto explosion que se muestra cuando el disparo choca con el dron del jugador
	
	private GameController gameController;	// Este objeto es el controlador del nivel, y es donde esta la funcion que, en este caso, resta vida cuando el disparo choca con el dron
	private float speed = 6;				// esta variable controla la velocidad del disparo
	private int amount = 10;				// Esta variable se utiliza para indicar la cantidad de daño que se ocasiona al jugador// La velocidad del disparo

	GameObject jugador;						// El objeto al que apunta el disparo, el dron, evidentemente
	Vector3 direction;						// La variable que asignamos para la direccion del disparo

	// Esta funcion genera el disparo en la direccion del dron
	public void Awake () 
	{
		jugador = GameObject.FindGameObjectWithTag ("Player");
		direction = new Vector3 (jugador.GetComponent<Rigidbody>().position.x, jugador.GetComponent<Rigidbody>().position.y, jugador.GetComponent<Rigidbody>().position.z);

		transform.LookAt(direction);
		GetComponent<Rigidbody>().velocity = transform.forward * speed;

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController>();
	}

	// Esta funcion controla lo que ocurre cuando el disparo choca con el jugador, e indica a la funcion que resta salud al dron cuanto daño le ha hecho
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player")
		{
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			Destroy(gameObject);
			gameController.TakeDamage (amount);	
		}
	}
}
