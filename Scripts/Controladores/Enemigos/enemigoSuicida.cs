using UnityEngine;
using System.Collections;

public class enemigoSuicida : MonoBehaviour {

	public GameObject explosion;                // Este es el objeto de la explosion del enemigo
	public GameObject playerExplosion;          // Este es el objeto de la explosion del jugador
	public int vidaEnemigo = 1;                // Los puntos de vida del enemigo

	private GameController gameController;		// aqui llamamos al script que controla el nivel
	private int scoreValue = 10;               // Los puntos que nos da este enemigo al ser destruido
	private int amount = 8;                    // La cantidad de daño que hacen los disparos de este enemigo
	private int velocidad = 8;
	private GameObject jugador;
	private Vector3 direction;


	void Awake ()
	{
		jugador = GameObject.FindGameObjectWithTag ("Player");
		direction = new Vector3 (jugador.GetComponent<Rigidbody> ().position.x, jugador.GetComponent<Rigidbody> ().position.y, jugador.GetComponent<Rigidbody> ().position.z);

		transform.LookAt (direction);

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController>();
	}

	void Start () 
	{
		GetComponent<Rigidbody> ().AddForce (transform.forward * velocidad);
	}


	void Update () 
	{
		GetComponent<Rigidbody> ().AddForce(transform.forward * 1);
	}

	// El control de las colisiones de la nave enemiga
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boundary")
		{
			return;
		}
		/*Instantiate(explosion, transform.position, transform.rotation); // as GameObject;*/
		
		if (other.tag == "Player")
		{
			Instantiate(explosion, transform.position, transform.rotation);
			gameController.TakeDamage(amount);
			gameController.AddScore(scoreValue);
			vidaEnemigo = vidaEnemigo - 3;
		}
		
		if (other.tag == "DisparoJugador")
		{
			Debug.Log(vidaEnemigo);
			Instantiate(explosion, transform.position, transform.rotation);
			DestroyObject(other.gameObject);
			vidaEnemigo--;
		}
		
		if (other.tag == "Disparo10")
		{
			return;
		}
		
		// A este if habra que añadirle todos los objetos que no colisionan con la nave enemiga
		
		if (vidaEnemigo <= 0)
		{
			Destroy(gameObject);
			gameController.AddScore(scoreValue);
		}
	}
}
