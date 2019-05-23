using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // HUD timer, shield, and lightning text reference
    public Text timerText, shieldUIText, lightningUIText, gameOverText, restartTimerText, gameWinnerText;

    private float startTime, timePassed, timeLeft;

    public Image faderScreen;

    public bool timeFinished;

    private const float kMaxTimeSecsLevel1 = 200; 
    public const int kMaxAmtShields = 5;

    int startLivesAmt = 3;
    int startShieldsAmt = 2;

    //############################################################
    // Level restart variables 
    private float restartDelay = 5f;         // Time to wait before restarting the level

    public  Scene scene;
    float restartTimer;                     // Timer to count up to restarting the level

    // Start is called before the first frame update
    void Start()
    {
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

       // get the current scene
       scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
            setShieldAndPowerUpCountText();
            setTimerText();
            gameOverUpdate();  
    }

    void gameOverUpdate() {
        if(Player.IsDead) 
        {
            Vector4 temp;

            temp = new Vector4(faderScreen.color.r,faderScreen.color.g,faderScreen.color.b,1.0f);
            faderScreen.color = temp;
            temp = new Vector4(gameOverText.color.r,gameOverText.color.g,gameOverText.color.b,1.0f);
            gameOverText.color = temp;
           
            // .. increment a timer to count up to restarting.
            restartTimer += Time.deltaTime;

            // restart timer text
            temp = new Vector4(restartTimerText.color.r,restartTimerText.color.g,restartTimerText.color.b,1.0f);
            restartTimerText.color = temp;

            if (restartTimer <= restartDelay)
            {
                restartTimerText.text = "Restarting in \n" + (restartDelay - restartTimer).ToString("f0");
            } 
            
            // .. if it reaches the restart delay...
            if(restartTimer >= restartDelay)
            {
                Debug.Log("Restart Level");
                // .. then reload the currently loaded level.
                resetLevel1();

                // display restart countdown 
                SceneManager.LoadScene(scene.name);
            }

        }
        else if(Player.IsWinner)
        {
            Vector4 temp;

            temp = new Vector4(faderScreen.color.r, faderScreen.color.g, faderScreen.color.b, 1.0f);
            faderScreen.color = temp;
            temp = new Vector4(gameWinnerText.color.r, gameWinnerText.color.g, gameWinnerText.color.b, 1.0f);
            gameWinnerText.color = temp;

            // .. increment a timer to count up to restarting.
            restartTimer += Time.deltaTime;

            // restart timer text
            temp = new Vector4(restartTimerText.color.r, restartTimerText.color.g, restartTimerText.color.b, 1.0f);
            restartTimerText.color = temp;

            if (restartTimer <= restartDelay)
            {
                restartTimerText.text = "Restarting in \n" + (restartDelay - restartTimer).ToString("f0");
            }

            // .. if it reaches the restart delay...
            if (restartTimer >= restartDelay)
            {
                Debug.Log("Restart Level");
                // .. then reload the currently loaded level.
                resetLevel1();

                // display restart countdown 
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    void resetLevel1()
    {
        Player.IsDead = false;
        Player.IsWinner = false;
        Player.NumOfLives = startLivesAmt;
        Player.NumOfShields = startShieldsAmt;
        startTime = kMaxTimeSecsLevel1;
        restartTimer = 0;
    }
    
    void setShieldAndPowerUpCountText() {
        // Get Player Shield amount 
        shieldUIText.text = Player.NumOfShields.ToString() + "/" + kMaxAmtShields.ToString();
        lightningUIText.text = Player.NumOfLives.ToString();  
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
           Player.IsDead = true;
           return; 
       }

       string minutes = ((int)timeLeft / 60).ToString();
       string seconds = (timeLeft % 60).ToString("f2");

       timerText.text = minutes + ": " + seconds; 
    }
}
