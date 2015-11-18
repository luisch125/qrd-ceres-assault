using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Nivel3IntroDController : MonoBehaviour {

    // Este script controla la intro del nivel 3 y el menú de información que hay y que se puede consultar al acabar el nivel 2
    public GameObject volverButton;             // El botón que nos devuelve de la pantalla del informe de situación al menú de información
    public GameObject anteriorButton;           // El botón para ir hacia atrás en las páginas del informe de situación
    public GameObject siguienteButton;          // El botón para ir hacia delante en las páginas del informe de situación

    public GameObject pagina1;                  // las páginas del informe de situación
    public GameObject pagina2;
    public GameObject pagina3;
    public GameObject pagina4;

    public GameObject mainCamera;               // La cámara principal de la escena

    int controlPaginacion = 1;                  // Esta variable sirve para controlar en qué página del informe estamos, pudiendo así activar/desactivar los botones de pasar página

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

        // Al empezar a ver el informe comprobamos las condiciones de paginación para activar y desactivar los botones de paginación correspondientes
        // y desactivamos todas las páginas menos la primera

        Paginacion();
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

    // El botón volver activa esta función, que nos devuelve al menú principal de la intro del nivel 3
    public void MainRepLoad()
    {
        volverButton.GetComponent<AudioSource>().Play();
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Menu"));
        Application.LoadLevel("Nivel3-Intro-b");
    }

    // Cada vez que pulsamos los botones de página anterior o siguiente, se suma o resta una unidad a la variable controlPaginación, y se revisa esta función
    // por si hay que activar o desactivar alguno de los botones de pasar página
    public void Paginacion()
    {
        if (controlPaginacion == 1)
        {
            anteriorButton.SetActive(false);
            siguienteButton.SetActive(true);
            pagina1.SetActive(true);
            pagina2.SetActive(false);
            pagina3.SetActive(false);
            pagina4.SetActive(false);

        }
        else if(controlPaginacion == 2)
        {
            anteriorButton.SetActive(true);
            siguienteButton.SetActive(true);
            pagina1.SetActive(false);
            pagina2.SetActive(true);
            pagina3.SetActive(false);
            pagina4.SetActive(false);
        }
        else if (controlPaginacion == 3)
        {
            anteriorButton.SetActive(true);
            siguienteButton.SetActive(true);
            pagina1.SetActive(false);
            pagina2.SetActive(false);
            pagina3.SetActive(true);
            pagina4.SetActive(false);
        }
        else if (controlPaginacion == 4)
        {
            anteriorButton.SetActive(true);
            siguienteButton.SetActive(false);
            pagina1.SetActive(false);
            pagina2.SetActive(false);
            pagina3.SetActive(false);
            pagina4.SetActive(true);
        }
    }

    //Las dos siguientes funciones se asignan a los botones para pasar página
    public void AnteriorPagina()
    {
        controlPaginacion--;
        Paginacion();
    }

    public void SiguientePagina()
    {
        controlPaginacion++;
        Paginacion();
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
