using UnityEngine;
using System.Collections;

public class enemigoSentry : MonoBehaviour {

    // Esta nave debe disparar tiros de manera circular. He empezado a programarlo y parecia más sencillo.

    public GameObject explosion;                // Este es el objeto de la explosión del enemigo
    public GameObject playerExplosion;          // Este es el objeto de la explosión del jugador
    public GameObject bala;                     // Este es el objeto del disparo enemigo

    public int vidaEnemigo = 20;                // Los puntos de vida del enemigo

    private GameController gameController;		// aquí llamamos al script que controla el nivel
    private int scoreValue = 300;               // Los puntos que nos da este enemigo al ser destruido
    private int amount = 15;                    // La cantidad de daño que hacen los disparos de este enemigo
    private float fireTimer = 2f;               // La espera entre disparos del enemigo
    float grados = 0;


    void Start ()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * -100);

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("No puedo encontrar el script 'GameController'");
        }
    }
	
	void Update ()
    {
        fireTimer -= 1 * Time.deltaTime;

        if (fireTimer <= 0.0f)
        {
            Atacando();
        }
    }

    void Atacando()
    {
        Instantiate(bala, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, grados, 0));
        grados = grados + 27;
        fireTimer = 0.25f;
    }

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
