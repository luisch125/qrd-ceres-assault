using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Este script maneja el spawn de las oleadas de enemigos en el tutorial.
// La funcion ControlOleadas va llamando progresivamente a las oleadas mediante una variable int llamada tiempoOleadas.
// Cada oleada es una funcion que, al acabar, incrementa a tiempoOleadas, haciendo que se llame a la siguiente funcion.

public class WaveControllerTutorial : MonoBehaviour {
		
	//variables referentes a la cantidad y tiempo de las oledas
	public int ordenOleadas;				// variable segun la cual se ordenan las oleadas
	public int cantidadEnemigos;			// enemigos por oleada

	public GameObject enemigoTutorial1;		// los siguientes objetos definen cuantos tipos de enemigos habra en las oleadas del nivel
	public GameObject enemigoTutorial2;
	public GameObject enemigoTutorial3;
	public GameObject enemigoTutorial4;

	public static int contadorEnemigos;		// cuenta los enemigos en pantalla para iniciar las oleadas sucesivamente en CheckEnemigos() si hay 0 enemigos

	public GameObject instruc01;			// los siguientes objetos son las instrucciones que nos van indicando que debemos hacer
	public GameObject instruc02a;
	public GameObject instruc02b;
	public GameObject instruc02c;
	public GameObject instruc03;
	public GameObject instruc04;
	public GameObject instruc05;
	public GameObject instruc06;
	public GameObject instruc07;

	public GameObject faderNiveles;			// este objeto es el fundido a negro que activamos al terminar un nivel o terminar la partida

	//La variable oportunidades solo se usa en el tutorial. La variable descanso hace una pausa entre oleadas
	private int oportunidades = 1;
	private float descanso = 0;

	private float bordeXIzquierdo = -5.5f;	// se toma como referencia el borde izquierdo de la pantalla para generar las oleadas enemigas
	
	private GameController gameController;	// aqui llamamos al script que controla el nivel

	void Awake () 
	{
		// Al iniciar cada nivel comprobamos las opciones de sonido y musica
		if(GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().isOn == false)
		{
			GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().Stop ();
		}

		if(GameObject.FindGameObjectWithTag("ControlSonido").GetComponent<Toggle>().isOn == false)
		{
			AudioListener.volume = 0f;
		}

        // Volvemos a indicar que la musica de fondo del nivel es independiente del resto de sonidos.
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().ignoreListenerVolume = true;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().ignoreListenerPause = true;

        // Para evitar sobrecargar las funciones Update, añadimos un Listener a los controles de sonido y musica,
        // para que controle si se activan o desactivan. Deben estar en todos los scripts controladores de cada nivel (en los WaveController, por ejemplo)
        GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().onValueChanged.AddListener(OnPointerClick);
        GameObject.FindGameObjectWithTag("ControlSonido").GetComponent<Toggle>().onValueChanged.AddListener(OnPointerClick2);
    }
	void Start () 
	{
        // Asignamos el GameController para controlar el Game Over y otras funciones
        GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent <GameController>();

		faderNiveles.SetActive(false);		// desactivamos el fundido a negro, porque al inicio no hace falta

		//Con el contador de enemigos = al maximo de enemigos por oleada controlamos cuando llamamos a la siguiente oleada
		contadorEnemigos = cantidadEnemigos;

		// Estas son las instrucciones del tutorial. Las desactivamos todas para ir activando la que necesitemos en cada momento
		instruc01.SetActive(false);
		instruc02a.SetActive(false);
		instruc02b.SetActive(false);
		instruc02c.SetActive(false);
		instruc03.SetActive(false);
		instruc04.SetActive(false);
		instruc05.SetActive(false);
		instruc06.SetActive(false);
		instruc07.SetActive(false);

		// La primera oleada la iniciamos aqui, el resto se van encadenando desde el ControlOleadas() y CheckEnemigos()
		StartCoroutine(OleadaBasica());
	}

	// Control de la musica del juego. Presente en cada nivel
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

	// Control de los sonidos. Tambien presente en cada nivel
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

	//Desde aqui controlamos siempre que se actualice la funcion de chequeo de enemigos
	void Update()
	{
		CheckEnemigos();
	}

	//Solo para el tutorial, desde aqui nos llevamos a la nave fuera de la pantalla al finalizar el nivel
	void FixedUpdate()
	{
		if (ordenOleadas == 5)
		{
			StartCoroutine(SaltoNave());
		}
	}

	//Control de eneigos para iniciar y pausar entre oleadas
	void CheckEnemigos()
	{
		if(contadorEnemigos > 0)
		{
			descanso = 0;
		}

		//********* CONTROL DEL TUTORIAL PARA COMPROBAR QUE SE ESTA HACIENDO BIEN *********
		//Todos estos else controlan si el jugador esta haciendo bien el tutorial para ir avanzando el nivel. Si no, termina el juego.

		else if (contadorEnemigos <= 0 && GameController.score < 300 && oportunidades > 0)
		{
			instruc01.SetActive(false);
			instruc02b.SetActive(true);
			descanso = descanso + 1 * Time.deltaTime;
			if (descanso >= 3)
			{
				StartCoroutine(OleadaBasica2());
			}
		}
		            else if (contadorEnemigos <= 0 && GameController.score < 500 && oportunidades <= 0)
		            {
			            instruc02b.SetActive(false);
			            instruc02c.SetActive(true);
			            StopAllCoroutines();
			            descanso = descanso + 1 * Time.deltaTime;
			            if (descanso >= 2)
			            {
				            gameController.GameOver();
			            }
		            }
		            else if (contadorEnemigos <= 0 && GameController.score < 700 && oportunidades <= 0)
		            {
			            instruc01.SetActive(false);
			            instruc02b.SetActive(false);
			            instruc02c.SetActive(true);
			            StopAllCoroutines();
			            descanso = descanso + 1 * Time.deltaTime;
			            if (descanso >= 2)
			            {
				            gameController.GameOver();
			            }
		            }
		else if (contadorEnemigos <= 0 && GameController.score >=300 && ordenOleadas == 1)
		{
			instruc01.SetActive(false);
			instruc02a.SetActive(true);
			descanso = descanso + 1 * Time.deltaTime;
			if (descanso >= 3)
			{
				ordenOleadas ++;
				ControlOleadas ();
			}
		}
		else if (contadorEnemigos <= 0 && GameController.score >=700 && ordenOleadas == 2)
		{
			instruc01.SetActive(false);
			instruc02b.SetActive(false);
			instruc02c.SetActive(false);
			instruc03.SetActive(true);
			descanso = descanso + 1 * Time.deltaTime;
			if (descanso >= 3)
			{
				ordenOleadas ++;
				ControlOleadas ();
			}
		}
		else if (contadorEnemigos <= 0 && GameController.score >=900 && ordenOleadas == 3)
		{
			instruc01.SetActive(false);
			instruc02b.SetActive(false);
			instruc02c.SetActive(false);
			instruc03.SetActive(false);
			instruc04.SetActive(true);
			descanso = descanso + 1 * Time.deltaTime;
			if (descanso >= 3)
			{
				ordenOleadas ++;
				ControlOleadas ();
			}
		}
		else if (contadorEnemigos <= 0 && GameController.score >=1100 && ordenOleadas == 4)
		{
			instruc01.SetActive(false);
			instruc02b.SetActive(false);
			instruc02c.SetActive(false);
			instruc03.SetActive(false);
			instruc04.SetActive(false);
			instruc05.SetActive(true);
			descanso = descanso + 1 * Time.deltaTime;
			if (descanso >= 3)
			{
				ordenOleadas ++;
				ControlOleadas ();
			}
		}
		else if (contadorEnemigos <= 0 && (GameController.score < 700 && ordenOleadas == 2 || GameController.score <900 && ordenOleadas == 3 || GameController.score <1100 && ordenOleadas == 4 || GameController.score <1300 && ordenOleadas == 5))
		{
			instruc01.SetActive(false);
			instruc02b.SetActive(false);
			instruc02c.SetActive(false);
			instruc03.SetActive(false);
			instruc04.SetActive(false);
			instruc05.SetActive(false);
			instruc06.SetActive(false);
			instruc07.SetActive(false);
			instruc02c.SetActive(true);
			StopAllCoroutines();
			descanso = descanso + 1 * Time.deltaTime;
			if (descanso >= 2)
			{
				gameController.GameOver();
			}
		}

		//********* FIN DEL CONTROL DEL TUTORIAL **********
	}

	//************* TODAS LAS OLEADAS DE ENEMIGOS **********

	// Los siguientes IEnumerator se corresponden a cada oleada. Cada oleada se llama al terminar la anterior

	IEnumerator OleadaBasica () 
	{
		instruc01.SetActive(true);
		yield return new WaitForSeconds(4);

		float espera = 1;
		descanso = 0;

		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, -0.5f, 16);
		for (int i = 0; i < cantidadEnemigos; i ++)
		{
			Instantiate (enemigoTutorial1, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + 3;
			for (espera = 1; espera < 30; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
		}
	}

	IEnumerator OleadaBasica2 () 
	{
		descanso = 0;
		oportunidades--;
		contadorEnemigos = cantidadEnemigos;
		float espera = 1;
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, -0.5f, 16);
		for (int i = 0; i < cantidadEnemigos; i ++)
		{
			Instantiate (enemigoTutorial1, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + 3;
			for (espera = 1; espera < 30; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
		}
	}

	IEnumerator OleadaAgresiva ()
	{
		descanso = 0;
		oportunidades--;
		contadorEnemigos = cantidadEnemigos;

		float espera = 1;
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, -0.5f, 16);

		for (int i = 0; i < cantidadEnemigos; i ++)
		{
			Instantiate (enemigoTutorial2, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + 3;
			for (espera = 1; espera < 20; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
		}
	}

	IEnumerator OleadaDisparo1 ()
	{
		contadorEnemigos = cantidadEnemigos;
		descanso = 0;
		oportunidades--;

		float espera = 1;
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, -0.5f, 16);
		
		for (int i = 0; i < cantidadEnemigos; i ++)
		{
			Instantiate (enemigoTutorial3, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + 3;
			
			for (espera = 1; espera < 20; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
			if (cantidadEnemigos == (i+1))
			{
				yield return new WaitForSeconds(22);
				ordenOleadas ++;
				ControlOleadas ();
			}
			else if (cantidadEnemigos == 0)
			{
				ControlOleadas ();
			}
		}
	}

	IEnumerator OleadaDisparo2 ()
	{
		contadorEnemigos = cantidadEnemigos;
		descanso = 0;
		oportunidades--;

		float espera = 1;
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, -0.5f, 16);
		
		for (int i = 0; i < cantidadEnemigos; i ++)
		{
			Instantiate (enemigoTutorial4, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + 3;
			
			for (espera = 1; espera < 20; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
		}
	}

	IEnumerator FinTutorial ()
	{
		yield return new WaitForSeconds(2);
		instruc05.SetActive(false);
		instruc06.SetActive(true);
		yield return new WaitForSeconds(6);
		instruc06.SetActive(false);
		instruc07.SetActive(true);
		yield return new WaitForSeconds(10);
        PlayerPrefs.SetInt("Player Score", GameController.score);
        Application.LoadLevel ("Nivel2-Intro-a");

	}

	//************* FIN DE TODAS LAS OLEADAS DE ENEMIGOS **********


	// Solo en el tutorial. Esta funcion se lleva la nave fuera de la pantalla.
	IEnumerator SaltoNave ()
	{
		yield return new WaitForSeconds(15);
		for (float i=1; i<4; i=i+0.1f)
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().boundary.zMax = 16;
			GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().AddForce(transform.forward * (600*i) * Time.fixedDeltaTime);

		}
		faderNiveles.SetActive(true);
	}

	// Funciones reservadas para otras oleadas. Cuando se utilicen tienen que convertirse en IEnumerator
	void OleadaSuicida ()
		
	{
		return;
	}
	
	void OleadaAsteroides ()
	{
		return;
	}
	
	void OleadaEnBlanco ()
	{
		return;
	}

	// Esta es la funcion que va llamando a las siguientes oleadas cada vez que CheckEnemigos() la llama
	// Esta logica hay que intentar reemplazarla por una logica de listas o diccionarios. Pensare en ello.
	// Un control con switch-case creo que seria suficiente. Tambien puede que sea innecesaria, cada oleada puede llamar a la siguiente...

	void ControlOleadas ()
	{
		if (ordenOleadas == 1)
		{
			OleadaBasica ();
		}
		else if(ordenOleadas == 2)
		{										 
			StartCoroutine(OleadaAgresiva());
		}
		else if (ordenOleadas == 3)
		{
			StartCoroutine(OleadaDisparo1());
		}
		else if (ordenOleadas == 4)
		{
			StartCoroutine(OleadaDisparo2());
		}
		else if (ordenOleadas == 5)
		{
			StartCoroutine(FinTutorial());
		}
		else if(GameController.gameOver == true)	//Detiene todas las IEnumerator para no seguir llamando oleadas de enemigos, ya que hemos fracasado
			StopAllCoroutines();
	}
}