using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class ArtilleryController : MonoBehaviour
{
    /// <summary>
    /// This is the first artillery controller script which holds logic for the keyboard simulation.
    /// The script also includes launchRocket() which is used by the Arduino fire button.
    /// </summary>

    //Rotation Variables
    public GameObject moveableArtillery;
    public GameObject centralArtillery;
    public int speed;
    public float friction;
    public float lerpSpeed;
    Camera overviewCamera;

    //Artillery launch variables
    public GameObject artilleryShell;
    Rigidbody shellRB;
    public Transform firingPoint;
    public GameObject explosion;
    public float launchPower;
    public float powerMult = 1;
    public GameObject missleCopy;

    //Ammo Count
    public int totalAmmo = 3;
    public AudioSource ArtilleryFiringSound;
    public AudioSource HydraulicStart;
    public AudioSource HydraulicMiddle;
    public AudioSource HydraulicEnd;

    public float fireRate = 13f;
    public float nextFire = 0f;
    public AudioSource ReloadingShell;


    // Start is called before the first frame update
    void Start()
    {
        overviewCamera = Camera.main;
        launchPower *= powerMult;
        UduinoManager.Instance.pinMode(12, PinMode.Output);
        //fireableRocketLED();
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time < nextFire || totalAmmo == 0)
        {
           // UduinoManager.Instance.digitalWrite(12, State.LOW);
        }
        else
        {
           // UduinoManager.Instance.digitalWrite(12, State.HIGH);
        }



        //Up button to move MoveableArtillery
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            HydraulicStart.Play();
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            moveableArtillery.transform.Rotate(Vector3.right * lerpSpeed *  friction);
            if (HydraulicStart.isPlaying != false)
            {
                HydraulicMiddle.Play();
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            if (HydraulicMiddle.isPlaying || HydraulicStart.isPlaying)
            {
                HydraulicMiddle.Stop();
                HydraulicStart.Stop();
            }
            HydraulicEnd.Play();
        }


        //Down button to move MoveableArtillery
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            HydraulicStart.Play();
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            moveableArtillery.transform.Rotate(Vector3.left * lerpSpeed * friction);
            if (HydraulicStart.isPlaying != false)
            {
                HydraulicMiddle.Play();
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            if (HydraulicMiddle.isPlaying || HydraulicStart.isPlaying)
            {
                HydraulicMiddle.Stop();
                HydraulicStart.Stop();
            }
            HydraulicEnd.Play();
        }


        //Left button to move centralArtillery
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            HydraulicStart.Play();
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            centralArtillery.transform.Rotate(Vector3.down * lerpSpeed * friction);
            if (HydraulicStart.isPlaying != false)
            {
                HydraulicMiddle.Play();
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            if (HydraulicMiddle.isPlaying || HydraulicStart.isPlaying)
            {
                HydraulicMiddle.Stop();
                HydraulicStart.Stop();

            }
            HydraulicEnd.Play();
        }





        //Right button to move centralArtillery
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            HydraulicStart.Play();
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            centralArtillery.transform.Rotate(Vector3.up * lerpSpeed * friction);
            if (HydraulicStart.isPlaying != false)
            {
                HydraulicMiddle.Play();
            }
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            if (HydraulicMiddle.isPlaying || HydraulicStart.isPlaying)
            {
                HydraulicMiddle.Stop();
                HydraulicStart.Stop();

            }
            HydraulicEnd.Play();
        }


        //LED to show that you can fire another missle

        if (Input.GetKeyDown(KeyCode.Space) && totalAmmo > 0 && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            LaunchMissle();
            if (totalAmmo != 0)
            {
                ReloadingShell.PlayDelayed(3);
            }
        }

    }

    /*
    private void fireableRocketLED()
    {

            if ((Time.time > nextFire && totalAmmo > 0) || totalAmmo == 5)
            {
                UduinoManager.Instance.digitalWrite(12, State.HIGH);
            }
            else
            {
                UduinoManager.Instance.digitalWrite(12, State.LOW);
            }

    }
    */

    /*
     * This script is used to instantiate missle objects from the artilery cannon.
     * Creates Shell, applies propulsion, creates explosion, plays sound effect.
     */
    public void LaunchMissle()
    {
        totalAmmo -= 1;
        missleCopy = Instantiate(artilleryShell, firingPoint.position, firingPoint.rotation) as GameObject;
        shellRB = missleCopy.GetComponent<Rigidbody>();
        shellRB.AddForce(transform.up * launchPower);
        GameObject launchExplosion = Instantiate(explosion, firingPoint.position, firingPoint.rotation);
        ArtilleryFiringSound.Play();
    }
}
