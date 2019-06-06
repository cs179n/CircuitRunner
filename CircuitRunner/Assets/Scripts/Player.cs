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
    private static bool isWon;
    public static bool IsDead { get => isGameOver; set => isGameOver = value; }
    public static bool IsWinner { get => isWon; set => isWon = value; }
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

        if (other.gameObject.CompareTag("Battery Pick Up"))
        {
            if (playerLives < 10)
            {
                playerLives++;
            }
            Destroy(other.gameObject);
        }  

        if (other.gameObject.CompareTag("Brick Wall") || other.gameObject.CompareTag("LogicGate"))
        {
            this.loseLife();
            other.gameObject.GetComponent<AudioSource>().Play();
            this.GetComponent<Movement>().flipVelocity();
        }
        if (other.gameObject.CompareTag("Win"))
        {
            playerLives = 69;
            isWon = true;
        }
        if (other.gameObject.CompareTag("Mite"))
        {
            this.loseLife();
            Vector3 velocity = this.GetComponent<Movement>().getVelocity();
            other.gameObject.GetComponent<Enemy>().knockOut(velocity);
            other.gameObject.GetComponent<AudioSource>().Play();
            //Vector3 target = other.transform.position + velocity;
            //float step = 1f * Time.deltaTime;
            //other.transform.position = Vector3.MoveTowards(other.transform.position, target, step);
        }
    }

    void loseLife() {
        if (playerLives > 1) {
            playerLives--;
        } else {
            isGameOver = true;
        }
    }
}
