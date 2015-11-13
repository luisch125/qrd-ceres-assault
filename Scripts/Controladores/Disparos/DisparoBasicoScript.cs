using UnityEngine;
using System.Collections;

public class DisparoBasicoScript : MonoBehaviour 

{
	public GameObject explosion;

	private GameController gameController;
	private float speed = 8;
	private int amount = 20;
	
	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			Instantiate (explosion, other.transform.position, other.transform.rotation);
			Destroy (gameObject);
			gameController.TakeDamage(amount);
		}	
	}	
}