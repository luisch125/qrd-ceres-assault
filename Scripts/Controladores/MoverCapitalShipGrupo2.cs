using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoverCapitalShipGrupo2 : MonoBehaviour 
{
	// Este script controla los movimientos de las naves capitales en la intro
	public float speed;
	
	void Start()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
		StartCoroutine(HiperSalto());
	}
	
	IEnumerator HiperSalto()
	{
		yield return new WaitForSeconds(30);
		GetComponent<Rigidbody>().velocity = transform.forward * speed * 6500;
	}
}