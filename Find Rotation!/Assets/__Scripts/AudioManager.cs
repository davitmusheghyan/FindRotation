using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource partChange;
    [SerializeField] AudioSource background;
    public AudioSource[] button;

    public int audioStatus;
    public int vibrationStatus;

    void Start()
    {
        audioStatus = PlayerPrefs.GetInt("volumeStatus");
        vibrationStatus = PlayerPrefs.GetInt("vibrationStatus");
        if (audioStatus == 0)
            background.volume = 0.5f;
        else
            background.volume = 0;
    }
    
    public void AudioOnOff()
    {
        if(audioStatus == 0)
        {
            background.volume = 0f;
            audioStatus = 1;
            PlayerPrefs.SetInt("volumeStatus", audioStatus);
        } else
        {
            background.volume = 0.5f;
            audioStatus = 0;
            PlayerPrefs.SetInt("volumeStatus", audioStatus);
        }
    }
    public void VibrationOnOff()
    {
        if (vibrationStatus == 0)
        {
            vibrationStatus = 1;
            PlayerPrefs.SetInt("vibrationStatus", vibrationStatus);
        }
        else
        {
            vibrationStatus = 0;
            PlayerPrefs.SetInt("vibrationStatus", vibrationStatus);
        }
    }

    public void ButtonDown()
    {
        if(audioStatus == 0)
        {
            button[0].Play();
        }
    }
    public void ButtonUp()
    {
        if (audioStatus == 0)
        {
            button[1].Play();
        }
    }

    public void PartChange()
    {
        if (audioStatus == 0)
        {
            partChange.Play();
        }
    }
}
