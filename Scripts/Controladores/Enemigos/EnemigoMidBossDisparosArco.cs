using UnityEngine;
using System.Collections;

public class EnemigoMidBossDisparosArco : MonoBehaviour {

    public GameObject explosion;                // Este es el objeto de la explosion del enemigo
    public GameObject playerExplosion;          // Este es el objeto de la explosion del jugador
    public GameObject bala;                     // Este es el objeto del disparo enemigo
    public GameObject balaSegunda;

    public int vidaEnemigo = 90;                // Los puntos de vida del enemigo

    private int vidaInicial = 90;              // puntos de vida con los que empieza el enemigo, para controlar las "fases" del boss.
    private GameController gameController;		// aqui llamamos al script que controla el nivel
    private int scoreValue = 1000;               // Los puntos que nos da este enemigo al ser destruido
    private int amount = 18;                    // La cantidad de daño que hacen los disparos de este enemigo
    private float fireTimer = 1f;				// La espera entre disparos del enemigo
    private int patron = 0;                     // variable para ir cambiando el patron de disparo del MidBoss.
    float grados = 0;

    //Variables de posicion inicial de los disparos enemigos.
    private Vector3 disparos1Izq;
    private Vector3 disparos2Izq;
    private Vector3 disparos3Izq;
    private Vector3 disparos1Der;
    private Vector3 disparos2Der;
    private Vector3 disparos3Der;
    private Vector3 disparosCentro1;
    private Vector3 disparosCentro2;

    void Start()
    {
        // Aqui comprobamos que existe el GameController
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("No puedo encontrar el script 'GameController'");
        }

        GetComponent<Rigidbody>().AddForce(transform.right * 18);
    }

    void FixedUpdate()
    {

        if (gameObject.transform.position.x < 0)
        {
            GetComponent<Rigidbody>().AddForce(transform.right * -6);
        }
        else if (gameObject.transform.position.x > 0)
        {
            GetComponent<Rigidbody>().AddForce(transform.right * 6);
        }

        fireTimer -= 1 * Time.deltaTime;

        if (fireTimer <= 0.0f)
        {
            if (vidaEnemigo >= vidaInicial / 2 && patron <= 10)
            {
                Atacando();
            }
            else if (vidaEnemigo >= vidaInicial / 2 && patron > 10)
            {
                Atacando2();
            }
            else if (vidaEnemigo < vidaInicial / 2 && patron <= 10)
            {
                Atacando3();
            }
            else if (vidaEnemigo < vidaInicial / 2 && patron > 10)
            {
                Atacando4();
            }

        }
    }

    void Atacando()
    {
        patron += 1;
        Instantiate(bala, new Vector3(transform.position.x + 0.65f, transform.position.y, transform.position.z - 1f), transform.rotation);
        Instantiate(bala, new Vector3(transform.position.x + 0.55f, transform.position.y, transform.position.z - 1f), Quaternion.Euler(0, 135, 0));
        Instantiate(bala, new Vector3(transform.position.x + 0.45f, transform.position.y, transform.position.z - 1f), Quaternion.Euler(0, -135, 0));
        Instantiate(bala, new Vector3(transform.position.x - 0.65f, transform.position.y, transform.position.z - 1f), transform.rotation);
        Instantiate(bala, new Vector3(transform.position.x - 0.55f, transform.position.y, transform.position.z - 1f), Quaternion.Euler(0, 135, 0));
        Instantiate(bala, new Vector3(transform.position.x - 0.45f, transform.position.y, transform.position.z - 1f), Quaternion.Euler(0, -135, 0));
        fireTimer = 0.5f;
    }

    void Atacando2()
    {
        if (patron >= 20)
        {
            patron = 0;
        }
        else
        {
            patron += 1;
            Instantiate(balaSegunda, new Vector3(transform.position.x + 0.50f, transform.position.y, transform.position.z + 0.5f), transform.rotation);
            Instantiate(balaSegunda, new Vector3(transform.position.x - 0.50f, transform.position.y, transform.position.z + 0.5f), transform.rotation);
            Instantiate(balaSegunda, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z + 0.5f), transform.rotation);
            Instantiate(balaSegunda, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z + 0.5f), transform.rotation);
            fireTimer = 0.37f;
        }
    }

    void Atacando3()
    {
        patron += 1;
        int disparos = 0;
        for (disparos = 0; disparos <= 10; disparos++)
        {
            Instantiate(bala, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, grados, 0));
            grados = grados + 36;
        }
        fireTimer = 0.25f;
    }

    void Atacando4()
    {
        if (patron >= 20)
        {
            patron = 0;
        }
        else
        {
            patron += 1;
            Instantiate(balaSegunda, new Vector3(transform.position.x + 0.85f, transform.position.y, transform.position.z - 0.7f), transform.rotation);
            Instantiate(balaSegunda, new Vector3(transform.position.x + 0.65f, transform.position.y, transform.position.z - 1f), Quaternion.Euler(0, 135, 0));
            Instantiate(balaSegunda, new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z - 0.85f), Quaternion.Euler(0, -135, 0));
            Instantiate(balaSegunda, new Vector3(transform.position.x - 0.85f, transform.position.y, transform.position.z - 0.7f), transform.rotation);
            Instantiate(balaSegunda, new Vector3(transform.position.x - 0.65f, transform.position.y, transform.position.z - 1f), Quaternion.Euler(0, 135, 0));
            Instantiate(balaSegunda, new Vector3(transform.position.x - 0.45f, transform.position.y, transform.position.z - 0.85f), Quaternion.Euler(0, -135, 0));
            fireTimer = 0.25f;
        }
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
