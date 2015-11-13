using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Este script controla al jugador: movimiento, disparo y zona de la pantalla por la que puede moverse.

public class DronApoyoController : MonoBehaviour 
{
	public float speed;										// La velocidad de la nave
	public float tilt;										// La inclinacion cuando se mueve a los lados
    public Boundary boundary;
    public float xMin, xMax, zMin, zMax;                    // Los limites de la zona por donde puede moverse la nave
    public GameObject explosion;

    int vida = 100;								
	GameObject motorInf;                                    // Las emisiones de las toberas segun el movimiento de la nave
    GameObject motorIzq;
	GameObject motorDcho;
    GameObject motorSup;


    public GameObject shot;									// El disparo del jugador
	public Transform ShotSpawn;								// El punto de origen del disparo del jugador
	public Transform ShotSpawn2;							// El punto de origen del disparo del jugador
	public float fireRate;                                  // La cadencia de disparo del jugador

    private bool rebote = false;				            // Esta variable controla el cambio de direccion del enemigo
    private float nextFire;                                 // Controla si ha pasado el tiempo definido en fireRate para disparar otra vez

    void Start()
	{
        motorSup = GameObject.Find("engines_player_sup");
        motorInf = GameObject.Find("engines_player_inf");
        motorIzq = GameObject.Find("motorIzq");
        motorDcho = GameObject.Find("motorDcho");

        // Apagamos todos los motores
        motorSup.SetActive(false);
		motorInf.SetActive(false);
		motorIzq.SetActive(false);
		motorDcho.SetActive(false);
    }

    public void LlegadaDrones(int force)
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * (force+Random.Range(80, 110)));

        if (gameObject.transform.position.x < 0)
        {
            GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(80, 110));
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-80, -110));
        }
    }
	void Update ()
	{
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, ShotSpawn.position, ShotSpawn.rotation);
            Instantiate(shot, ShotSpawn2.position, ShotSpawn2.rotation);
            GetComponent<AudioSource>().Play();
        }
        if (vida <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void FixedUpdate ()
	{
        // Aqui se define el movimiento del jugador y los limites de la zona de movimiento, asi como el encendido de los motores segun el movimiento de la nave
        var rigidbody = GetComponent<Rigidbody>();

        rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);

        if (gameObject.transform.position.z > 0)
        {
            motorInf.SetActive(false);
            motorSup.SetActive(true);
            GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(-80, -110) * Time.fixedDeltaTime);
            rebote = true;
        }

        if (gameObject.transform.position.z < 0 && rebote == true)
        {
            motorInf.SetActive(true);
            motorSup.SetActive(false);
            GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(80, 110) * Time.fixedDeltaTime);
        }


        if (gameObject.transform.position.x < -1f && gameObject.transform.position.x > -1.6f)
        {
            motorIzq.SetActive(false);
            motorDcho.SetActive(true);
            //rigidbody.position = new Vector3(rigidbody.position.x - 0.1f, 0, rigidbody.position.z);
            //rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
            GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-500, -700) * Time.fixedDeltaTime);
        }

        if (gameObject.transform.position.x <= -5f && gameObject.transform.position.x < -4f)
        {
            motorIzq.SetActive(true);
            motorDcho.SetActive(false);
            //rigidbody.position = new Vector3(rigidbody.position.x + 0.1f, 0, rigidbody.position.z);
            //rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
            GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(500, 700) * Time.fixedDeltaTime);
        }
        
        else
        {
            if (gameObject.transform.position.x >= 5f && gameObject.transform.position.x > 4f)
            {
                motorIzq.SetActive(false);
                motorDcho.SetActive(true);
                //rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
                //rigidbody.position = new Vector3(rigidbody.position.x - 0.1f, 0, rigidbody.position.z);
                GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-500, -700) * Time.fixedDeltaTime);
            }

            if (gameObject.transform.position.x > 1 && gameObject.transform.position.x < 1.6f)
            {
                motorIzq.SetActive(true);
                motorDcho.SetActive(false);
                //rigidbody.position = new Vector3(rigidbody.position.x + 0.1f, 0, rigidbody.position.z);
                //rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
                GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(500, 700) * Time.fixedDeltaTime);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Disparo10")
        {
            Destroy(other.gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
            vida = vida - 10;
        }
    }
}