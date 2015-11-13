using UnityEngine;
using System.Collections;

public class enemigoTutorial3 : MonoBehaviour {

	// Este script controla el comportamiento del tercer enemigo del tutorial.

	public GameObject explosion;				// Este objeto es la explosion del enemigo
	public GameObject playerExplosion;			// Este objeto es la explosion del... jugador
	public GameObject bala;						// Este objeto es el disparo enemigo

	public int vidaEnemigo = 10;				// la vida del enemigo

	private GameController gameController;		// Este objeto es el controlador del nivel
	private int scoreValue = 100;				// Cuantos puntos nos da eliminar a este enemigo
	private bool rebote = false;				// Esta variable controla el cambio de direccion del enemigo
	private float retroceder = 0;				// Esta variable controla cuantas veces el enemigo cambia de direccion antes de irse de la pantalla
	float fireTimer = 2f;						// Esta variable es el contador que separa un disparo del siguiente, en segundos

	// Esta funcion inicia el movimiento del enemigo y se asegura que existe el controlador del nivel
	void Start () 
	{
		GetComponent<Rigidbody>().AddForce(transform.forward * 360);

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

	// Esta funcion controla el movimiento del enemigo (cambio de direccion y cuantas veces la cambia hasta que se va de la pantalla) y el disparo
	void FixedUpdate ()
	{
			if(gameObject.transform.position.z < 10)
			{
				GetComponent<Rigidbody>().AddForce(transform.forward * -200 * Time.fixedDeltaTime);
				rebote = true;
				retroceder += 1 * Time.deltaTime;
			}

			if(gameObject.transform.position.z > 10 && rebote == true && retroceder <= 6)
			{
				GetComponent<Rigidbody>().AddForce(transform.forward * 200 * Time.fixedDeltaTime);
			}

		fireTimer -= 1* Time.deltaTime;
		if (fireTimer <= 0.0f) 
		{
			Atacando ();	// Aqui activamos la funcion del disparo cada vez que el fireTimer llega a 0
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
			vidaEnemigo = vidaEnemigo -10;
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