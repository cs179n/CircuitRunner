using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] playerSprite;

    public SpriteRenderer playerRenderer;

    public Rigidbody rb;

    //#########################################################################################
    private static AudioSource level1Music;
    public static AudioSource Level1Music { get => level1Music; set => level1Music = value; }
    //#########################################################################################

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
        if (other.gameObject.CompareTag("Rail"))
        {
             
        }
    }
}
