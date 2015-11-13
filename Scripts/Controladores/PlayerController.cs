using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Este script controla al jugador: movimiento, disparo y zona de la pantalla por la que puede moverse.

[RequireComponent(typeof(CharacterController))]
[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;					// Estas son las variables que delimitan la zona por la que puede moverse la nave
}

public class PlayerController : MonoBehaviour 
{
	public float speed;										// La velocidad de la nave
	public float tilt;										// La inclinacion cuando se mueve a los lados
	public Boundary boundary;								// Los limites de la zona por donde puede moverse la nave

	public GameObject motorSup;								// Las emisiones de las toberas segun el movimiento de la nave
	public GameObject motorInf;
	public GameObject motorIzq;
	public GameObject motorDcho;

	public GameObject shot;									// El disparo del jugador
	public Transform ShotSpawn;								// El punto de origen del disparo del jugador
	public Transform ShotSpawn2;							// El punto de origen del disparo del jugador
	public float fireRate;									// La cadencia de disparo del jugador
	public botonDisparo areaButton;							// El boton de disparo

	public CNAbstractController MovementJoystick;			// El mando de movimiento

	private CharacterController _characterController;		// Todo esto es del mando de movimiento
	private Transform _mainCameraTransform;
	private Transform _transformCache;
	private Transform _playerTransform;

	private float nextFire;									// Controla si ha pasado el tiempo definido en fireRate para disparar otra vez

	void Start()
	{
		// Apagamos todos los motores
		motorSup.SetActive(false);
		motorInf.SetActive(false);
		motorIzq.SetActive(false);
		motorDcho.SetActive(false);
	}

	void Update ()
	{
		// Este if controla el disparo del jugador
		if (areaButton.CanFire() && Time.time > nextFire || Input.GetKey (KeyCode.LeftControl) && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate (shot, ShotSpawn.position, ShotSpawn.rotation);
			Instantiate (shot, ShotSpawn2.position, ShotSpawn2.rotation);
			GetComponent<AudioSource>().Play ();
		}
	}
	
	void FixedUpdate ()
	{
		// Aqui se define el movimiento del jugador y los limites de la zona de movimiento, asi como el encendido de los motores segun el movimiento de la nave
		var rigidbody = GetComponent<Rigidbody>();

        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement2 = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        rigidbody.velocity = movement2 * speed;

        //var movement = new Vector3(MovementJoystick.GetAxis("Horizontal"), 0f, MovementJoystick.GetAxis("Vertical"));
        //rigidbody.velocity = movement * speed;
		
		rigidbody.position = new Vector3 
			(
				Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
				0.0f,
				Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
				);
		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);

		// A partir de aqui los if que controlan el encendido de los motores
		if (MovementJoystick.GetAxis("Vertical") < 0)
		{
			motorSup.SetActive(true);
		}
		else
		{
			motorSup.SetActive(false);
		}

		if (MovementJoystick.GetAxis("Vertical") > 0)
		{
			motorInf.SetActive(true);
		}
		else
		{
			motorInf.SetActive(false);
		}

		if (MovementJoystick.GetAxis("Horizontal") < 0)
		{
			motorDcho.SetActive(true);
		}
		else
		{
			motorDcho.SetActive(false);
		}
		
		if (MovementJoystick.GetAxis("Horizontal") > 0)
		{
			motorIzq.SetActive(true);
		}
		else
		{
			motorIzq.SetActive(false);
		}
	}
}