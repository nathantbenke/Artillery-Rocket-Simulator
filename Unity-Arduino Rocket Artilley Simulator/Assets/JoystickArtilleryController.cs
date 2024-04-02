using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickArtilleryController : MonoBehaviour
{


    //Rotation Variables
    public GameObject moveableArtillery;
    public GameObject centralArtillery;
    public int speed;
    public float friction;
    public float lerpSpeed;
    Camera overviewCamera;
    public InputAction playerControls;

    //Artillery launch variables
    public GameObject artilleryShell;
    Rigidbody shellRB;
    public Transform firingPoint;
    public GameObject explosion;
    public float launchPower;
    public float powerMult = 1;


    //Ammo Count
    public int totalAmmo = 3;
    public AudioSource ArtilleryFiringSound;
    public AudioSource HydraulicStart;
    public AudioSource HydraulicMiddle;
    public AudioSource HydraulicEnd;

    Vector2 moveDir = Vector2.zero;



    // Start is called before the first frame update
    void Start()
    {
        overviewCamera = Camera.main;
        launchPower *= powerMult;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = playerControls.ReadValue<Vector2>();

        //Down button to move MoveableArtillery
        moveableArtillery.transform.Rotate(new Vector3(moveDir.y * lerpSpeed * friction, 0, 0));


        //Left button to move centralArtillery
        centralArtillery.transform.Rotate(new Vector3(0, moveDir.x * lerpSpeed * friction, 0));



        if (Input.GetKey(KeyCode.Space) && totalAmmo > 0)
        {
            LaunchMissle();
        }

    }


    public void LaunchMissle()
    {
        totalAmmo -= 1;
        GameObject missleCopy = Instantiate(artilleryShell, firingPoint.position, firingPoint.rotation) as GameObject;
        shellRB = missleCopy.GetComponent<Rigidbody>();
        shellRB.AddForce(-transform.up * launchPower);
        GameObject launchExplosion = Instantiate(explosion, firingPoint.position, firingPoint.rotation);
        ArtilleryFiringSound.Play();
    }
}
