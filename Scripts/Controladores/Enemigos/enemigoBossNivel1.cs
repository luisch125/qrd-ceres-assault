using UnityEngine;
using System.Collections;

public class enemigoBossNivel1 : MonoBehaviour {

    public GameObject explosion;                // Este es el objeto de la explosion del enemigo
    public GameObject playerExplosion;          // Este es el objeto de la explosion del jugador
    public GameObject bala;                     // Este es el objeto del disparo enemigo
    public GameObject balaSeguimiento;
    public GameObject balaAcelerador;

    public int vidaEnemigo = 300;                // Los puntos de vida del enemigo

	private int vidaInicial = 300;
	private GameController gameController;		// aqui llamamos al script que controla el nivel
    private int scoreValue = 30000;               // Los puntos que nos da este enemigo al ser destruido
    private int amount = 15;                    // La cantidad de daño que hacen los disparos de este enemigo
    private float fireTimer = 4f;               // La espera entre disparos del enemigo
    private int patron = 0;
	int rotacion = 180;							// valor para la rotacion del disparo de Atacando5. Esta fuera para que no se reinicie cada vez que llamamos a la funcion de Atacando5.
	
	void Start ()
    {
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
	

	void FixedUpdate ()
    {
        fireTimer -= 1 * Time.deltaTime;

		if (fireTimer <= 0) 
		{
			Debug.Log (vidaInicial);
			if ((vidaEnemigo > 0.7 * vidaInicial) && patron < 8)
			{
				StartCoroutine(Atacando5 ());
			}
			else if ((vidaEnemigo > 0.7 * vidaInicial) && patron > 7)
			{
				StartCoroutine (Atacando2 ());
			}
			else if ((vidaEnemigo < 0.7 * vidaInicial && vidaEnemigo > 0.3 * vidaInicial) && patron < 8)
			{
				StartCoroutine (Atacando3 ());
			}
			else if ((vidaEnemigo < 0.7 * vidaInicial && vidaEnemigo > 0.3 * vidaInicial ) && patron > 7)
			{
				StartCoroutine (Atacando4 ());
			}
			else if ((vidaEnemigo < 0.3 * vidaInicial) && patron > -1)
			{
				StartCoroutine (Atacando5 ());
			}
		}

	}

    IEnumerator Atacando ()
    {

		fireTimer = 1.25f;
		patron += 1;

		float posicionIzq = gameObject.transform.position.x;
		float posicionDer = gameObject.transform.position.x;


		for (int i = 0; i < 8; i += 1) 
		{
			Instantiate(balaAcelerador, new Vector3(posicionIzq - 0.5f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler (0, 180, 0));
			Instantiate(balaAcelerador, new Vector3(posicionDer + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler (0, 180, 0));
			posicionIzq -= 0.25f;
			posicionDer += 0.25f;
			yield return new WaitForSeconds(0.125f);
		}
    }

    IEnumerator Atacando2 ()
    {
		fireTimer = 1f;

		if (patron < 17) 
		{
			patron += 1;
		} 
		else 
		{
			patron = 0;
		}

		float posicionIzq = gameObject.transform.position.x;
		float posicionDer = gameObject.transform.position.x;

		float rotacionIzq = 220;
		float rotacionDer = 140;

		Vector3 puntoDeLanzamientoIzq = new Vector3 (gameObject.transform.position.x - 0.75f, gameObject.transform.position.y, gameObject.transform.position.z - 0.5f);
		Vector3 puntoDeLanzamientoDer = new Vector3 (gameObject.transform.position.x + 0.75f, gameObject.transform.position.y, gameObject.transform.position.z - 0.5f);

		for (int i = 0; i < 12; i += 1) 
		{
			Instantiate(bala, puntoDeLanzamientoIzq, Quaternion.Euler(0, rotacionIzq, 0));
			Instantiate(bala, puntoDeLanzamientoDer, Quaternion.Euler(0, rotacionDer, 0));
			rotacionIzq -= 7;
			rotacionDer += 7;

			if (i % 6 == 0)
			{
				Instantiate(balaAcelerador, new Vector3(posicionIzq - 0.5f, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler (0, 180, 0));
				Instantiate(balaAcelerador, new Vector3(posicionDer + 0.5f, gameObject.transform.position.y, gameObject.transform.position.z), transform.rotation);
				posicionIzq -= 0.25f;
				posicionDer += 0.25f;
			}

			yield return new WaitForSeconds(0.125f);
		}

	}	
    IEnumerator Atacando3 ()
    {
		fireTimer = 2.25f;
		patron += 1;
		int cantidadDisparos = 32;
		int disparosTirados = 0;

		float posicionX = gameObject.transform.position.x - 2.5f;

		for (int i = 0; i < cantidadDisparos; i++) 
		{
			Instantiate(bala, new Vector3(posicionX, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.Euler (0, 180, 0));
			disparosTirados += 1;

			if (disparosTirados < cantidadDisparos / 2)
			{
				posicionX = posicionX + 0.35f;
			}
			else 
			{
				posicionX = posicionX - 0.35f;
			}

			yield return new WaitForSeconds(0.175f);
		}



    }

    IEnumerator Atacando4()
    {
		fireTimer = 2.125f;

		if (patron < 17) 
		{
			patron += 1;
		} 
		else 
		{
			patron = 0;
		}

		float posicionIzq = gameObject.transform.position.x - 0.75f;
		float posicionDer = gameObject.transform.position.x + 0.75f;
		float posicionCentro = gameObject.transform.position.x;

		int cantidadDisparos = 15;
		
		for (int i = 0; i < cantidadDisparos; i++) 
		{
			Instantiate(balaSeguimiento, new Vector3(posicionIzq, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
			Instantiate(balaSeguimiento, new Vector3(posicionDer, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
			Instantiate(balaSeguimiento, new Vector3(posicionCentro, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
			yield return new WaitForSeconds(0.125f);
		}


    }

    IEnumerator Atacando5()
    {
		yield return new WaitForSeconds (0);

		fireTimer = 0.125f;
		Vector3 puntoSpawn = gameObject.transform.position;
		int rotacionInterna = rotacion;

		for (int i = 0; i < 3; i ++) 
		{
			rotacionInterna = rotacion;
			for (int j = 0; j < 7; j++)
			{
				Instantiate(bala, puntoSpawn, Quaternion.Euler (0, rotacionInterna, 0));
				rotacionInterna += 15;
			}
			rotacion += 8;
		}
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
