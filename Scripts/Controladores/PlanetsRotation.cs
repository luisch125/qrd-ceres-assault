using UnityEngine;
using System.Collections;

public class PlanetsRotation : MonoBehaviour {

    public float anguloRotacion;

    void FixedUpdate()
    {
        transform.Rotate(0, 0, anguloRotacion * Time.fixedDeltaTime);
    }
}