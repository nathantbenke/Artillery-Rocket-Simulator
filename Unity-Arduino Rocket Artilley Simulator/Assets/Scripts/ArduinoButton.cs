using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;
using UnityEngine.WSA;

public class ArduinoButton : MonoBehaviour
{
    ArtilleryController controller;
    public int button = 2;


    int currButtonVal = 0;
    int prevButtonVal = 0;


    // Start is called before the first frame update
    void Start()
    {
        UduinoManager.Instance.pinMode(button, PinMode.Input_pullup);
    }

    // Update is called once per frame
    void Update()
    {
        currButtonVal = UduinoManager.Instance.digitalRead(button);

        if (currButtonVal != prevButtonVal)
        {
            if (currButtonVal == 0)
            {
                controller.LaunchMissle();
            }
            prevButtonVal = currButtonVal;
        }
    }
}
