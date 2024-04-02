using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GuidedMissleControl : MonoBehaviour
{
    /// <summary>
    ///  Attached to the missle. Used to control the rockcet outs the Guided toggle switch has been activated.
    /// </summary>
    /// 
    private Rigidbody rocketRB;
    private float rocketSpeed = 25f;
    [SerializeField] public float drag = .1f;
    [SerializeField] public float rocketTurn = 20f;


    //Joystick Controls
    public ArtilleryMovementControls controls;
    Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    private Vector2 thrustDirection;

    private void OnEnable()
    {
        move = controls.ArtilleryCannon.Move;
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


    // Start is called before the first frame update
    void Start()
    {
        //When the guide systemm is enabled, the physics of the rocket is changed.
        rocketRB = this.transform.GetComponent<Rigidbody>();
        rocketRB.velocity = Vector3.zero;
        rocketRB.drag = 2;
        //rocketRB.useGravity = false;
    }

    private void Update()
    {
        moveDirection = controls.ArtilleryCannon.Move.ReadValue<Vector2>();    
        //Adds propulsion to the missle
        rocketRB.AddForce(transform.up * rocketSpeed);
       
        
        // Debug.Log("Velocity X: " + rocketRB.velocity.x + " Velocity Y: " + rocketRB.velocity.y + " Velocity Z: " + rocketRB.velocity.z);
        //Debug.Log("Velocity: " + rocketRB.velocity);

    }

    private void FixedUpdate()
    {
    thrustDirection = transform.up;
    //rocketRB.AddForce(rocketPropulsion * rocketSpeed);
    //Debug.Log("Velocity: " + rocketRB.velocity);
    //Debug.Log("Joystick Controls: " + moveDirection.magnitude);

    //Joystick commands for controlling the missle
        if (moveDirection.x > 0.1 || moveDirection.x < -0.1)
        {
            this.transform.Rotate(new Vector3(0, 0, -moveDirection.x * rocketTurn * drag));
            rocketRB.AddForce(transform.right* moveDirection.x * rocketTurn);
            //rocketRB.AddForce(-transform.up * rocketSpeed);
        } else
        {
            //rocketRB.velocity = Vector3.zero;
        }

        if ((moveDirection.y > 0.1 || moveDirection.y < -0.1))
        {
            this.transform.Rotate(new Vector3(moveDirection.y * rocketTurn * drag, 0, 0));
            rocketRB.AddForce(transform.forward * moveDirection.y * rocketTurn);
            //rocketRB.AddForce(transform.up * moveDirection.y * rocketTurn);

            //            rocketRB.AddForce(thrustDirection * moveDirection.y * rocketSpeed);
            //rocketRB.AddForce(-transform.up * moveDirection.x * rocketTurn);

            //rocketRB.AddForce(-rocketPropulsion * rocketSpeed);
        } else
        {
            //rocketRB.velocity = Vector3.zero;
        }

    }



}
