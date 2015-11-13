using UnityEngine;
using System.Collections;

public class enemigoTirosDobles : MonoBehaviour { // esta nave es una nave de aguante intermedio. Dispara una ráfaga de cuatro disparos dobles antes de empezar a escapar... mientras sigue descargando la municion
                                                     //El patron es sencillo. Entra en la pantalla, baja hasta Z:12 y ahi empieza a retirarse hacia atras lentamente, antes de dar un empujon fuerte hacia el borde izquierdo de la pantalla.                                                     

	public GameObject bala;                 //la bala que vamos a usar...
    public int vidaEnemigo = 24;                // Los puntos de vida del enemigo
    public GameObject explosion;                // Este es el objeto de la explosion del enemigo
    public GameObject playerExplosion;          // Este es el objeto de la explosion del jugador

    private GameController gameController;      // aqui llamamos al script que controla el nivel
    private int scoreValue = 300;               // Los puntos que nos da este enemigo al ser destruido
    private int amount = 15;                    // La cantidad de daño que hacen los disparos de este enemigo
    float fireTimer = 1;                    //un timer para controlar el intervalo de disparos.
    int disparos = 0;                       //un timer para contar la cantidad de disparos antes de escapar.
    bool ataque = false;                    //un bool que determina si hemos empezado a atacar, para poder escapar de la condicion de ataque por la posicion de la nave en el entorno de juego. Un uso muy creativo de un OR logico!

	void Start () 
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

        GetComponent<Rigidbody> ().AddForce(transform.forward * 180);       //el empujoncito para empezar             
	}

    void FixedUpdate ()
    {
        if (gameObject.transform.position.z < 12 || ataque == true)         //comprobamos que la nave ha entrado en la pantalla antes de disparar. Usamos el OR para seguir atacando una vez hayamos traspasado la posicion inicial.
        {
            fireTimer -= 1 * Time.deltaTime;                                //cuenta atras para los disparos.
            if (fireTimer <= 0.0f)                                          //si fireTimer es menor que cero multiplicado por deltaTime, empezamos a disparar. Bum bum.
            {
                disparos++;                                                 //aumentamos el contador de disparos. La nave escapara cuando haya disparado un cierto numero de veces.
                Atacando();                                                 //... atacando, claro.
            }
        }
        if (ataque == true)                                                 //si la nave ha empezado a atacar, empieza una sibilina retirada hacia la parte superior de la pantalla.
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * -2);     //un empujoncito para volver...
        }

    }

	void Atacando ()
	{
        if (disparos == 4 && gameObject.transform.position.x < 0)                                                  //si no puedes con el enemigo, huye. Cuando haya disparado cuatro veces, la nave empieza a retirarse hacia el borde izquierdo.
        {
            GetComponent<Rigidbody>().AddForce(transform.right * 20);      //y aqui ya se esta yendo. Adios!
        }
        else if (disparos == 4 && gameObject.transform.position.x > 0)
        {
            GetComponent<Rigidbody>().AddForce(transform.right * -20);
        }

        fireTimer = 1;                                                      //reiniciamos el fireTimer para que no se nos llene la pantalla de tiros.
        ataque = true;                                                      //la booleana que nos permite esquivar la condicion de posicion en el FixedUpdate.
        Instantiate(bala, new Vector3(transform.position.x - 0.35f, 0, transform.position.z - 1.0f), transform.rotation);
        Instantiate(bala, new Vector3(transform.position.x + 0.35f, 0, transform.position.z - 1.0f), transform.rotation); // Dakka dakka dakka! Los disparos se instancian en los "cañones" de la nave. 
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
