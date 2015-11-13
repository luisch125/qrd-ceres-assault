using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Este script maneja el spawn de las oleadas de enemigos en el tutorial.
// La funcion ControlOleadas va llamando progresivamente a las oleadas mediante una variable int llamada tiempoOleadas.
// Cada oleada es una funcion que, al acabar, incrementa a tiempoOleadas, haciendo que se llame a la siguiente funcion.

public class WaveControllerNivel2 : MonoBehaviour {
		
	//variables referentes a la cantidad y tiempo de las oledas
	public int cantidadEnemigos;                    // enemigos por oleada

    public GameObject enemigoNivel1Basico;          // los siguientes objetos definen cuantos tipos de enemigos habra en las oleadas del nivel
    public GameObject enemigoNivel1Basico2;
    public GameObject enemigoNivel1Sentry;
    public GameObject enemigoNivel1TirosDobles;
    public GameObject enemigoNivel1Estatico;
    public GameObject enemigoNivel1FormaV;
    public GameObject enemigoNivel1MidBoss;
    public GameObject enemigoNivel1FinalBoss;
    

    public GameObject faderNiveles;                 // este objeto es el fundido a negro que activamos al terminar un nivel o terminar la partida

    public GameObject hazard;           // los asteroides de la demo del juego. Se puede reutilizar aleatoriamente, pero mejor en el script de oleadas, posiblemente
    public GameObject debris;           // las lineas que hacen el efecto de avance de la nave.
    public Vector3 spawnValues;         // los valores que controlan desde donde se generan los objetos enemigos, asteroides y debris
    public float minEscala;             // valor minimo de tamaño de los asteroides, o de cualquier otro objeto que nos interese
    public float maxEscala;             // valor maximo de tamaño de los asteroides, o de cualquier otro objeto que nos interese
    public int hazardCount;             // la cantidad de objetos por oleada. Solo para las oleadas de asteroides o equivalentes, que se generan aleatoriamente
    public float spawnWait;             // la espera entre la generacion de cada asteroide o equivalente
    public float startWait;             // la espera desde el inicio del nivel hasta que se genera la primera oleada de asteroides o equivalente. Candidata a desaparecer
    public float waveWait;              // la espera entre oleadas de asteroides o equivalentes. Otra candidata a desaparecer por el script de control de oleadas

    public GameObject[] DronesApoyo;

    private int debrisCount = 5;        // la cantidad de debris simultaneo en pantalla.
    private float debrisWait = 0.2f;    // la espera entre la generacion de cada objeto de debris

    private int ordenOleadas = 1;                   // variable segun la cual se ordenan las oleadas
    private float bordeXIzquierdo = -5.5f;          // se toma como referencia el borde izquierdo de la pantalla para generar las oleadas enemigas

	void Awake () 
	{
        // Al iniciar cada nivel comprobamos las opciones de sonido y musica
        if (GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().isOn == false)
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

        DronesApoyo[0] = GameObject.Find("DronDeApoyo1");
        DronesApoyo[1] = GameObject.Find("DronDeApoyo2");
    }
	void Start () 
	{
        DronesApoyo[0].SetActive(false);
        DronesApoyo[1].SetActive(false);
        faderNiveles.SetActive(false);      // desactivamos el fundido a negro, porque al inicio no hace falta

        // se inicializan las corrutinas de generacion de asteroides y debris. Evidentemente, la de asteroides es candidata a desaparecer, como las variables que incluye
        StartCoroutine(SpawnWaves());
        StartCoroutine(SpawnDebris());

        // La primera oleada la iniciamos aqui, el resto se van encadenando desde el ControlOleadas() y CheckEnemigos()
        StartCoroutine(OleadaBasicaIzq());
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

    // Esta corrutina controla la generacion de debris. Evidentemente debe ir en todos los controladores de nivel, en caso de que este script
    // no permanezca fijo durante toda la partida.
    IEnumerator SpawnDebris()
    {

        yield return new WaitForSeconds(debrisWait);
        while (true)
        {
            for (int i = 0; i < debrisCount; i++)
            {
                debris.transform.localScale = Vector3.one * Random.Range(minEscala / 5, maxEscala / 5);
                Vector3 debrisPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion debrisRotation = Quaternion.identity;
                Instantiate(debris, debrisPosition, debrisRotation);
                yield return new WaitForSeconds(debrisWait);
            }
            yield return new WaitForSeconds(debrisWait);
        }
    }

    // Esta corrutina controla la generacion de oleadas de asteroides o similares. Debera moverse a los scripts de control de oleadas de cada nivel
    IEnumerator SpawnWaves()
    {

        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                hazard.transform.localScale = Vector3.one * Random.Range(minEscala / 8, maxEscala / 9);
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y - 2, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);

                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    //************* TODAS LAS OLEADAS DE ENEMIGOS **********

    // Los siguientes IEnumerator se corresponden a cada oleada. Cada oleada se llama al terminar la anterior

    IEnumerator OleadaBasicaIzq () 
	{
		yield return new WaitForSeconds(10);

        int espera = 0;

		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, -0.5f, 16);

		for (int i = 0; i < cantidadEnemigos; i ++)
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
		yield return new WaitForSeconds(4);

		int espera = 0;
		
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 10f, -0.5f, 16);

		for (int i = 0; i < cantidadEnemigos; i ++)
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

    IEnumerator OleadaBasicaMezclada()
    {

        yield return new WaitForSeconds(4);

        int espera = 0;

        Vector3 posicionIzq = new Vector3(bordeXIzquierdo + 1f, -0.5f, 16);
        Vector3 posicionDer = new Vector3(bordeXIzquierdo + 10f, -0.5f, 16);

        for (int i = 0; i < cantidadEnemigos * 2; i++)
        {
            if (i % 2 == 0)
            {
                Instantiate(enemigoNivel1Basico, new Vector3(posicionIzq.x, posicionIzq.y, posicionIzq.z), Quaternion.Euler(0.0f, 180, 0.0f));
            }
            else
            {
                Instantiate(enemigoNivel1Basico, new Vector3(posicionDer.x, posicionDer.y, posicionDer.z), Quaternion.Euler(0.0f, 180, 0.0f));
            }

            for (espera = 1; espera < 30; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaDisparo1 ()
	{
        yield return new WaitForSeconds(4);
        int espera = 1;
		Vector3 posicion = new Vector3 (bordeXIzquierdo + 1f, -0.5f, 16);
		
		for (int i = 0; i < cantidadEnemigos; i ++)
		{
			Instantiate (enemigoNivel1Basico, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
			posicion.x = posicion.x + 3;
			
			for (espera = 1; espera < 20; espera ++)
			{
				yield return new WaitForFixedUpdate();
			}
			if (cantidadEnemigos == (i+1))
			{
				yield return new WaitForSeconds(21);
				ordenOleadas ++;
				ControlOleadas ();
			}
			else if (cantidadEnemigos == 0)
			{
				ControlOleadas ();
			}
		}
    }

    IEnumerator OleadaDisparo1b()
    {
        yield return new WaitForSeconds(4);
        int espera = 1;
        Vector3 posicion = new Vector3(bordeXIzquierdo + 1f, -0.5f, 16);

        for (int i = 0; i < 4; i++)
        {
            Instantiate(enemigoNivel1Basico2, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
            posicion.x = posicion.x + 3;

            for (espera = 1; espera < 20; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaDisparo2()
    {
        yield return new WaitForSeconds(4);
        int espera = 1;
        Vector3 posicion = new Vector3(bordeXIzquierdo + 1f, -0.5f, 16);

        for (int i = 0; i < 4; i++)
        {
            Instantiate(enemigoNivel1Basico2, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
            posicion.x = posicion.x + 3;

            for (espera = 1; espera < 20; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaAgresiva ()
	{
        yield return new WaitForSeconds(4);

        int espera = 1;
        Vector3 posicion = new Vector3(bordeXIzquierdo + 1f, -0.5f, 16);

        for (int i = 0; i < cantidadEnemigos; i++)
        {
            Instantiate(enemigoNivel1Basico, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
            posicion.x = posicion.x + 3;
            for (espera = 1; espera < 20; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaDura ()
	{
        yield return new WaitForSeconds(4);

        Vector3 posicion = new Vector3(bordeXIzquierdo + 1f, 0f, 16);

        for (int i = 0; i < cantidadEnemigos / 2; i++)
        {
            Instantiate(enemigoNivel1TirosDobles, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
            posicion.x = posicion.x + 8;
        }

        yield return new WaitForSeconds(5);
        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaDeSentrys()
    {
        yield return new WaitForSeconds(4);

        int espera = 0;
        int enemigosIzquierda = Random.Range(2, 4);
        int enemigosDerecha = Random.Range(2, 4);

        for (int i = 0; i <= enemigosIzquierda; i++)
        {
            Vector3 posicionIzquierda = new Vector3(Random.Range(-5.5f, 0f), 0f, Random.Range(17, 20));
            Instantiate(enemigoNivel1Sentry, posicionIzquierda, transform.rotation);
            for (espera = 1; espera < 75; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        for (int i = 0; i <= enemigosDerecha; i++)
        {
            Vector3 posicionDerecha = new Vector3(Random.Range(0.5f, 5.5f), 0f, Random.Range(17, 20));
            Instantiate(enemigoNivel1Sentry, posicionDerecha, transform.rotation);
            for (espera = 1; espera < 75; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        yield return new WaitForSeconds(3);
        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaMezclaHorizontal()
    {
        yield return new WaitForSeconds(4);

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

        yield return new WaitForSeconds(5);
        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaFormaV()
    {
        yield return new WaitForSeconds(6);
        int espera = 0;


        Vector3 posicion = new Vector3(bordeXIzquierdo + 1f, -0.5f, 16);

        for (int i = 0; i < 8; i++)
        {
            Instantiate(enemigoNivel1FormaV, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));
            for (espera = 1; espera < 40; espera++)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        yield return new WaitForSeconds(2);
        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator OleadaMidBoss()
    {
        yield return new WaitForSeconds(6);

        Vector3 posicion = new Vector3(bordeXIzquierdo - 2f, 0f, 9f);

        Instantiate(enemigoNivel1MidBoss, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));


        yield return new WaitForSeconds(1);
        ordenOleadas++;
        ControlOleadas();
    }

    IEnumerator FinalBoss()
    {
        yield return new WaitForSeconds(6);

        Vector3 posicion = new Vector3(0f, 0f, 15f);

        Instantiate(enemigoNivel1FinalBoss, new Vector3(posicion.x, posicion.y, posicion.z), Quaternion.Euler(0.0f, 180, 0.0f));

        yield return new WaitForSeconds(6);
        DronesApoyo[0].SetActive(true);

        DronesApoyo[0].GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(80, 110));
        DronesApoyo[0].GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(80, 110));

        yield return new WaitForSeconds(3);
        DronesApoyo[1].SetActive(true);

        DronesApoyo[1].GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(80, 110));
        DronesApoyo[1].GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-80, -110));


        yield return new WaitForSeconds(100);
        ordenOleadas++;
        ControlOleadas();
    }



    //************* FIN DE TODAS LAS OLEADAS DE ENEMIGOS **********


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
            OleadaBasicaIzq();
        }
        else if (ordenOleadas == 2)
        {
            StartCoroutine(OleadaBasicaDer());
        }
        else if (ordenOleadas == 3)
        {
            StartCoroutine(OleadaBasicaMezclada());
        }
        else if (ordenOleadas == 4)
        {
            StartCoroutine(OleadaBasicaMezclada());
        }
        else if (ordenOleadas == 5)
        {
            StartCoroutine(OleadaDisparo1());
        }
        else if (ordenOleadas == 6)
        {
            StartCoroutine(OleadaAgresiva());
        }
        else if (ordenOleadas == 7)
        {
            StartCoroutine(OleadaDura());
        }
        else if (ordenOleadas == 8)
        {
            StartCoroutine(OleadaDeSentrys());
        }
        else if (ordenOleadas == 9)
        {
            StartCoroutine(OleadaDisparo1b());
        }
        else if (ordenOleadas == 10)
        {
            StartCoroutine(OleadaBasicaMezclada());
        }
        else if (ordenOleadas == 11)
        {
            StartCoroutine(OleadaMezclaHorizontal());
        }
        else if (ordenOleadas == 12)
        {
            StartCoroutine(OleadaDura());
        }
        else if (ordenOleadas == 13)
        {
            StartCoroutine(OleadaDura());
        }
        else if (ordenOleadas == 14)
        {
            StartCoroutine(OleadaBasicaIzq());
        }
        else if (ordenOleadas == 15)
        {
            StartCoroutine(OleadaBasicaDer());
        }
        else if (ordenOleadas == 16)
        {
            StartCoroutine(OleadaDeSentrys());
        }
        else if (ordenOleadas == 17)
        {
            StartCoroutine(OleadaBasicaMezclada());
        }
        else if (ordenOleadas == 18)
        {
            StartCoroutine(OleadaMezclaHorizontal());
        }
        else if (ordenOleadas == 19)
        {
            StartCoroutine(OleadaFormaV());
        }
        else if (ordenOleadas == 20)
        {
            StartCoroutine(OleadaFormaV());
        }
        else if (ordenOleadas == 21)
        {
            StartCoroutine(OleadaDeSentrys());
        }
        else if (ordenOleadas == 22)
        {
            StartCoroutine(OleadaDura());
        }
        else if (ordenOleadas == 23)
        {
            StartCoroutine(OleadaDura());
        }
        else if (ordenOleadas == 24)
        {
            StartCoroutine(OleadaDisparo1());
        }
        else if (ordenOleadas == 24)
        {
            StartCoroutine(OleadaBasicaMezclada());
        }
        else if (ordenOleadas == 25)
        {
            StartCoroutine(FinalBoss());
        }
        else if (GameController.gameOver == true)    //Detiene todas las IEnumerator para no seguir llamando oleadas de enemigos, ya que hemos fracasado
        {
            ordenOleadas = -10;
            StopAllCoroutines();
        }
	}
}