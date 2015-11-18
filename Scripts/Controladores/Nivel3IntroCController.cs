using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Nivel3IntroCController : MonoBehaviour {

    // Este script controla la intro del nivel 3 y el menú de información que hay y que se puede consultar al acabar el nivel 2
    public GameObject volverButton;             // El botón que nos devuelve de la pantalla del informe de situación al menú de información

    public GameObject tierraLunaButton;         // El botón para consultar información del sistema Tierra-Luna
    public GameObject marteButton;             // El botón para consultar información de Marte
    public GameObject ceresButton;              // El botón para consultar información de Ceres
    public GameObject vestaButton;              // El botón para consultar información de Vesta
    public GameObject palasButton;              // El botón para consultar información de Palas
    public GameObject jupiterButton;            // El botón para consultar información de Jupiter
    public GameObject saturnoButton;            // El botón para consultar información de Saturno

    public GameObject mainCamera;               // La cámara principal de la escena

    GameObject infoTierraLuna;                  // Cuadro de información del sistema Tierra-Luna
    GameObject infoMarte;                       // Cuadro de información de Marte
    GameObject infoCeres;                       // Cuadro de información de Ceres
    GameObject infoVesta;                       // Cuadro de información de Vesta
    GameObject infoPalas;                       // Cuadro de información de Palas
    GameObject infoJupiter;                     // Cuadro de información de Jupiter
    GameObject infoSaturno;                     // Cuadro de información de Saturno


    void Start()
    {
        //Estas líneas definen los cuadros de información y los ocultan en principio, para activarse individualmente al tocar su imagen correspondiente en el mapa
        infoTierraLuna = GameObject.Find("Info Tierra-Luna");
        infoMarte = GameObject.Find("Info Marte");
        infoCeres = GameObject.Find("Info Ceres");
        infoVesta = GameObject.Find("Info Vesta");
        infoPalas = GameObject.Find("Info Palas");
        infoJupiter = GameObject.Find("Info Júpiter");
        infoSaturno = GameObject.Find("Info Saturno");

        infoTierraLuna.SetActive(false);
        infoMarte.SetActive(false);
        infoCeres.SetActive(false);
        infoVesta.SetActive(false);
        infoPalas.SetActive(false);
        infoJupiter.SetActive(false);
        infoSaturno.SetActive(false);


        // Estas dos lineas de codigo permiten separar el comportamiento de la musica de fondo del resto de sonidos del juego.
        // Debemos indicarlo en cada nivel, ya que la musica de fondo cambia con cada nivel
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().ignoreListenerVolume = true;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().ignoreListenerPause = true;

        // Estos dos if seguidos indican si deben estar sonando los sonidos y la musica, aunque solo funcionan una vez hemos jugado una partida.
        // Deben estar aqui para asegurarnos que despues de acabar la partida se mantienen las opciones de sonido elegidas previamente
        if (GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().isOn == false)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().Stop();
        }

        if (GameObject.FindGameObjectWithTag("ControlSonido").GetComponent<Toggle>().isOn == false)
        {
            AudioListener.volume = 0f;
        }

        // Para evitar sobrecargar las funciones Update, añadimos un Listener a los controles de sonido y musica,
        // para que controle si se activan o desactivan. Deben estar en todos los scripts controladores de cada nivel (en los WaveController, por ejemplo)
        GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().onValueChanged.AddListener(OnPointerClick);
        GameObject.FindGameObjectWithTag("ControlSonido").GetComponent<Toggle>().onValueChanged.AddListener(OnPointerClick2);
    }

    // Aquí el update sólo controla si usamos el botón atrás del móvil para salir del juego
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

    public void MainRepLoad()
    {
        volverButton.GetComponent<AudioSource>().Play();
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
        Application.LoadLevel("Nivel3-Intro-b");
    }


    //Las siguientes funciones activan el cuadro de información de cada planeta cuando pulsamos en su imagen, en el mapa de la intro3C
    public void TierraLunaInfoOn()
    {
        infoTierraLuna.SetActive(true);
    }

    public void MarteInfoOn()
    {
        infoMarte.SetActive(true);
    }

    public void CeresInfoOn()
    {
        infoCeres.SetActive(true);
    }

    public void VestaInfoOn()
    {
        infoVesta.SetActive(true);
    }

    public void PalasInfoOn()
    {
        infoPalas.SetActive(true);
    }

    public void JupiterInfoOn()
    {
        infoJupiter.SetActive(true);
    }

    public void SaturnoInfoOn()
    {
        infoSaturno.SetActive(true);
    }

    //Las siguientes funciones desactivan el cuadro de información de cada planeta cuando dejamos de pulsar en su imagen, en el mapa de la intro3C
    public void TierraLunaInfoOff()
    {
        infoTierraLuna.SetActive(false);
    }

    public void MarteInfoOff()
    {
        infoMarte.SetActive(false);
    }

    public void CeresInfoOff()
    {
        infoCeres.SetActive(false);
    }

    public void VestaInfoOff()
    {
        infoVesta.SetActive(false);
    }

    public void PalasInfoOff()
    {
        infoPalas.SetActive(false);
    }

    public void JupiterInfoOff()
    {
        infoJupiter.SetActive(false);
    }

    public void SaturnoInfoOff()
    {
        infoSaturno.SetActive(false);
    }

    // Esta funcion se encarga de activar o desactivar la musica, y debe incluirse en el script controlador de cada nivel
    void OnPointerClick(bool isOn)
    {
        if (GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().isOn == true)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().Play();
        }
        else if (GameObject.FindGameObjectWithTag("ControlMusica").GetComponent<Toggle>().isOn == false)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioSource>().Stop();
        }
    }

    // Esta funcion se encarga de activar o desactivar los sonidos, y debe incluirse en el script controlador de cada nivel
    void OnPointerClick2(bool isOn)
    {
        if (GameObject.FindGameObjectWithTag("ControlSonido").GetComponent<Toggle>().isOn == true)
        {
            AudioListener.volume = 0.9f;
        }
        else if (GameObject.FindGameObjectWithTag("ControlSonido").GetComponent<Toggle>().isOn == false)
        {
            AudioListener.volume = 0f;
        }
    }
}
