using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour 
{
	// Este script controla la colision del objeto acoplado al script con el jugador. Se puede dejar para usarlo con asteroides y objetos similares.
	public GameObject explosion;
	public static GameObject playerExplosion;
	public int vidaObjeto;


	private GameController gameController;
	private int scoreValue;
	private int amount = 40;


	void Start ()
	{
		if(transform.localScale.x < 0.83) 
		{
			vidaObjeto = 4;
			scoreValue = 100;
		}
		else if(transform.localScale.x >= 0.84 && transform.localScale.x <= 1.17)
		{
			vidaObjeto = 8;
			scoreValue = 200;
		}
		else if(transform.localScale.x >= 1.18)
		{
			vidaObjeto = 12;
			scoreValue = 400;
		}
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
		}

		vidaObjeto--;

		if (vidaObjeto <= 0) 
		{
			Destroy(gameObject);
			gameController.AddScore (scoreValue);
		}

	}

}
