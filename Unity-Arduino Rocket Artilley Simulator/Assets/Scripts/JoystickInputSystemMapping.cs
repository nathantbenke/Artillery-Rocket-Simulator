using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class JoystickInputSystemMapping : MonoBehaviour
{
    /// <summary>
    /// Used to control artillery cannons movement with Joystick
    /// </summary>

    public GameObject moveableArtillery;
    public GameObject centralArtillery;
    [SerializeField] private float friction = .5f;
    [SerializeField] private float lerpSpeed = .1f;
    //Camera overviewCamera;
    private Transform artilleryRotX;
    private float artilleryUpward;
    private float storedAngle;

    public ArtilleryMovementControls controls;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    // ArtilleryMovementControls controls;
    //public InputAction control;


    //Due to the way that the new input system works, I was unable recreate the Hydraulic sound effects based on the Joystick inputs.
    public AudioSource HydraulicStart;
    public AudioSource HydraulicMiddle;
    public AudioSource HydraulicEnd;

   // [SerializeField] private float prevStateX = -1;
   // [SerializeField] private float currStateX = 0;
   // [SerializeField] private float prevStateY = -1;
    //[SerializeField] private float currStateY = 0;


    private void OnEnable()
    {
        move = controls.ArtilleryCannon.Move;
        artilleryRotX = moveableArtillery.GetComponent<Transform>();
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Awake()
    {
        controls = new ArtilleryMovementControls();
    }
    /*
     
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    */

    // Update is called once per frame
    void Update()
    {
        moveDirection = controls.ArtilleryCannon.Move.ReadValue<Vector2>();
        //Debug.Log(moveDirection);
    }
    private void FixedUpdate()
        {

        //Used to control the Artillery cannons movement with the Joystick
        if (moveDirection.x > 0.1 || moveDirection.x < -0.1)
        {
            centralArtillery.transform.Rotate(new Vector3(0, moveDirection.x * lerpSpeed * friction, 0));
        }

        if ((moveDirection.y > 0.1 || moveDirection.y < -0.1))
        {
            artilleryUpward = moveDirection.y * lerpSpeed * friction;
            //Debug.Log("Reached");

            if (moveableArtillery.transform.localEulerAngles.x + artilleryUpward >= 270 && moveableArtillery.transform.localEulerAngles.x + artilleryUpward <= 355)
            {
                moveableArtillery.transform.Rotate(new Vector3(artilleryUpward, 0, 0));
            }

        }

    }

}
