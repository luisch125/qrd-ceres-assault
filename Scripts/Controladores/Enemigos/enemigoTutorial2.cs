using UnityEngine;
using System.Collections;

public class enemigoTutorial2 : MonoBehaviour {

	// Este script controla el comportamiento del segundo enemigo del tutorial.

	public GameObject explosion;				// Este objeto es la explosion del enemigo
	public GameObject playerExplosion;			// Este objeto es la explosion del... jugador
	public int vidaEnemigo = 10;				// la vida del enemigo
	
	private GameController gameController;		// Este objeto es el controlador del nivel
	private int scoreValue = 100;				// Cuantos puntos nos da eliminar a este enemigo
	private bool rebote = false;				// Esta variable controla el cambio de direccion del enemigo
	private float retroceder = 0;				// Esta variable controla cuantas veces el enemigo cambia de direccion antes de irse de la pantalla
	
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

	// Esta funcion controla el movimiento del enemigo (cambio de direccion y cuantas veces la cambia hasta que se va de la pantalla)
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
	}

	// Esta funcion controla lo que ocurre cuando colisiona con el jugador, con el disparo del jugador y con los bordes imaginarios del juego,
	// que usamos para destruir los enemigos que se escapan sin ser destruidos por el jugador.
	// Tambien lo destruye cuando su vida llega a cero.
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Boundary") 
		{
			return;
		}

		Instantiate(explosion, transform.position, transform.rotation); // as GameObject; Siempre que choca con algo, excepto el borde, genera la explosion
		
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
		
		if (vidaEnemigo <= 0) 
		{
			WaveControllerTutorial.contadorEnemigos--;
			Destroy(gameObject);
			gameController.AddScore (scoreValue);
		}		
	}
}	