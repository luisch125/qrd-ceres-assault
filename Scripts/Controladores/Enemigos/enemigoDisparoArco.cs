using UnityEngine;
using System.Collections;

// Este script corresponde a una nave algo mas grande que las basicas. Debe tener vida, un movimiento lento y con patron
// y deberia disparar tres tiros en arco (90 grados).

public class enemigoDisparoArco : MonoBehaviour {

	public float speed;
	public int vidaNave;
	public int scoreValue;
	public int fireTimer;

	void Update ()
	{
		return;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "DisparoJugador") 
		{
			if (vidaNave > 0)
			{
				vidaNave -= 1;
			}
			else
			{
				Destroy (gameObject);
			}
		}
	}

}
