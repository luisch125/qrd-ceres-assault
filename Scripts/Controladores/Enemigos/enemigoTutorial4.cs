using UnityEngine;
using System.Collections;

public class enemigoTutorial4 : MonoBehaviour {

	// Este script controla el comportamiento del cuarto y ultimo enemigo del tutorial.

	public GameObject explosion;				// Este objeto es la explosion del enemigo
	public GameObject playerExplosion;			// Este objeto es la explosion del... jugador
	public GameObject bala;						// Este objeto es el disparo enemigo

	public int vidaEnemigo = 8;					// la vida del enemigo

	private GameController gameController;		// Este objeto es el controlador del nivel
	private int scoreValue = 100;				// Cuantos puntos nos da eliminar a este enemigo
	float fireTimer = 2f;						// Esta variable es el contador que separa un disparo del siguiente, en segundos

	// Esta funcion inicia el movimiento del enemigo y se asegura que existe el controlador del nivel
	void Start () 
	{
		GetComponent<Rigidbody>().AddForce(transform.forward * 420);

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

	// Este enemigo tiene un movimiento sin cambios, por eso en esta ocasion la funcion solo controla cuando dispara
	void FixedUpdate ()
	{
		fireTimer -= 1* Time.deltaTime;
		if (fireTimer <= 0.0f) 
		{
			Atacando ();
		}
	}

	//Esta es la funcion que activa el disparo enemigo y resetea el fireTimer
	void Atacando ()
	{
		fireTimer = 3;
		Instantiate(bala, transform.position, transform.rotation);
		GetComponent<AudioSource>().Play ();
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
		Instantiate(explosion, transform.position, transform.rotation); // as GameObject;
		
		if (other.tag == "Player") 
		{
			gameController.TakeDamage (40);	
			gameController.AddScore (scoreValue);
			vidaEnemigo = vidaEnemigo -8;
		}
		
		if (other.tag == "DisparoJugador")
		{
			DestroyObject(other.gameObject);
			vidaEnemigo--;
		}

		// En este if hay que añadir todos los disparos enemigos para que no colisionen con otros enemigos
		if (other.tag == "Disparo10")
		{
			return;
		}
		
		if (vidaEnemigo <= 0) 
		{
			WaveControllerTutorial.contadorEnemigos--;
			Destroy(gameObject);
			gameController.AddScore (scoreValue);
		}		
	}
}