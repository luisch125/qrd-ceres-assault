using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Nivel3IntroBController : MonoBehaviour {

    // Este script controla la intro del nivel 3 y el menú de información que hay y que se puede consultar al acabar el nivel 2
    public GameObject sitRepButton;             // El botón para consultar el informe de situación
    public GameObject campRepButton;            // El botón consultar el informe de campaña
    public GameObject continuarButton;          // El botón para jugar el nivel 3

    void Start()
    {

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

    //Al seleccionar la opción "informe de situación" se carga la escena correspondiente
    public void SitRepLoad()
    {
        sitRepButton.GetComponent<AudioSource>().Play();
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
        Application.LoadLevel("Nivel3-Intro-c");
    }

    //Al seleccionar la opción "informe de campaña" se carga la escena correspondiente
    public void CampRepLoad()
    {
        sitRepButton.GetComponent<AudioSource>().Play();
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
        Application.LoadLevel("Nivel3-Intro-d");
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
