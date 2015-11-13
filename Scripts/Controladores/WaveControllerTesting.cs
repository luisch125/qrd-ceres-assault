using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Este script maneja el spawn de las oleadas de enemigos en el tutorial.
// La funcion ControlOleadas va llamando progresivamente a las oleadas mediante una variable int llamada tiempoOleadas.
// Cada oleada es una funcion que, al acabar, incrementa a tiempoOleadas, haciendo que se llame a la siguiente funcion.

public class WaveControllerTesting : MonoBehaviour {
		
	//variables referentes a la cantidad y tiempo de las oledas
	public int ordenOleadas;				// variable segun la cual se ordenan las oleadas
	public int cantidadEnemigos;			// enemigos por oleada

	public GameObject enemigoNivel1Basico;		// los siguientes objetos definen cuantos tipos de enemigos habra en las oleadas del nivel
    public GameObject enemigoNivel1Basico2;
	public GameObject enemigoNivel1FormaV;
    public GameObject enemigoNivel1Sentry;
	public GameObject enemigoNivel1TirosDobles;
	public GameObject enemigoNivel1Estatico;
	public GameObject enemigoNivel1Suicida;
	public GameObject enemigoNivel1MidBoss;
	public GameObject enemigoNivel1FinalBoss;
    public GameController gameController;

    public GameObject faderNiveles;			// este objeto es el fundido a negro que activamos al terminar un nivel o terminar la partida
	

	private float bordeXIzquierdo = -5.5f;	// se toma como referencia el borde izquierdo de la pantalla para generar las oleadas enemigas

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

        GameObject gameControllerObject = GameObject.FindWithTag("GameController"); //buscamos el game controller

        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("No puedo encontrar el script 'GameController'");
        }
    }
	void Start () 
	{
		// Volvemos a indicar que la musica de fondo del nivel es independiente del resto de sonidos.
		GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().ignoreListenerVolume = true;
		GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().ignoreListenerPause = true;

		faderNiveles.SetActive(false);		// desactivamos el fundido a negro, porque al inicio no hace falta

		// La primera oleada la iniciamos aqui, el resto se van encadenando desde el ControlOleadas() y CheckEnemigos()
		StartCoroutine(OleadaFormaV());
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
	

	//Control de eneigos para iniciar y pausar entre oleadas
	void CheckEnemigos()
	{
		if (cantidadEnemigos > 0) 
		{
			ordenOleadas++;
		}
	}

	//************* TODAS LAS OLEADAS DE ENEMIGOS **********

	// Los siguientes IEnumerator se corresponden a cada oleada. Las oleadas se llaman en un script individual para cada nivel.

	IEnumerator OleadaBasicaIzq () 
	{
		yield return new WaitForSeconds(6);

		int espera = 0;

		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, 0f, 16);

		for (int i = 0; i < 5; i ++)
		{
			Instantiate (enemigoNivel1Basico, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			for (espera = 1; espera < 30; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
		}

        ordenOleadas++;
        ControlOleadas();
    }

	IEnumerator OleadaBasicaDer () 
	{
		yield return new WaitForSeconds(6);

		int espera = 0;
		
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 10f, 0f, 16);

		for (int i = 0; i < 5; i ++)
		{
			Instantiate (enemigoNivel1Basico, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			for (espera = 1; espera < 30; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
		}

        ordenOleadas++;
        ControlOleadas();
	}

	IEnumerator OleadaBasicaMezclada ()
	{

		yield return new WaitForSeconds (6);

		int espera = 0;

		Vector3 posicionIzq = new Vector3 (bordeXIzquierdo + 1f, 0f, 16);
		Vector3 posicionDer = new Vector3 (bordeXIzquierdo + 10f, 0f, 16);

		for (int i = 0; i < 12; i ++)
		{
			if (i % 2 == 0)
			{
				Instantiate (enemigoNivel1Basico, new Vector3(posicionIzq.x, posicionIzq.y, posicionIzq.z), Quaternion.Euler(0.0f, 180, 0.0f));
			}
			else
			{
				Instantiate (enemigoNivel1Basico, new Vector3(posicionDer.x, posicionDer.y, posicionDer.z), Quaternion.Euler(0.0f, 180, 0.0f));
			}

			for (espera = 1; espera < 30; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
		}

        ordenOleadas++;
        ControlOleadas();
    }


	IEnumerator OleadaAgresiva ()
	{
        yield return new WaitForSeconds(6);

        float espera = 1;
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, 0f, 16);

		for (int i = 0; i < cantidadEnemigos; i ++)
		{
			Instantiate (enemigoNivel1Basico, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + 3;
			for (espera = 1; espera < 20; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
		}

		yield return new WaitForSeconds (1);

		for (int i = 0; i < cantidadEnemigos; i ++)
		{
			Instantiate (enemigoNivel1Basico, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + 3;
			for (espera = 1; espera < 20; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
		}

        ordenOleadas++;
        ControlOleadas();
    }

	IEnumerator OleadaDisparo1 ()
	{
        yield return new WaitForSeconds(6);

        float espera = 1;
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, 0f, 16);
		
		for (int i = 0; i < 6; i ++)
		{
			Instantiate (enemigoNivel1Basico, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
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

        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaSentriesYDuros ()
    {
        yield return new WaitForSeconds(6);
        float posicionZ = 16;
        Vector3 posicionIzqDuro = new Vector3(bordeXIzquierdo + 3f, 0f, 20);
        Vector3 posicionDerDuro = new Vector3(bordeXIzquierdo + 6f, 0f, 20);

        for (int i = 0; i < 6; i ++)
        {
            yield return new WaitForFixedUpdate();



            Vector3 posicionIzqSentry = new Vector3(Random.Range (bordeXIzquierdo, bordeXIzquierdo + 2f), 0f, posicionZ);
            Vector3 posicionDerSentry = new Vector3(Random.Range (bordeXIzquierdo + 8f, bordeXIzquierdo + 10f), 0f, posicionZ);

            Instantiate(enemigoNivel1Sentry, posicionIzqSentry, Quaternion.identity);
            Instantiate(enemigoNivel1Sentry, posicionDerSentry, Quaternion.identity);

            posicionZ += 4;

            if (i % 6 == 0)
            {
                Instantiate(enemigoNivel1TirosDobles, posicionIzqDuro, Quaternion.Euler(0, 180f, 0));
                Instantiate(enemigoNivel1TirosDobles, posicionDerDuro, Quaternion.Euler(0, 180f, 0));
            }

        }

        ordenOleadas++;
        ControlOleadas();
    }

	IEnumerator OleadaDisparo2 ()
	{
        yield return new WaitForSeconds(6);
        float espera = 1;
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, 0f, 16);
		
		for (int i = 0; i < 6; i ++)
		{
			Instantiate (enemigoNivel1Basico2, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + 3;
			
			for (espera = 1; espera < 20; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
		}

        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaBoss()
    {
        yield return new WaitForSeconds(6);

        Instantiate(enemigoNivel1FinalBoss, new Vector3(bordeXIzquierdo + 3f, 0f, 18), Quaternion.identity);
    }

	IEnumerator OleadaDura ()
	{

		yield return new WaitForSeconds (6);

		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, 0f, 16);

		for (int i = 0; i < cantidadEnemigos / 2; i ++)
		{
			Instantiate (enemigoNivel1TirosDobles, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler (0.0f, 180, 0.0f));
			posicion.x = posicion.x + 8;
		}


        ordenOleadas++;
        ControlOleadas();

    }

    IEnumerator OleadaDeSentrys ()
    {

        yield return new WaitForSeconds(6);

        int espera = 0;
        int enemigosIzquierda = Random.Range(2, 4);
        int enemigosDerecha = Random.Range(2, 4);

        for (int i = 0; i <= enemigosIzquierda; i ++)
        {
            Vector3 posicionIzquierda = new Vector3(Random.Range(-5.5f, 0f), 0f, Random.Range(17, 20));
            Instantiate(enemigoNivel1Sentry, posicionIzquierda, transform.rotation);
            for (espera = 1; espera < 75; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        for (int i = 0; i <= enemigosDerecha; i ++)
        {
            Vector3 posicionDerecha = new Vector3(Random.Range(0f, 5.5f), 0f, Random.Range(17, 20));
            Instantiate(enemigoNivel1Sentry, posicionDerecha, transform.rotation);
            for (espera = 1; espera < 75; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        ordenOleadas++;
        ControlOleadas();

    }

    IEnumerator OleadaMezclaHorizontal ()
    {
        yield return new WaitForSeconds(6);
        Vector3 posicionIzquierda = new Vector3(bordeXIzquierdo - 2f, 0f, 8);
        Vector3 posicionDerecha = new Vector3(bordeXIzquierdo + 12f, 0f, 7);

        Instantiate(enemigoNivel1Basico2, posicionIzquierda, Quaternion.Euler(0, 90, 0));
        Instantiate(enemigoNivel1Basico2, new Vector3(posicionIzquierda.x, posicionIzquierda.y, posicionIzquierda.z + 2), Quaternion.Euler(0, 90, 0));
        Instantiate(enemigoNivel1Basico2, new Vector3(posicionIzquierda.x, posicionIzquierda.y, posicionIzquierda.z + 4), Quaternion.Euler(0, 90, 0));
        Instantiate(enemigoNivel1Basico2, new Vector3(posicionIzquierda.x, posicionIzquierda.y, posicionIzquierda.z + 6), Quaternion.Euler(0, 90, 0));
        Instantiate(enemigoNivel1Basico2, posicionDerecha, Quaternion.Euler(0, -90, 0));
        Instantiate(enemigoNivel1Basico2, new Vector3(posicionDerecha.x, posicionDerecha.y, posicionDerecha.z + 2), Quaternion.Euler(0, -90, 0));
        Instantiate(enemigoNivel1Basico2, new Vector3(posicionDerecha.x, posicionDerecha.y, posicionDerecha.z + 4), Quaternion.Euler(0, -90, 0));
        Instantiate(enemigoNivel1Basico2, new Vector3(posicionDerecha.x, posicionDerecha.y, posicionDerecha.z + 6), Quaternion.Euler(0, -90, 0));

        yield return new WaitForFixedUpdate();

        ordenOleadas++;
        ControlOleadas();

    }

    IEnumerator OleadaFormaV()
    {
        yield return new WaitForSeconds(6);
        int espera = 0;


        Vector3 posicion = new Vector3(bordeXIzquierdo + 1f, -0.5f, 16);

        for (int i = 0; i < 8; i ++)
        {
            Instantiate(enemigoNivel1FormaV, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
            for (espera = 1; espera < 40; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaMidBoss()
    {
        yield return new WaitForSeconds(6);
        Vector3 posicion = new Vector3(bordeXIzquierdo-2f, 0f, 9f);

        Instantiate(enemigoNivel1MidBoss, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));


        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaSuicida()

    {
		float posicion = bordeXIzquierdo;

		for (int i = 0; i < 8; i++) 
		{
			Instantiate(enemigoNivel1Suicida, new Vector3(posicion, 0f, 20f), Quaternion.identity);
			posicion += 1.5f;
			yield return new WaitForSeconds(0.5f);
		}

    }

    void OleadaAsteroides()
    {
        return;
    }

    void OleadaEnBlanco()
    {
        return;
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

	// Esta es la funcion que va llamando a las siguientes oleadas cada vez que CheckEnemigos() la llama
	// Esta logica hay que intentar reemplazarla por una logica de listas o diccionarios. Pensare en ello.
	// Un control con switch-case creo que seria suficiente. Tambien puede que sea innecesaria, cada oleada puede llamar a la siguiente...

	void ControlOleadas ()
	{
		if (ordenOleadas == 1)
		{
			OleadaBasicaIzq ();
		}
		else if(ordenOleadas == 2)
		{
			StartCoroutine (OleadaBasicaDer());
		}
		else if(ordenOleadas == 3)
		{										 
			StartCoroutine(OleadaAgresiva());
		}
		else if (ordenOleadas == 4)
		{
			StartCoroutine(OleadaDisparo1());
		}
		else if (ordenOleadas == 5)
		{
			StartCoroutine(OleadaDisparo2());
		}
		else if (ordenOleadas == 6)
		{
			StartCoroutine (OleadaBasicaMezclada());
		}
		else if (ordenOleadas == 7)
		{
			StartCoroutine (OleadaDura());
		}
		else if(GameController.gameOver == true)	//Detiene todas las IEnumerator para no seguir llamando oleadas de enemigos, ya que hemos fracasado
			StopAllCoroutines();
	}
}