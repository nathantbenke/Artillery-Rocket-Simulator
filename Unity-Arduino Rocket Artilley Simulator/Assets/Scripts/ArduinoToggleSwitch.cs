using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class ArduinoToggleSwitch : MonoBehaviour
{
    public int ledPin1 = 12;
    public int ledPin2 = 13;

    public int toggleSwitch;
    public int toggleSwitch2;

    // Start is called before the first frame update
    void Start()
    {
        UduinoManager.Instance.pinMode(toggleSwitch, PinMode.Input_pullup);
        UduinoManager.Instance.pinMode(toggleSwitch2, PinMode.Input_pullup);
        UduinoManager.Instance.pinMode(ledPin1, PinMode.Output);
        UduinoManager.Instance.pinMode(ledPin2, PinMode.Output);

    }

    // Update is called once per frame
    void Update()
    {
        int toggleSwitchVal1 = UduinoManager.Instance.digitalRead(toggleSwitch);
        int toggleSwitchVal2 = UduinoManager.Instance.digitalRead(toggleSwitch2);


        //turns on LEDS
        if (toggleSwitchVal1 == 1)
        {
            UduinoManager.Instance.digitalWrite(ledPin1, State.HIGH);
        } else
        {
            UduinoManager.Instance.digitalWrite(ledPin1, State.LOW);
        }

        if (toggleSwitchVal2 == 1)
        {
            UduinoManager.Instance.digitalWrite(ledPin2, State.HIGH);
        }
        else
        {
            UduinoManager.Instance.digitalWrite(ledPin2, State.LOW);
        }


        if (toggleSwitchVal1 == 1 && toggleSwitchVal2 == 0)
        {
            //Launch missle enabled.
        } else if (toggleSwitchVal1 == 1 && toggleSwitchVal2 == 1) { 
            //Switch to manual explosion and/or control of rocket.
        } else
        {
            //Nothing or RELOAD?
        }


    }
}
