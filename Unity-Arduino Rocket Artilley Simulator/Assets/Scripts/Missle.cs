using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Uduino;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Missle : MonoBehaviour
{
    /// <summary>
    /// This script is attached to the Missle object and holds a portion of its internal functions including
    /// the remote detonation and self-destruct sequences.
    /// </summary>

    public GameObject explosion;
    public float minHeight = -20f;
    AudioSource distantExplosion;
    public GameObject flare;

    public SphereCollider destructuionRadius;

    public MonoBehaviour guidedMissleScript;
    public GameObject mainArtillery;
    public JoystickInputSystemMapping artilleryMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        distantExplosion = GameObject.Find("DistantMissleExplosion").GetComponent<AudioSource>();
        destructuionRadius = transform.GetComponent<SphereCollider>();
        mainArtillery = GameObject.Find("ArtilleryRocketCannon");
        artilleryMovementScript = mainArtillery.GetComponent<JoystickInputSystemMapping>();
    }

    // Update is called once per frame
    void Update()
    {
        // If rocket flies below minimum height, the rocket is destroyed.
        if (this.transform.position.y < minHeight)
        {
            Destroy(this.gameObject);
        }

        if (Input.GetKey(KeyCode.L))
        {
            remoteDetonate();
            //Destroy(this.gameObject);
        }

        if (Input.GetKey(KeyCode.K))
        {
            guidedMissleScript.enabled = true;
            artilleryMovementScript.enabled = false;
        }
    }

    //If the missle collides with the ground, it will be destroyed
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(explosion, transform.position, transform.rotation);
        distantExplosion.Play();
        Instantiate(flare, transform.position, transform.rotation);
        Destroy(this.gameObject);
        artilleryMovementScript.enabled = true;

    }


    
    public void remoteDetonate()
    {
    
        //Enables blast radius
        destructuionRadius.enabled = true;
        Instantiate(explosion, transform.position, transform.rotation);
        distantExplosion.Play();
        StartCoroutine(WaitToDestroy());
    }



    //If the Jet target is within the blast radius, they are destroyed.
    private void OnCollisionEnter(Collision target)
    {
        if (target.transform.gameObject.tag.Equals("Jet"))
        {
            //Debug.Log("Entered");
            Instantiate(explosion, target.transform.position, target.transform.rotation);
            Destroy(target.gameObject);
        }
    }


    // This destroys the missle after 0.1 seconds. This was done to allow time for the blast radius to detect any
    // targets before the missle is destroyed.
    IEnumerator WaitToDestroy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            Destroy(gameObject);
        }

    }

}



