using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// Este script es definitivo (21 - 7 - 2015 10:25)
// Este script manejara el spawn de las diferentes oleadas de enemigos. Mediante funciones definira la forma de las oleadas, la cantidad de enemigos
// y el tiempo entre oleadas.
// La funcion ControlOleadas va llamando progresivamente a las oleadas mediante una variable int llamada ordenOleadas. Cada oleada es una funcion.
// En Update controlamos la duracion de las oleadas y con la comprobacion de la oleadaActual y el tiempo que lleva, pasamos a la siguiente llamando a ControlOleadas.
// Es importante darse cuenta de que la logica del cambio de oleadas (en Update) puede hacerse usando otras variables
// u otras condiciones. Solo hay que cambiar la logica del if.

public class WaveController : MonoBehaviour {


	//variables referentes a la cantidad y tiempo de las oledas
	public int ordenOleadas;
	public int cantidadEnemigos;
	public GameObject enemigoBasico;


	//variables referentes al posicionamiento y movimiento de los enemigos
	private float bordeXIzquierdo = -5.5f;
	private float anchoPantalla = 11f;	
	private float timerOleadas = 0; // Este es el contador interno que las oleadas usaran para llamar a la siguiente.
	private int oleadaActual = 0;   // Cada oleada tiene un numero, que es su orden en este script. Esta variable se usa
	private GameObject finder;	    // para que en Update pueda fijar el tiempo que dura cada oleada y llamar a la funcion
								    // ControlOleadas.					
	void Start () 
	{
		ordenOleadas = 0;
		oleadaActual = 0;
	}

	void Update () //en Update controlaremos el tiempo que dura cada oleada
	{
		finder = GameObject.FindGameObjectWithTag ("MidBoss");
		timerOleadas += 1;

		if (timerOleadas >= 300 && oleadaActual == 0) {
			ordenOleadas += 1;
			ControlOleadas ();
		} 

		else if (timerOleadas >= 600 && oleadaActual == 1) 
		{
			ordenOleadas += 1;
			ControlOleadas ();
		}

		else if (finder == null && oleadaActual == 5)
		{
			ordenOleadas += 1;
			ControlOleadas ();
		}
	}

	void OleadaBasica () 
	{
		oleadaActual = 1;
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1.75f, 0.0f, 22);

		for (int i = 0; i < cantidadEnemigos; i ++)
		{
			Instantiate (enemigoBasico, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + anchoPantalla / 4;
			Debug.Log(ordenOleadas);
		}return;

	}

	void OleadaAgresiva ()
	{
		oleadaActual = 2;
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1.75f, 0.0f, 22);
		
		for (int i = 0; i < cantidadEnemigos + 2; i ++)
		{
			Instantiate (enemigoBasico, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + anchoPantalla / 5;
			Debug.Log(ordenOleadas);
		}return;
	}

	void OleadaSuicida ()

	{
		oleadaActual = 3;
		return;
	}

	void OleadaAsteroides ()
	{
		oleadaActual = 4;
		return;
	}

	void OleadaMidBoss ()
	{
		oleadaActual = 5;
		return;
	}

	void EnemigoFinal()
	{
		oleadaActual = 6;
		return;
	}

	void FinalNivel()
	{
		return;
	}

	void ControlOleadas () // Esta funcion es muy importante. En ella controlamos el tiempo que lleva una oleada y cual es la siguiente oleada.
	{
		timerOleadas = 0;

		if (ordenOleadas == 1) 
		{
			OleadaBasica ();
		}
		else if(ordenOleadas == 2)              //Esta logica hay que intentar reemplazarla por una logica de listas
		{										// o diccionarios. Pensare en ello. ¿Quiza mediante un argumento en la 
			OleadaAgresiva ();					// declaracion de la funcion?
		}
		else if (ordenOleadas == 3)
		{
			OleadaBasica ();
		}

	}
}
