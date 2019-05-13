using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // HUD timer, shield, and lightning text reference
    public Text timerText, shieldUIText, lightningUIText, gameOverText;

    private float startTime, timePassed, timeLeft;

    public Image faderScreen;

    public bool timeFinished;

    private const float kMaxTimeSecsLevel1 = 20; 
    public const int kMaxAmtShields = 5;

    // Start is called before the first frame update
    void Start()
    {
       int startLivesAmt = 3;
       Debug.Log("StartLives = " + startLivesAmt.ToString());
       int startShieldsAmt = 2;
       Debug.Log("StartShields = " + startShieldsAmt.ToString());

       // get the start time of the level
       startTime = Time.time; 
       string minutes = ((int)kMaxTimeSecsLevel1 / 60).ToString();
       string seconds = (kMaxTimeSecsLevel1 % 60).ToString();
       string timeLeft = minutes + ": " + seconds;
       timerText.text = timeLeft;
       timeFinished = false;

       // set the lives and shield count
       shieldUIText.text = startShieldsAmt.ToString() + "/" + kMaxAmtShields.ToString();
       lightningUIText.text = startLivesAmt.ToString();

    }

    // Update is called once per frame
    void Update()
    {
      
            setShieldAndPowerUpCountText();
            setTimerText();
            gameOverUpdate();

        
    }

    void gameOverUpdate() {
        if(PlayerController.IsDead) 
        {
            Vector4 temp;

            temp = new Vector4(faderScreen.color.r,faderScreen.color.g,faderScreen.color.b,1.0f);
            faderScreen.color = temp;
            temp = new Vector4(gameOverText.color.r,gameOverText.color.g,gameOverText.color.b,1.0f);
            gameOverText.color = temp;
        }
    }
    void setShieldAndPowerUpCountText() {
        // Get Player Shield amount 
        shieldUIText.text = PlayerController.NumOfShields.ToString() + "/" + kMaxAmtShields.ToString();
        lightningUIText.text = PlayerController.NumOfLives.ToString();  
    }

    void setTimerText() {

       if (timeFinished) {
           return;
       }

       timePassed = Time.time - startTime;
       timeLeft = kMaxTimeSecsLevel1 - timePassed;

       if (timeLeft <= 0) {
           timeFinished = true;
           timerText.color = Color.red;
           timerText.text = "0: 00";

           // player died
           PlayerController.IsDead = true;
           return; 
       }

       string minutes = ((int)timeLeft / 60).ToString();
       string seconds = (timeLeft % 60).ToString("f2");

       timerText.text = minutes + ": " + seconds; 
    }
}
