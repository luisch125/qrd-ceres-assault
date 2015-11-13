using UnityEngine;
using System.Collections;

public class WaveControllerSurvival : MonoBehaviour {

    // Vale, este es el script del modo Survival. Te va spawneando oleadas aleatorias, cada vez mas dificiles.
    // IDEAS:
    // 1º: Gradacion de dificultad
    // En un principio (mediante un random.range) las oleadas seran escogidas de entre una lista de oleadas "sencillas". Despues, iremos incrementando el rango incluyendo
    // oleadas mas complejas.
    // 2º: Dificultad acomodada
    // de alguna manera podriamos intentar medir si el jugador esta limitandose a esquivar las oleadas o a matarlas (enemigosTotales, o algo asi).
    // si el jugador esta luchando por sobrevivir, podriamos rebajar el rango (esta idea es dificil de implementar, asi que es de prioridad baja).
    // 3º: tabla de puntuacion.
    // Francamente no tengo ni idea de como implementar esto. Usando un fichero externo? Guardando un array de variables? Ni idea.

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
