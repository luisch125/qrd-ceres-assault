using UnityEngine;
using System.Collections;

public class enemigoBasico : MonoBehaviour {
		
	public GameObject explosion;				// Este es el objeto de la explosion del enemigo
	public GameObject playerExplosion;			// Este es el objeto de la explosion del jugador
	public GameObject bala;						// Este es el objeto del disparo enemigo
	
	public int vidaEnemigo = 10;				// Los puntos de vida del enemigo
	
	private GameController gameController;		// aqui llamamos al script que controla el nivel
    private int scoreValue = 300;				// Los puntos que nos da este enemigo al ser destruido
	private int amount = 30;                    // La cantidad de daño que hacen los disparos de este enemigo
    private float startPos = 0;
    private float fireTimer = 2f;				// La espera entre disparos del enemigo
	
	void Start () 
	{
        
        startPos = gameObject.transform.position.x; // guardamos en startPos la posicion inicial de la nave. Nos sera util para definir su logica segun la oleada en la que participe.

		GetComponent<Rigidbody>().AddForce(transform.forward * 460);  // Este es el empuje inicial al movimiento del enemigo
		
		// Aqui comprobamos que existe el GameController
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

	void FixedUpdate ()
	{
		// Este if controla el resto del movimiento de la nave
		if(gameObject.transform.position.z < 10)
		{
            if (gameObject.transform.position.x < 0 && gameController.mezclandose == false)
            {
                GetComponent<Rigidbody>().AddForce(transform.right * -280 * Time.fixedDeltaTime); 
                transform.Rotate(0, 0, -15 * Time.deltaTime);
            }
            else if (gameObject.transform.position.x > 0 && gameController.mezclandose == false) 
            {
                GetComponent<Rigidbody>().AddForce(transform.right * 280 * Time.fixedDeltaTime);
                transform.Rotate(0, 0, 15 * Time.deltaTime);
            }
            else if (gameController.mezclandose == true && startPos < 0)                                       
            {
                GetComponent<Rigidbody>().AddForce(transform.right * -280 * Time.fixedDeltaTime); 
                transform.Rotate(0, 0, -15 * Time.deltaTime);
            }
            else if (gameController.mezclandose == true && startPos > 0)
            {
                GetComponent<Rigidbody>().AddForce(transform.right * 280 * Time.fixedDeltaTime);
                transform.Rotate(0, 0, 15 * Time.deltaTime);
            }
        }

		// Esta parte controla en que momento puede disparar la nave
		fireTimer -= 1* Time.deltaTime;
		if (fireTimer <= 0.0f) 
		{
			Atacando ();
		}
	}

	// La funcion que controla el disparo enemigo
	void Atacando ()
	{
		fireTimer = 2f;
		Instantiate(bala, transform.position, transform.rotation);
	}

	// El control de las colisiones de la nave enemiga
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Boundary") 
		{
			return;
		}
		Instantiate(explosion, transform.position, transform.rotation); // as GameObject;
		
		if (other.tag == "Player") 
		{
			gameController.TakeDamage (amount);	
			gameController.AddScore (scoreValue);
			vidaEnemigo = vidaEnemigo -3;
		}
		
		if (other.tag == "DisparoJugador")
		{
			DestroyObject(other.gameObject);
			vidaEnemigo--;
		}

		// A este if habra que añadirle todos los objetos que no colisionan con la nave enemiga
		if (other.tag == "Disparo10")
		{
			return;
		}
		
		if (vidaEnemigo <= 0) 
		{
			WaveControllerTutorial.contadorEnemigos--;	// Esta linea es para testear este script en el tutorial. Despues hay que quitarla, ya que este enemigo no sale en el tutorial
			Destroy(gameObject);
			gameController.AddScore (scoreValue);
		}		
	}
}