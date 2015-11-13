using UnityEngine;
using System.Collections;

public class enemigoBasicoRecto : MonoBehaviour {
		
	public GameObject explosion;				// Este es el objeto de la explosion del enemigo
	public GameObject playerExplosion;			// Este es el objeto de la explosion del jugador
	public GameObject bala;						// Este es el objeto del disparo enemigo
	
	public int vidaEnemigo = 10;				// Los puntos de vida del enemigo
    public bool escapando = false;

    private GameController gameController;		// aqui llamamos al script que controla el nivel
    private int scoreValue = 300;				// Los puntos que nos da este enemigo al ser destruido
	private int amount = 30;                    // La cantidad de daño que hacen los disparos de este enemigo
    private float startPos = 0;
    private float fireTimer = 2f;				// La espera entre disparos del enemigo
    private int rebote = 0;

	void Start () 
	{
        
        startPos = gameObject.transform.position.x; // guardamos en startPos la posicion inicial de la nave. Nos sera util para definir su logica segun la oleada en la que participe.

		GetComponent<Rigidbody>().AddForce(transform.forward * 450);  // Este es el empuje inicial al movimiento del enemigo
		
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

    void FixedUpdate()
    {
        // Este if controla el resto del movimiento de la nave

        if (rebote < 3 && gameObject.transform.position.x < -6.5 && startPos > 6)
        {
            Girar();
        }
        else if (rebote < 3 && gameObject.transform.position.x > 6.6 && startPos > 6)
        {
            Girar();
        }

        if (rebote < 3 && gameObject.transform.position.x < -7.6 && startPos < -7)
        {
            Girar();
        }
        else if (rebote < 3 && gameObject.transform.position.x > 6.6 && startPos < -7)
        {
            Debug.Log(startPos);
            Girar();
        }




        // Esta parte controla en que momento puede disparar la nave
        fireTimer -= 1 * Time.deltaTime;
		if (fireTimer <= 0.0f) 
		{
			Atacando ();
		}
	}

	// La funcion que controla el disparo enemigo
	void Atacando ()
	{
		fireTimer = Random.Range(1f, 2f);
		Instantiate(bala, transform.position, transform.rotation);
	}

    // La funcion que controla el giro de la nave
    void Girar ()
    {
        rebote += 1;
        transform.Rotate(0, 180, 0);
        GetComponent<Rigidbody>().AddForce(transform.right * 5);
        GetComponent<Rigidbody>().AddForce(transform.forward * 900);
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
			Destroy(gameObject);
			gameController.AddScore (scoreValue);
		}		
	}
}