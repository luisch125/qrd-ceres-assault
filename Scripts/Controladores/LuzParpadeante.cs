using UnityEngine;
using System.Collections;

public class LuzParpadeante : MonoBehaviour {

    public float luzOn = 0.2f;
    public float luzOff = 2;
    private Light luz;

    void Start ()
    {
        luz = GetComponent<Light>();
        luz.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        luzOff -= 1 * Time.deltaTime;

        if (luzOff <= 0.0f)
        {
            StartCoroutine(EncenderLuz());
        }
    }

    IEnumerator EncenderLuz()
    {
        luz.enabled = true;
        yield return new WaitForSeconds(luzOn);
        luzOff = 2;
        luz.enabled = false;
    }
}
