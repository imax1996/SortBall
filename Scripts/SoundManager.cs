using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixerGroup mixer;
    public Toggle soundstart;

    private string soundkey = "Sound";

    void Start()
    {
        if (PlayerPrefs.HasKey(soundkey))
        {
            if (PlayerPrefs.GetString(soundkey) == "Off")
            {
                SoundOff(true);
                soundstart.isOn = true;
            }
            else
            {
                SoundOff(false);
                soundstart.isOn = false;
            }
        }
        else {
            SoundOff(false);
            soundstart.isOn = false;
            PlayerPrefs.SetString(soundkey, "On");
        }
    }

    public void SoundOff(bool enabled) {
        if (enabled)
        {
            mixer.audioMixer.SetFloat("MasterVolume", -80f);
            PlayerPrefs.SetString(soundkey, "Off");
        }
        else {
            mixer.audioMixer.SetFloat("MasterVolume", 0f);
            PlayerPrefs.SetString(soundkey, "On");
        }
    }
}
