using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedData : MonoBehaviour
{
    public bool Level1Clear;
    public bool Level2Clear;
    public int numLives;
    public int numShields;
    public TextAsset textFile;     // drop your file here in inspector
    public byte[] byteText;

    void Start()
    {
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

    }

    void SaveGame()
    {
        numLives = Player.NumOfLives;
        numShields = Player.NumOfShields;
    }
}