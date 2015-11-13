using UnityEngine;
using System.Collections;

public class BackgroundStarsRotation : MonoBehaviour {

    public float anguloRotacion;

	void FixedUpdate ()
    {
        transform.Rotate(anguloRotacion * Time.fixedDeltaTime, 0, 0);
    }
}