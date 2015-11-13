using UnityEngine;
using System.Collections;

public class naveCapital : MonoBehaviour {

    // Este script controla el disparo basico de los enemigos. Este disparo se dirige siempre a la posicion del jugador en el momento de ser disparado

    public GameObject bala;                 // Este es el objeto del disparo
    public GameObject bala2;

    public Transform turret1;               // Las torretas que disparan
    public Transform turret2;
    public Transform turret3;
    public Transform turret4;
    private GameController gameController;	// Este objeto es el controlador del nivel, y es donde esta la funcion que, en este caso, resta vida cuando el disparo choca con el dron
    private float fireTimer = 1f;          // La espera entre disparos del enemigo

    void Start()
    {
        StartCoroutine(EndAtacando());
    }
    void FixedUpdate()
    {
        // Esta parte controla en que momento puede disparar la nave
        fireTimer -= 1 * Time.deltaTime;
        if (fireTimer <= 0.0f)
        {
            StartCoroutine(Atacando());
        }
    }

    // La funcion que controla el disparo enemigo
    IEnumerator Atacando()
    {
        float espera = 1;
        fireTimer = Random.Range(1f,2.5f);

        for (int i = 0; i < Random.Range(4f, 10f); i++)
        {
            Instantiate(bala, turret1.position, turret1.rotation);
            Instantiate(bala, turret2.position, turret2.rotation);
            for (espera = 1; espera < 9; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
        for (int i = 0; i < Random.Range(4f, 10f); i++)
        {
            Instantiate(bala2, turret3.position, turret3.rotation);
            Instantiate(bala2, turret4.position, turret4.rotation);
            for (espera = 1; espera < 9; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    IEnumerator EndAtacando()
    {
        yield return new WaitForSeconds(55);
        fireTimer = 10000;
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 0.07f;
    }

}
