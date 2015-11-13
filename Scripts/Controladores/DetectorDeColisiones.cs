using UnityEngine;
using System.Collections;

public class DetectorDeColisiones : MonoBehaviour {

    public GameObject dronApoyo;
    GameObject motorIzq;
    GameObject motorDcho;

    GameObject jugador;
    

    void Awake()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");

    }

    void Start()
    {
        motorIzq = GameObject.Find("motorIzq");
        motorDcho = GameObject.Find("motorDcho");
    }

    void Update ()
    {
	
	}
    void OnTriggerEnter(Collider other)
    {
        var rigidbody = dronApoyo.GetComponent<Rigidbody>();

        if (other.tag == "Player" && dronApoyo.transform.position.x < jugador.transform.position.x)
        {
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
            motorIzq.SetActive(false);
            motorDcho.SetActive(true);
        }

        if (other.tag == "Player" && dronApoyo.transform.position.x > jugador.transform.position.x)
        {
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
            motorIzq.SetActive(true);
            motorDcho.SetActive(false);

        }

        if (other.tag == "Disparo10" && dronApoyo.transform.position.x < GameObject.FindGameObjectWithTag("Disparo10").transform.position.x)
        {
            motorIzq.SetActive(false);
            motorDcho.SetActive(true);
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }

        if (other.tag == "Disparo10" && dronApoyo.transform.position.x > GameObject.FindGameObjectWithTag("Disparo10").transform.position.x)
        {
            motorIzq.SetActive(true);
            motorDcho.SetActive(false);
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }

        if (other.tag == "Enemigo" && dronApoyo.transform.position.x < GameObject.FindGameObjectWithTag("Enemigo").transform.position.x)
        {
            motorIzq.SetActive(false);
            motorDcho.SetActive(true);
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }

        if (other.tag == "Enemigo" && dronApoyo.transform.position.x > GameObject.FindGameObjectWithTag("Enemigo").transform.position.x)
        {
            motorIzq.SetActive(true);
            motorDcho.SetActive(false);
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }
    }

}
