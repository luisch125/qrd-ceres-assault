using UnityEngine;
using System.Collections;

public class DronesApoyoControlColisiones : MonoBehaviour {

    public float xMin, xMax, zMin, zMax;
    public Boundary boundary;

    GameObject jugador;
    
    void Awake ()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");

    }

    void OnTriggerEnter(Collider other)
    {
        var rigidbody = GetComponent<Rigidbody>();

        if (other.tag == "Player" && gameObject.transform.position.x < jugador.transform.position.x)
        {
            //rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
            //rigidbody.position = new Vector3(rigidbody.position.x - 0.1f, 0, rigidbody.position.z);
            //rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
            GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-80, -110) * Time.fixedDeltaTime);
        }

        if (other.tag == "Player" && gameObject.transform.position.x > jugador.transform.position.x)
        {
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }

        if (other.tag == "Disparo10" && gameObject.transform.position.x < GameObject.FindGameObjectWithTag("Disparo10").transform.position.x)
        {
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }

        if (other.tag == "Disparo10" && gameObject.transform.position.x > GameObject.FindGameObjectWithTag("Disparo10").transform.position.x)
        {
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }

        if (other.tag == "Enemigo" && gameObject.transform.position.x < GameObject.FindGameObjectWithTag("Enemigo").transform.position.x)
        {
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }

        if (other.tag == "Enemigo" && gameObject.transform.position.x > GameObject.FindGameObjectWithTag("Enemigo").transform.position.x)
        {
            rigidbody.velocity = new Vector3(-rigidbody.velocity.x, 0, rigidbody.velocity.z);
        }


    }
}
