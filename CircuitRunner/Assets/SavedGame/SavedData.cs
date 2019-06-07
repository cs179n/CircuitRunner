using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;



public class SavedData : MonoBehaviour
{
    public bool Level1Clear;
    public bool Level2Clear;
    public int numLives;
    public int numShields;
    public TextAsset textFile;     // drop your file here in inspector
    public byte[] byteText;
    private int timer;

    void Start()
    {
        timer = 0;
        string text = textFile.text;  //this is the content as string
        byteText = textFile.bytes;  //this is the content as byte array
        Level1Clear = (textFile.bytes[0] == 49);
        Level2Clear = (textFile.bytes[3] == 49);
        numLives = (textFile.bytes[6] - 48);
        numShields = (textFile.bytes[9] - 48);

    }

    // Update is called once per frame
    void Update()
    {
        if (Player.IsWinner)
        {
            timer ++;
            if (timer == 1)
            {
                Debug.Log("UPDATED FILE");
                int sceneNum = SceneManager.GetActiveScene().buildIndex;
                if (sceneNum == 1)
                {
                    byteText[0] = 49;
                }
                if (sceneNum == 2)
                {
                    byteText[3] = 49;
                }
                UpdateText(textFile);
            }
        }
    }

    void SaveGame()
    {
        numLives = Player.NumOfLives;
        numShields = Player.NumOfShields;
    }
    void UpdateText(TextAsset saved) {
        string path = "Assets/SavedGame/saved.txt";
        StreamWriter writer = new StreamWriter(path, true);
        for (int i = 0; i < byteText.Length; i++)
        {
            if (byteText[i] >= 48)
                writer.WriteLine(byteText[i]-48);
 
            //writer.WriteLine(byteText[i]);
        }
        writer.Close();
    }
}