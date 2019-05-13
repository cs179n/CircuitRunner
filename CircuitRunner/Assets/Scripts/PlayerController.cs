using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // CONSTANTS
    public static int kStartNumShields;
    public static int kStartNumLives;

    public Sprite[] playerSprite;

    public SpriteRenderer playerRenderer;

    public Rigidbody rb;


    // Player properties
    private static bool isDead;
    private static int numOfLives;
    private static int numOfShields;
    public static int NumOfPowerUps { get => numOfLives; set => numOfLives = value; }
    public static int NumOfShields { get => numOfShields; set => numOfShields = value; }
    public static bool IsDead { get => isDead; set => isDead = value; }

    void start() 
    {
        kStartNumLives = 4;
        kStartNumShields = 2;
        isDead = false;

        // Init the num of shields and power ups
        numOfLives = kStartNumLives;
        numOfShields = kStartNumShields;  

        rb = GetComponent<Rigidbody> ();
    }
    // Update is called once per frame
    void Update()
    {
        getKeyBoardInput();
        
    }

    void getKeyBoardInput() {
        // Player controls
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x+5, this.transform.position.y, this.transform.position.z);
            playerRenderer.sprite = playerSprite[1];
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x-5, this.transform.position.y, this.transform.position.z);
            playerRenderer.sprite = playerSprite[0];
        }
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+5, this.transform.position.z);
        }
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y-5, this.transform.position.z);
        }
    }

    void OnTriggerEnter (Collider other) 
    {
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
            numOfLives++;
            Destroy(other.gameObject);
        }  

        if (other.gameObject.CompareTag("Brick Wall"))
        {
            isDead = true;
            playerRenderer.sprite = playerSprite[0];
        }
    }
}
