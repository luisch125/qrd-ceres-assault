using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NavesDestruidasNivel2IntroB : MonoBehaviour {

	//Este script se coloca en las naves destuidas de la intro del nivel 2 y en el mismo nivel 2. Crea el movimiento aleatorio de las naves

	public void Awake () 
	{
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 0.1f;
	}	
}