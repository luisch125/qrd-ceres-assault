using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class camaraFollowPlayer : MonoBehaviour {

	//Este script se coloca en la camara de la intro del nivel 1 para controlar que siempre mire al dron del jugador.
	//Tambien controla las funciones de audio y si tocamos la pantalla para saltar la intro y cargar el nivel.

	public GameObject camara;				// definimos la camara principal
	public Transform target;				// el objeto al cual va a seguir la camara
	public Text textoSaltarIntro;			// cuando tocamos la pantalla saldra este texto para confirmar si queremos saltar la intro

	int fingerCount = 0;					// esta variable controla la cantidad de toques a la pantalla del movil, asi sabemos cuando saltar la escena

	void Awake () 
	{
		StartCoroutine("LoadAfterAnim");	// iniciamos la corrutina que carga el siguiente nivel al acabar la animacion de la intro
		textoSaltarIntro.text = "";			// este es el texto que avisa de que al segundo toque saltamos la intro. Primero lo ponemos en blanco, claro

		// Como en cada nivel, estas lineas controlan si la musica y los sonidos estan activos
		if(GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().isOn == false)
		{
			GameObject.FindGameObjectWithTag("NavesParent").GetComponent<AudioSource>().Stop ();
		}

		GameObject.FindGameObjectWithTag("NavesParent").GetComponent<AudioSource>().ignoreListenerVolume = true;
		GameObject.FindGameObjectWithTag("NavesParent").GetComponent<AudioSource>().ignoreListenerPause = true;
	}
	
	void Update () 
	{
		// este if es para saltar la intro en el pc. En el juego final es innecesario y hay que quitarlo
		if (Input.GetKeyDown(KeyCode.Space))
		{
			DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
			Application.LoadLevelAsync("Nivel1");
		}

		// Estas lineas controlan que la camara mire al objeto "target" (en este caso, el dron definido como "player", seleccionado en el inspector de Unity)
		Vector3 relativePos = target.position - camara.transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		camara.transform.rotation = rotation;

		// Este es el control de toques en pantalla para saltar la intro
		foreach (Touch touch in Input.touches) 
		{
			if (touch.phase == TouchPhase.Ended)
			{
				fingerCount++;
			}
			if (fingerCount == 1)
			{
				saltarIntro ();				
			}
			if (fingerCount == 2)
			{
				DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
				Application.LoadLevelAsync("Nivel1");
			}
		}
	}

	// Esta es la corrutina que se llama al principio para cargar el siguiente nivel al final de la intro
	public IEnumerator LoadAfterAnim()
	{
		yield return new WaitForSeconds(60);
		DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
		Application.LoadLevelAsync("Nivel1");
	}

	// Esta funcion se activa con el primer toque en la pantalla, a la vez que activa la corrutina que vuelve a ocultar el texto de saltar intro a los 3 segundos
	public void saltarIntro ()
	{
		textoSaltarIntro.text = "toca otra vez para saltar la historia";
		StartCoroutine("NoSaltarIntro");
	}

	// Esta corrutina se activa en la funcion anterior. Si a los 3 segundos no se da un segundo toque en la pantalla, se oculta el texto de saltar intro
	// y se vuelve a cero el recuento de toques en pantalla.
	public IEnumerator NoSaltarIntro()
	{
		yield return new WaitForSeconds(3);
		fingerCount--;
		textoSaltarIntro.text = "";
	}
}
