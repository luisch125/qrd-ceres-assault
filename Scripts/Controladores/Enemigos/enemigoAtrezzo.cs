using UnityEngine;
using System.Collections;

public class enemigoAtrezzo : MonoBehaviour {

	// Este script controla el comportamiento del cuarto y ultimo enemigo del tutorial.

	private GameController gameController;		// Este objeto es el controlador del nivel

	// Esta funcion inicia el movimiento del enemigo y se asegura que existe el controlador del nivel
	void Start () 
	{
		GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(-40, -80));

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) 
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null) 
		{
			Debug.Log ("No puedo encontrar el script 'GameController'");
		}
	}

	// Esta funcion controla lo que ocurre cuando colisiona con el jugador, con el disparo del jugador y con los bordes imaginarios del juego,
	// que usamos para destruir los enemigos que se escapan sin ser destruidos por el jugador.
	// Tambien lo destruye cuando su vida llega a cero. Y por ultimo, ignora la colision con los disparos de otros enemigos
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Boundary") 
		{
			return;
		}
	}
}