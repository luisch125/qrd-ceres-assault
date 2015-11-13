using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour 
{
	// Este script controla la logica basica del juego. Se inicia en el primer nivel.
	// - IMPORTANTE - Hay que revisar si este objeto debe permanecer siempre desde su primera ejecucion
	// o si debemos crearlo y destruirlo en cada nuevo nivel, manteniendo solo el marcador, o incluso guardando una puntuacion por nivel.
	// La salud se regenera en cada nivel, por eso tenemos que decidir segun avance el juego el comportamiento exacto 
	// de este script.
	
	public int startingHealth = 1000; 	               // la integridad del casco del dron. La "vida", para entendernos.
	public static int currentHealth;	               // la vida actual (startingHealth - los daños que se reciban a lo largo de cada nivel). Si llega a cero...
	public Image Fill; 					               // Controla el comportamiento de la barra de integridad del dron.
	[HideInInspector] public static bool gameOver;	   // controla la activacion del "Game over".
	public Slider barraSalud;			               // este es el objeto completo que representa la integridad del dron.
	
	public Text gameOverText;		                   // el texto de game over. Se terminara dejando privada, como todos los textos...
	public Text scoreText;				               // el texto del marcador.
	public Text cuentaAtras;			               // la cuenta atras para continuar la partida (si, como antaño en las recreativas)
	public Text juegoPausado;                          // el texto cuando pausamos el juego. Aparece encima del menu de pausa

    public Text highScoreText;                         // los textos que muestran la puntuación de la partida y la más alta cuando muere el jugador
    public Text highScoreTextCabecera;
    public Text thisGameScoreText;
    public Text thisGameScoreTextCabecera;

    public GameObject restartButton;                	// el boton para continuar la partida
	public GameObject faderNiveles;		                // este objeto es el fundido a negro que hay que activar al finalizar cada nivel

	public static int amount;			                // a esta variable se le da un valor desde cada objeto que colisiona con el jugador. Representa cuanta "vida" le resta al dron.
	[HideInInspector] public static int score;	        // la puntuacion. Hay que guardarla al finalizar la partida para crear una tabla de puntuacion.
    [HideInInspector] public bool mezclandose = true;   // una variable de control para la oleada de naves basicas que se mezclan. Sirve para definir en el script de comportamiento de las naves basicas si siguen moviendose de manera uniforme o no.

    float x = 10;                                       // la variable que controla la cuenta atrás para continuar al activarse el game over

    private int highScore;                              // estas 3 variables se utilizan para controlar los récords de puntuación
    private int[] highScores = new int[5];
    string highScoreKey = "";

    void Awake()
	{
		currentHealth = startingHealth;	                // Según el comentario anterior, al iniciar el nivel, la currenthealth se iguala al valor inicial máximo
	}

	void Start()
	{
        //Get the score and highScore from player prefs if it is there, 0 otherwise.
        highScoreKey = "Record 1";
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);

        if (Application.loadedLevelName == "Nivel1")
            score = 0;
        else
            score = PlayerPrefs.GetInt("Player Score");            

        scoreText.text = "PUNTOS: " + score;
        //---------------------------------------------------------------------------

        cuentaAtras.text = "";			                // ocultamos la cuenta atras al principio, claro
		gameOver = false;				                // de la misma forma, al empezar no es game over

		restartButton.SetActive (false);                // ni debemos continuar la partida si acabamos de empezar
		gameOverText.text = "";			                // también ocultamos el texto game over y los demás
        highScoreText.text = "";
        highScoreTextCabecera.text = "";
        thisGameScoreText.text = "";
        thisGameScoreTextCabecera.text = "";

        // aqui se activan los botones de salir de la pausa y salir de la partida, pero estan fuera del campo de vision hasta que pausamos el juego. 
        GameObject[] GameObjects = (GameObject.FindGameObjectsWithTag("BotonesAMostrar") as GameObject[]);
		for (int i = 0; i < GameObjects.Length; i++)
		{
			GameObjects[i].SetActive(true);
		}
	}

	void Update()
	{
        // este if controla la pausa desde el teclado. Inutil en la version final del juego, consume recursos innecesariamente en un movil
        if (Input.GetKeyDown(KeyCode.P))
		{
			if (Time.timeScale == 1)
			{
				ShowMenu();
				Time.timeScale = 0;
			}
			else
			{
				GameObject.FindGameObjectWithTag("Menu").GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
				GameObject.FindGameObjectWithTag("Menu").transform.position = new Vector3(100,1f,9.5f);
				GameObject.FindGameObjectWithTag("BotonesAMostrar").GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
				GameObject.FindGameObjectWithTag("BotonesAMostrar").transform.position = new Vector3(100,1f,9.5f);
				Time.timeScale = 1;
				juegoPausado.text ="";
			}
		}

		// el if que permite salir del juego desde el boton "atras" del movil.
		if (Input.GetKey(KeyCode.Escape))
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				Application.Quit();
			}
		}
    }

	// Esta funcion se activa al pulsar el boton de pausa en el juego. Mueve al campo de vision del juego todos los objetos del menu de pausa.
	// Las lineas comentadas mas abajo no son utiles por ahora, pero se pueden dejar como referencia. De hecho, lo ideal seria reprogramar todo el
	// sistema de canvas, pero de momento supone bastante trabajo (es lo que pasa cuando se empieza sin saber lo suficiente)
	public void ShowMenu()
	{
		Time.timeScale = 0;
        GameObject.FindGameObjectWithTag("CanvasInstrucciones").GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        GameObject.FindGameObjectWithTag("CanvasInstrucciones").transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        GameObject.FindGameObjectWithTag("Menu").transform.position = new Vector3(0,6f,6.5f);
		GameObject.FindGameObjectWithTag("Menu").transform.rotation = Quaternion.Euler(90f,0f,0f);
		//GameObject.FindGameObjectWithTag("Menu").GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		//GameObject.FindGameObjectWithTag("Menu").GetComponent<Canvas>().pixelPerfect = true;
		//GameObject.FindGameObjectWithTag("Menu").GetComponent<Canvas>().sortingOrder = 14;
		GameObject.FindGameObjectWithTag("Menu").transform.localScale = new Vector3(0.025f,0.025f,0.025f);
		juegoPausado.text ="JUEGO\nPAUSADO";
		
		GameObject.FindGameObjectWithTag("BotonesAMostrar").transform.position = new Vector3(0,1f,6.5f);
		GameObject.FindGameObjectWithTag("BotonesAMostrar").transform.rotation = Quaternion.Euler(90f,0f,0f);
		//GameObject.FindGameObjectWithTag("BotonesAMostrar").GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		//GameObject.FindGameObjectWithTag("BotonesAMostrar").GetComponent<Canvas>().pixelPerfect = true;
		//GameObject.FindGameObjectWithTag("BotonesAMostrar").GetComponent<Canvas>().sortingOrder = 15;
		GameObject.FindGameObjectWithTag("BotonesAMostrar").transform.localScale = new Vector3(0.025f,0.025f,0.025f);

		GameObject.FindGameObjectWithTag("FondoMenu").transform.position = new Vector3(0,1f,6.5f);
		GameObject.FindGameObjectWithTag("FondoMenu").transform.rotation = Quaternion.Euler(90f,0f,0f);
		//GameObject.FindGameObjectWithTag("FondoMenu").GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		//GameObject.FindGameObjectWithTag("FondoMenu").GetComponent<Canvas>().pixelPerfect = true;
		//GameObject.FindGameObjectWithTag("FondoMenu").GetComponent<Canvas>().sortingOrder = 10;
		GameObject.FindGameObjectWithTag("FondoMenu").transform.localScale = new Vector3(0.05f,0.05f,0.05f);
	}

	// Esta funcion se lleva de nuevo fuera del campo de vision todos los objetos del menu de pausa, pero siguen activos
	// (de lo contrario no podriamos controlar que el sonido o la musica esten activados o desactivados en todo momento) y desactiva la pausa
	public void BotonPausaOff()
	{
        GameObject.FindGameObjectWithTag("CanvasInstrucciones").GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        GameObject.FindGameObjectWithTag("Menu").GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
		GameObject.FindGameObjectWithTag("Menu").transform.position = new Vector3(246f,45f,96f);
		GameObject.FindGameObjectWithTag("FondoMenu").GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
		GameObject.FindGameObjectWithTag("FondoMenu").transform.position = new Vector3(100,1f,9.5f);
		GameObject.FindGameObjectWithTag("BotonesAMostrar").GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
		GameObject.FindGameObjectWithTag("BotonesAMostrar").transform.position = new Vector3(100,1f,9.5f);
		Time.timeScale = 1;
	}

	// Esta funcion termina la partida y nos lleva a la pantalla del menu principal. En el futuro nos llevara a la escena de la tabla de puntuacion,
	// y sera desde esa escena desde donde volvamos al menu principal. Nos falta programar aqui como guardar la puntuacion en su fichero correspondiente
	// Ademas vuelve a posicionar el menu de opciones en su lugar correspondiente, porque en la pantalla de inicio sus coordenadas son diferentes.
	public void SalirDelJuego()
	{
		GameObject.FindGameObjectWithTag("Menu").GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
		GameObject.FindGameObjectWithTag("Menu").transform.rotation = Quaternion.Euler(0f,0f,0f);
		GameObject.FindGameObjectWithTag("Menu").transform.position = new Vector3(246f,45f,96f);
		GameObject.FindGameObjectWithTag("Menu").transform.localScale = new Vector3(0.4f,0.4f,0.4f);
		GameObject.FindGameObjectWithTag("BotonesAMostrar").GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
		GameObject.FindGameObjectWithTag("BotonesAMostrar").transform.position = new Vector3(100,1f,9.5f);
		Time.timeScale = 1;
		juegoPausado.text ="";
		faderNiveles.SetActive(true);
		StartCoroutine(SaltoNave2());
	}

	// Esta funcion espera un segundo para cerrar el nivel. Es el tiempo que tarda en completarse el fundido a negro que llamamos al final del nivel
	// o al salir de la partida
	IEnumerator SaltoNave2()
	{
		yield return new WaitForSeconds(1);
		Application.LoadLevel ("Intro");
	}

	// Esta funcion controla la cantidad de salud del dron, el comportamiento de la barra de salud al recibir daño y si llega a cero, activa la funcion "game over"
	public void TakeDamage (int newamount)
	{
		// Reduce la salud por la cantidad que se ha asignado a amount, segun que haya colisionado con el dron.
		currentHealth -= newamount;
		
		// La barra de salud se actualiza a la salud restante.
		barraSalud.value = currentHealth;

		// Si la salud baja de 60 la barra se pone de color amarillo
		if (currentHealth <= 60 && currentHealth > 20)
			Fill.color = Color.yellow;

		// Si la salud baja de 20 la barra se pone de color rojo
		else if (currentHealth <= 20 && currentHealth > 0)
			Fill.color = Color.red;

		// Si la salud llega a cero, activamos la funcion del "game over"
		if (currentHealth <= 0)
		{
			GameOver ();
		}
	}

	// Esta función controla el marcador de la partida
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
        scoreText.text = "PUNTOS: " + score;
    }

	// Esta es la funcion que controla toda la logica del "game over"
	public void GameOver ()
	{
        // cuenta atras para continuar la partida
        StartCoroutine(CuentaAtras());

        Fill.color = Color.red;

		// Estas tres lineas permiten que, mientras queden enemigos en pantalla, sus disparon tengan un objetivo.
		// Si eliminaramos directamente el objeto del dron, al no tener los enemigos un objetivo, el juego daria error.
		// Por eso simplemente lo ocultamos, pero sigue estando activo.
		GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().enabled = false;
		GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>().enabled = false;
		DestroyAllGameObjects();

        // Todos los textos del game over se activan aquí
		currentHealth = 0;
		gameOverText.text = "HAS FRACASADO...";
		restartButton.SetActive (true);
		gameOver = true;

        thisGameScoreText.text = score.ToString();
        thisGameScoreTextCabecera.text = "PUNTOS CONSEGUIDOS:";
        highScoreTextCabecera.text = "RÉCORD ACTUAL:";
        highScoreText.text = "" + highScore;

        //If our score is greater than highscore, set new higscore and save.
        for (int i = 0; i < highScores.Length; i++)
        {

            //Get the highScore from 1 - 5
            highScoreKey = "Record " + (i + 1).ToString();
            highScore = PlayerPrefs.GetInt(highScoreKey, 0);

            //if score is greater, store previous highScore
            //Set new highScore
            //set score to previous highScore, and try again
            //Once score is greater, it will always be for the
            //remaining list, so the top 5 will always be 
            //updated

            if (score > highScore)
            {
                int temp = highScore;
                PlayerPrefs.SetInt(highScoreKey,score);
                score = temp;
            }
        }        
    }

    //Esta función es la cuenta atrás que se inicia con el game over, para dar la oportunidad de continuar la partida
    IEnumerator CuentaAtras()
    {             
        for (x = 10; x >= 0; x--)
        {
            cuentaAtras.text = Mathf.Round(x).ToString();
            yield return new WaitForSeconds(1);
            if (x <= 0)
            {
                DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
                Application.LoadLevel("Intro");
            }
        }
    }

	// Esta funcion destruye todos los objetos enemigos que queden una vez activado el game over
	public void DestroyAllGameObjects()
	{
		GameObject[] GameObjects = (GameObject.FindGameObjectsWithTag("Enemigo") as GameObject[]);
		
		for (int i = 0; i < GameObjects.Length; i++)
		{
			Destroy(GameObjects[i]);
		}
	}

	// Esta funcion permite continuar la partida desde el game over, manteniendo los ajustes de sonido.
	public void RestartGame ()
	{
		//DontDestroyOnLoad(GameObject.FindGameObjectWithTag("BotonesAMostrar"));  // Esta linea creo que sobra, hay que comprobarlo, pero es seguro que sobra porque los botones estan fisicamente en la jerarquia, no los crea el script
		DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
        Application.LoadLevel (Application.loadedLevel);
	}
}