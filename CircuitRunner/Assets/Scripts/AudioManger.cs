using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public AudioSource m_MyAudioSource;

    //Play the music
    bool m_Play;
    //Detect when you use the toggle, ensures music isn’t played multiple times
    bool pausedChanged = false, gameOverChanged = false;

    void Start()
    {
        //Ensure the toggle is set to true for the music to play at start-up
        m_Play = true;
    }

    void Update()
    {
        //Get the Player pause status
        if (PauseMenuController.IsGamePause != pausedChanged) 
        {
            pausedChanged = PauseMenuController.IsGamePause;
        } 

        //Get the Player Lives status
        if (Player.IsDead != gameOverChanged)
        {
            gameOverChanged = Player.IsDead;
        }

        //Check to see if you just set the toggle to positive
        if (m_Play && (pausedChanged || gameOverChanged))
        {
            //Play the audio you attach to the AudioSource component
            m_MyAudioSource.Play();
            //Ensure audio doesn’t play more than once
            pausedChanged = false;
            gameOverChanged = false;
        }
        //Check if you just set the toggle to false
        if (!m_Play && (pausedChanged || gameOverChanged))
        {
            //Stop the audio
            m_MyAudioSource.Stop();
            //Ensure audio doesn’t play more than once
            pausedChanged = false;
            gameOverChanged = false;
        }
    }
}
