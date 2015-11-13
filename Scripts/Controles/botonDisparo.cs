using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class botonDisparo : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	// Este script controla el comportamiento del boton de disparo.

	private bool touched;					// Esta variable se usa para controlar si se esta tocando la pantalla
	private int pointerID;					// Esta variable controla si el dedo esta sobre el boton
	private bool canFire; 					// Esta variable indica si es el momento de disparar, segun informen las funciones de mas abajo
	
	void Awake () {
		touched = false;					// Empezamos definiendo como falso que se este tocando la pantalla
	}

	// Si tocamos y no estabamos tocando, se activa el disparo
	public void OnPointerDown (PointerEventData data) {
		if (!touched) {
			touched = true;
			pointerID = data.pointerId;
			canFire = true;
		}
	}

	//Si levantamos el dedo, dejamos de disparar
	public void OnPointerUp (PointerEventData data) {
		if (data.pointerId == pointerID) {
			canFire = false;
			touched = false;
		}
	}

	// Esta funcion se llama desde el script del jugador, para indicar que estamos disparando
	public bool CanFire () {
		return canFire;
	}
	
}