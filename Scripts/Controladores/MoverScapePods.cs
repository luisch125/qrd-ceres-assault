using UnityEngine;
using System.Collections;

public class MoverScapePods : MonoBehaviour 
{
	// Este script controla la velocidad del disparo del dron del jugador y de los asteroides. Se puede reutilizar para cualquiero objeto que se mueva en linea recta
	// por la pantalla, para controlar su velocidad
	public float speed;

	void Start()
	{
        StartCoroutine(PodsAway());
	}

    IEnumerator PodsAway()
    {
        yield return new WaitForSeconds(50);
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
}
