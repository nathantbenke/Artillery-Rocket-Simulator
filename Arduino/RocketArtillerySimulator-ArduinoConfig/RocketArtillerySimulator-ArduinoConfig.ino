#include<Uduino.h>
Uduino uduino("advancedBoard");


#include <TM1637Display.h>

#define CLK1 3
#define DIO1 4
#define CLK2 7
#define DIO2 8


int ammoCount = 0;
int launchPower = 0;

TM1637Display ammoDisplay = TM1637Display(CLK1, DIO1);
TM1637Display launchPowerDisplay = TM1637Display(CLK2, DIO2);

// Create an array that turns all segments ON
const uint8_t allON[] = {0xff, 0xff, 0xff, 0xff};

// Create an array that turns all segments OFF
const uint8_t allOFF[] = {0x00, 0x00, 0x00, 0x00};

void setup()
{
  Serial.begin(38400);
  uduino.addCommand("command", myCommand);
}

void myCommand() {
  int numberOfParameters = uduino.getNumberOfParameters();
  if (numberOfParameters == 3) {
    int val1 = uduino.charToInt(uduino.getParameter(0));
    int val2 = uduino.charToInt(uduino.getParameter(1));
    int val3 = uduino.charToInt(uduino.getParameter(2));
    ammoCount = val1;
    launchPower = val2;
    Serial.print("Received the parameters ");
    Serial.print(val1);
    Serial.print(", ");
    Serial.print(val2);
    Serial.print(", ");
    Serial.print(val3);
    Serial.println(".");
      
      ammoDisplay.showNumberDec(ammoCount);
      launchPowerDisplay.showNumberDec(launchPower);
  } else {
    Serial.print("3 parameters expected but only");
    Serial.print(numberOfParameters);
    Serial.println(" received.");
  }
}


void loop() {
  //ammoDisplay.showNumberDec(ammoCount);
  //launchPowerDisplay.showNumberDec(launchPower);
  
  
  //ammoDisplay.setSegments(allON);
  
  uduino.update();
}
