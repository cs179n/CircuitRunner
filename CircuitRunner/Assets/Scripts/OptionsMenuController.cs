using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class OptionsMenuController : MonoBehaviour
{
    public AudioMixer AudioMixer; 
    public void SetVolume(float volume) {
        AudioMixer.SetFloat("volume", volume);
    }
}
