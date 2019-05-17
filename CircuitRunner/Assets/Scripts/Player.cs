using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] playerSprite;

    public SpriteRenderer playerRenderer;

    public Rigidbody rb;


    // Player properties
    private static int playerLives = 3;
    private static int numOfShields = 2;
    public static int NumOfLives { get => playerLives; set => playerLives = value; }
    public static int NumOfShields { get => numOfShields; set => numOfShields = value; }
    //######################################################################################
    private static bool isGameOver;
    public static bool IsDead { get => isGameOver; set => isGameOver = value; }
    //######################################################################################

    void start() 
    {
        isGameOver = false;

        // Init the num of shields and power ups
        playerLives = 3;
        numOfShields = 2;  

        rb = GetComponent<Rigidbody> ();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other) 
    {
        Debug.Log("Collison");
        if (other.gameObject.CompareTag ("Shield Pick Up")) 
        {      
            if (numOfShields < UIManager.kMaxAmtShields)
            {
                numOfShields++;
            }
            Destroy(other.gameObject);
        } 
        
        if (other.gameObject.CompareTag ("Battery Pick Up"))
        {
            playerLives++;
            Destroy(other.gameObject);
        }  

        if (other.gameObject.CompareTag("Brick Wall"))
        {
            if (playerLives > 1) {
                playerLives--;
            } else {
                isGameOver = true;
            }
        }
    }
}
