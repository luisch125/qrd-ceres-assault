using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartController : MonoBehaviour {

	// Este script esta asignado a la escena del menu principal, al que llegamos justo despues de ver el logo del equipo. Es el encargado de controlar
	// el comportamiento de todos los objetos de la escena.
	public GameObject startButton;			// El boton para empezar la partida
	public GameObject menuButton;			// El boton que nos lleva a las opciones
	public GameObject creditsButton;		// El boton que nos lleva a los creditos
	public GameObject fondoMenu;			// El canvas de las opciones del juego
	public GameObject mainCamera;			// La camara principal de la escena

	void Start()
	{
		// Estas dos lineas de codigo permiten separar el comportamiento de la musica de fondo del resto de sonidos del juego.
		// Debemos indicarlo en cada nivel, ya que la musica de fondo cambia con cada nivel
		GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().ignoreListenerVolume = true;
		GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().ignoreListenerPause = true;

		// Estos dos if seguidos indican si deben estar sonando los sonidos y la musica, aunque solo funcionan una vez hemos jugado una partida.
		// Deben estar aqui para asegurarnos que despues de acabar la partida se mantienen las opciones de sonido elegidas previamente
		if(GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().isOn == false)
		{
			GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().Stop ();
		}
		
		if(GameObject.FindGameObjectWithTag("ControlSonido").GetComponent<Toggle>().isOn == false)
		{
			AudioListener.volume = 0f;
		}

		// Para evitar sobrecargar las funciones Update, añadimos un Listener a los controles de sonido y musica,
		// para que controle si se activan o desactivan. Deben estar en todos los scripts controladores de cada nivel (en los WaveController, por ejemplo)
		GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().onValueChanged.AddListener(OnPointerClick);
		GameObject.FindGameObjectWithTag("ControlSonido").GetComponent<Toggle>().onValueChanged.AddListener(OnPointerClick2);
	}

	// Esta funcion la activa el boton de comenzar a jugar, inicia el movimiento de camara previo al inicio de la partida,
	// y a la corrutina que carga la intro del primer nivel
	public void StartGame ()
	{
		startButton.GetComponent<AudioSource>().Play ();
		mainCamera.GetComponent<Animation>().Play ("camaraMoveStartGame");
		StartCoroutine("LoadAfterAnim");
	}

	// Esta funcion la activa el boton de opciones, y mueve la camara hasta las opciones de sonido y creditos
	public void CameraMoveOptions ()
	{
		mainCamera.GetComponent<Animation>().Play ("camaraMoveOptions");
		menuButton.GetComponent<AudioSource>().Play ();
	}

    // Esta funcion mueve la cámara desde la pantalla de inicio hasta la pantalla de puntuaciones
    public void CameraMoveToScores()
    {
        mainCamera.GetComponent<Animation>().Play("camaraMoveScores");
        creditsButton.GetComponent<AudioSource>().Play();
    }

    // Esta funcion mueve la cámara desde la pantalla de inicio hasta la pantalla de puntuaciones
    public void CameraMoveToStartFromScores()
    {
        mainCamera.GetComponent<Animation>().Play("camaraMoveScoresToStart");
        creditsButton.GetComponent<AudioSource>().Play();
    }

    // Esta funcion devuelve la camara desde las opciones a la pantalla de inicio donde estan los botones de empezar a jugar y de opciones
    public void CameraMoveToPlay ()
	{
		mainCamera.GetComponent<Animation>().Play ("camaraMovePlay");
		menuButton.GetComponent<AudioSource>().Play ();
	}

	// Esta funcion mueve la camara desde la pantalla de opciones hasta la pantalla de creditos
	public void CameraMoveToCredits ()
	{
		mainCamera.GetComponent<Animation>().Play ("camaraMoveCredits");
		menuButton.GetComponent<AudioSource>().Play ();
	}

	// Esta funcion devuelve la camara desde los creditos a la pantalla de inicio donde estan los botones de empezar a jugar y de opciones
	public void CameraMoveToStart ()
	{
		mainCamera.GetComponent<Animation>().Play ("camaraMoveStart");
		creditsButton.GetComponent<AudioSource>().Play ();
	}

	// La corrutina carga la intro a los dos segundos de pulsar el boton de inicio, que es el tiempo que tarda la animacion de la camara en completarse
	public IEnumerator LoadAfterAnim()
	{
		yield return new WaitForSeconds(2);
		DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
		Application.LoadLevelAsync("Nivel1-Intro");
	}

	// Aqui el update solo controla si usamos el boton atras del movil para salir del juego
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				Application.Quit();
			}
		}
	}

	// Esta funcion se encarga de activar o desactivar la musica, y debe incluirse en el script controlador de cada nivel
	void OnPointerClick(bool isOn)
	{
		if(GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().isOn == true)
		{
			GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().Play ();
		}
		else if(GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().isOn == false)
		{
			GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().Stop ();
		}
	}

	// Esta funcion se encarga de activar o desactivar los sonidos, y debe incluirse en el script controlador de cada nivel
	void OnPointerClick2(bool isOn)
	{
		if(GameObject.FindGameObjectWithTag("ControlSonido").GetComponent<Toggle>().isOn == true)
		{
			AudioListener.volume = 0.9f;
		}
		else if(GameObject.FindGameObjectWithTag("ControlSonido").GetComponent<Toggle>().isOn == false)
		{
			AudioListener.volume = 0f;
		}
	}
}
