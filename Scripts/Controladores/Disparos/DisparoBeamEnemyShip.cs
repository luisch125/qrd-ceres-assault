//This is free to use and no attribution is required
//No warranty is implied or given
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class DisparoBeamEnemyShip : MonoBehaviour {

    public float laserWidth;
    public float noise;
    public float maxLength = 50.0f;
    public Color color;
    public GameObject explosion;

    LineRenderer lineRenderer;
    int length;
    Vector3[] position;
    //Cache any transforms here
    Transform myTransform;
    Transform endEffectTransform;
    //The particle system, in this case sparks which will be created by the Laser
    public ParticleSystem endEffect;
    Vector3 offset;

    private float fireTimer = 0f;          // La espera entre disparos del enemigo
    private float vidaEnemyBeam = 55f;
    private int x = 0;
    GameObject target;                      // El objeto al que apunta el disparo


    // Use this for initialization
    void Start()
    {

        target = GameObject.FindGameObjectWithTag("CapitalShip1");

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetWidth(laserWidth, laserWidth);
        myTransform = transform;
        offset = new Vector3(target.GetComponent<Rigidbody>().position.x, target.GetComponent<Rigidbody>().position.y, target.GetComponent<Rigidbody>().position.z);
        endEffect = GetComponentInChildren<ParticleSystem>();
        if (endEffect)
            endEffectTransform = endEffect.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        fireTimer -= 1 * Time.deltaTime;
        vidaEnemyBeam -= 1 * Time.deltaTime;

        if (fireTimer <= 0.0f && vidaEnemyBeam > 0)
        {
            StartCoroutine(RenderLaser());
        }
    }

    IEnumerator RenderLaser()
    {
        //Shoot our laserbeam forwards!
        
        UpdateLength();

        if(x % 5 == 0)
        {
            Instantiate(explosion, target.transform.position, target.transform.rotation);            
        }
        x++;

        lineRenderer.enabled = true;
        lineRenderer.SetColors(color, color);
        //Move through the Array
        for (int i = 0; i < length; i++)
        {
            //Set the position here to the current location and project it in the forward direction of the object it is attached to
            offset.x = target.GetComponent<Rigidbody>().position.x + Random.Range(-noise, noise);
            offset.y = 0.7f+target.GetComponent<Rigidbody>().position.y + Random.Range(-noise, noise);
            offset.z = 1+target.GetComponent<Rigidbody>().position.z + Random.Range(-noise, noise);
            position[i] = offset;
            position[0] = myTransform.position;

            lineRenderer.SetPosition(i, position[i]);

        }
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        lineRenderer.enabled = false;
        fireTimer = Random.Range(0.3f, 1.2f);
    }

    void UpdateLength()
    {
        //Raycast from the location of the cube forwards
        RaycastHit[] hit;
        hit = Physics.RaycastAll(myTransform.position, -myTransform.forward, maxLength);
        int i = 0;
        while (i < hit.Length)
        {
            //Check to make sure we aren't hitting triggers but colliders
            if (!hit[i].collider.isTrigger)
            {
                length = (int)Mathf.Round(hit[i].distance);
                position = new Vector3[length];
                //Move our End Effect particle system to the hit point and start playing it
                if (endEffect)
                {
                    endEffectTransform.position = hit[i].point;

                    if (!endEffect.isPlaying)
                    {
                        //target.ActivarExplosion();
                        endEffect.Play();
                    }
                }
                lineRenderer.SetVertexCount(length);
                return;
            }
            i++;
        }
        //If we're not hitting anything, don't play the particle effects
        if (endEffect)
        {
            if (endEffect.isPlaying)
                endEffect.Stop();
        }
        length = (int)maxLength;
        position = new Vector3[length];
        lineRenderer.SetVertexCount(length);
    }
}