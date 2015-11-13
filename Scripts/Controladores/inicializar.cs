using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class inicializar : MonoBehaviour 
{

	//Este script esta asignado a la primera escena, donde se inicializan funciones que deben permanecer activas
	//durante la ejecucion del juego, independientemente de si esta una partida activa o no
	//(por ejemplo, las opciones de audio activado/desactivado. Los objetos del menu de audio se cargan en esta
	//primera escena, a la que ya no vamos a volver en todo el tiempo que el juego este activo). 

	void Start()
	{
		//Llamamos a la corrutina que iniciara el menu principal del juego si no tocamos la pantalla en 4 segundos.
		StartCoroutine(IniciarJuego());
	}
	
	void Update()
	{
		//Desde el primer momento damos la opcion de salir del juego con este if.
		if (Input.GetKey(KeyCode.Escape))
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				Application.Quit();
			}
		}

		//Este if controla los toques de la pantalla para saltar a la siguiente escena.
		// - IMPORTANTE - Los objetos que deben permanecer siempre activos deben indicarse aqui y en cada funcion
		//de salto de escena.
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.Space))
		{
			DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
			Application.LoadLevel("Intro");
		}
	}

	IEnumerator IniciarJuego()
	{
		//Como esta corrutina tambien controla un salto de escena, aqui tambien debemos indicar que objetos
		//deben permanecer activos en la siguiente escena.
		yield return new WaitForSeconds(4);
		DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
		Application.LoadLevelAsync("Intro");
	}
}
