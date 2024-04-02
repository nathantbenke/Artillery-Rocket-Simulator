using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class LaunchMissle : MonoBehaviour
{
    /// <summary>
    /// This holds bulk of code for interfacing with the Arduino as well as other functionalites for affect the status of the simulator
    /// </summary>


    //Arduino Plug tutorial:
    //2-4 - Buttons
    int currFireButtonVal = 0;
    int prevFireButtonVal = 0;
    private int fireButton = 22;


    int currIncLaunchVal = 0;
    int prevIncLaunchVal = 0;
    private int incLaunchPowerButton = 13;

    int currDecLaunchVal = 0;
    int prevDecLaunchVal = 0;
    private int decLaunchPowerButton = 3;
    ArtilleryController controller;


    //LED Display
    //GND - Ground, VCC - Power Supply, DIO - Data I/O pin / CLK is clock output pin

    //30-31 - LED Display 1
    //public int ammoDisplayPin = 5;
    // public int ammoPin = 6;


    //34-35 - LED Display 2
    //10-11 - Toggle Switch
    private int armedToggleSwitch = 8;
    private int guidedMissleToggleSwitch = 5;
    [SerializeField] private int armedToggleValue = 0;
    [SerializeField] private int guidedMissleToggleValue = 0;


    //12-13 - LED
    private int ledPin1 = 11; // 12 
    private int ledPin2 = 10; // 11





    //Artillery launch variables
    public GameObject artilleryShell;
    Rigidbody shellRB;
    public Transform firingPoint;
    public GameObject explosion;
    [SerializeField] public float launchPower;
    public float powerMult = 1;
    public AudioSource ArtilleryFiringSound;


    public bool toggleSwitchArmed = false;
    public bool toggleSwitchGuidedMissle = false;


    public JoystickInputSystemMapping artilleryMovementScript;
    public GuidedMissleControl guidedMissleScript;
    public Missle missleScript;
    public GameObject mainArtillery;



    [Range(0, 5)]
    public int val1 = 5;

    [Range(3000, 7000)]
    public int val2 = 6000;


    // Start is called before the first frame update
    void Start()
    {
        controller = this.transform.GetComponent<ArtilleryController>();
        UduinoManager.Instance.pinMode(fireButton, PinMode.Input_pullup);
        UduinoManager.Instance.pinMode(incLaunchPowerButton, PinMode.Input_pullup);
        UduinoManager.Instance.pinMode(decLaunchPowerButton, PinMode.Input_pullup);
        UduinoManager.Instance.pinMode(armedToggleSwitch, PinMode.Input_pullup);
        UduinoManager.Instance.pinMode(guidedMissleToggleSwitch, PinMode.Input_pullup);

        UduinoManager.Instance.pinMode(ledPin1, PinMode.Output);
        UduinoManager.Instance.pinMode(ledPin2, PinMode.Output);

        // UduinoManager.Instance.pinMode(ammoPin, PinMode.Input_pullup);
        // UduinoManager.Instance.pinMode(ammoDisplayPin, PinMode.Output);

        UduinoManager.Instance.OnDataReceived += ReceviedData;
        StartCoroutine(Loop()); // Reduce the send rate


        mainArtillery = GameObject.Find("ArtilleryRocketCannon");
        artilleryMovementScript = mainArtillery.GetComponent<JoystickInputSystemMapping>();

        launchPower *= powerMult;
        val2 = (int)launchPower;
    }

    //Used to send data to Arduino to be processed into LED displays
    IEnumerator Loop()
    {
        while (true)
        {
            UduinoManager.Instance.sendCommand("command", val1, val2);
            yield return new WaitForSeconds(0.1f);
        }
    }

    //Debugging Arduino send/recieve
    void ReceviedData(string data, UduinoDevice device)
    {
        Debug.Log(data);
    }

    // Update is called once per frame
    void Update()
    {
        //SendToArduino((int)totalAmmo);
        //UduinoDevice board = UduinoManager.Instance.GetBoard(arduinoName);
        //UduinoManager.Instance.sendCommand(board, "TOTAL_AMMO", totalAmmo);
        //UduinoManager.Instance.sendCommand("TOTAL_AMMO", totalAmmo.ToString());

        //1: Unpressed, 0: Pressed
        currFireButtonVal = UduinoManager.Instance.digitalRead(fireButton);
        armedToggleValue = UduinoManager.Instance.digitalRead(armedToggleSwitch);
        guidedMissleToggleValue = UduinoManager.Instance.digitalRead(guidedMissleToggleSwitch);
        currIncLaunchVal = UduinoManager.Instance.digitalRead(incLaunchPowerButton);
        currDecLaunchVal = UduinoManager.Instance.digitalRead(decLaunchPowerButton);


        //turns on LEDS. 0: On, 1: Off
        if (armedToggleValue == 0)
        {
            UduinoManager.Instance.digitalWrite(ledPin1, State.HIGH);
            toggleSwitchArmed = true;
        }
        else
        {
            UduinoManager.Instance.digitalWrite(ledPin1, State.LOW);
            toggleSwitchArmed = false;
        }

        if (guidedMissleToggleValue == 0)
        {
            UduinoManager.Instance.digitalWrite(ledPin2, State.HIGH);
            toggleSwitchGuidedMissle = true;
        }
        else
        {
            UduinoManager.Instance.digitalWrite(ledPin2, State.LOW);
            toggleSwitchGuidedMissle = false;
        }


        //Increase Launch Power
        if (currIncLaunchVal != prevIncLaunchVal)
        {
           // Debug.Log("Reached Increase 1, currIncLaunchVal: " + currIncLaunchVal + "       ,prevIncLaunchVal: " + prevIncLaunchVal);
            if (currIncLaunchVal == 0)
            {
                launchPower += 200;
                Debug.Log("Launch Power Increased: " + launchPower.ToString());
                val2 = (int)launchPower;

            }
            prevIncLaunchVal = currIncLaunchVal;
        }
        
        //Decrease Launch Power
        if (currDecLaunchVal != prevDecLaunchVal)
        {
            if (currDecLaunchVal == 0)
            {
                launchPower -= 100;
                Debug.Log("Launch Power Decreased: " + launchPower.ToString());
                val2 = (int)launchPower;
            }
            prevDecLaunchVal = currDecLaunchVal;
        }


        //Debug.Log("currFireButtonVal: " + currFireButtonVal);
        Debug.Log("armedToggleValue: " + armedToggleValue + " guidedMissleToggleValue: " + guidedMissleToggleSwitch);
        Debug.Log("toggleSwitchArmed: " + toggleSwitchArmed + " toggleSwitchGuidedMissle: " + toggleSwitchGuidedMissle);
        // Debug.Log("guidedMissleToggleValue: " + guidedMissleToggleSwitch);
        //Debug.Log("currFireButtonVal: " + currFireButtonVal + " currIncLaunchVal: " + currIncLaunchVal + " currDecLaunchVal: " + currDecLaunchVal);

        Debug.Log("toggleSwitchGuidedMissle: "+ toggleSwitchGuidedMissle);

        if (toggleSwitchGuidedMissle)
        {
            artilleryMovementScript.enabled = false;
        }
        else
        {
            artilleryMovementScript.enabled = true;
        }


        if (toggleSwitchArmed && !toggleSwitchGuidedMissle)
        {
            Debug.Log("Artillery ARMED");
            //Launch missle enabled.
            if (currFireButtonVal != prevFireButtonVal)
            {
                if (currFireButtonVal == 0 && controller.totalAmmo > 0 && Time.time > controller.nextFire)
                {
                    //controller.LaunchMissle();
                    Debug.Log("Rocket Launched");
                    controller.launchPower = launchPower;
                    controller.nextFire = Time.time + controller.fireRate;
                    controller.LaunchMissle();
                    val1 = (int)controller.totalAmmo;
                    if (controller.totalAmmo != 0)
                    {
                        controller.ReloadingShell.PlayDelayed(3);
                    }
                    //                    LaunchRocket();

                }
                prevFireButtonVal = currFireButtonVal;
            }


        }
        else if (toggleSwitchArmed && toggleSwitchGuidedMissle)
        {
            //Switch to manual explosion and/or control of rocket.
            Debug.Log("Guide System Activated");

            if (controller.missleCopy != null)
            {
                guidedMissleScript = controller.missleCopy.GetComponent<GuidedMissleControl>();
                missleScript = controller.missleCopy.GetComponent<Missle>();
                guidedMissleScript.enabled = true;
            }

            artilleryMovementScript.enabled = false;

            if (currFireButtonVal != prevFireButtonVal)
            {
                if (currFireButtonVal == 0)
                {
                    if (controller.missleCopy != null)
                        missleScript.remoteDetonate();
                }
                prevFireButtonVal = currFireButtonVal;
            }

        }
        else
        {
            //Nothing or RELOAD?
        }

    }

    //Unused
    /*
    public void LaunchRocket()
    {
        totalAmmo -= 1;
        //UduinoManager.Instance.sendCommand("receiveAmmoCount", totalAmmo);
        GameObject missleCopy = Instantiate(artilleryShell, firingPoint.position, firingPoint.rotation) as GameObject;
        shellRB = missleCopy.GetComponent<Rigidbody>();
        shellRB.AddForce(-transform.up * launchPower);
        GameObject launchExplosion = Instantiate(explosion, firingPoint.position, firingPoint.rotation);
        ArtilleryFiringSound.Play();
    }
    */
}
