
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    GameObject YesSound, NoSound;


    [Header("----------Audio Source----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------Audio Clip----------")]
    public AudioClip background;
    public AudioClip Diamond;
    public AudioClip ClickButton;
    public AudioClip wallTouch;
    public AudioClip GameOver;

    private bool isMuted = false;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();



        // Load the mute state
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        AudioListener.pause = isMuted;
        SFXSource.mute = isMuted;
        musicSource.mute = isMuted;

        // Update the button states
        if (isMuted)
        {
            NoSound.SetActive(true);
            YesSound.SetActive(false);
        }
        else
        {
            NoSound.SetActive(false);
            YesSound.SetActive(true);
        }

        musicSource.clip = background;
        musicSource.Play();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted;
        SFXSource.mute = isMuted;
        musicSource.mute = isMuted;

        if (isMuted)
        {
            NoSound.SetActive(true);
            YesSound.SetActive(false);
        }
        else
        {
            NoSound.SetActive(false);
            YesSound.SetActive(true);
        }

        // Save the mute state
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }


    public void PlaySFX(AudioClip clip)
    {
        if (!isMuted)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    public void PauseSFX()
    {
        SFXSource.Pause();
    }

    public void ResumeSFX()
    {
        SFXSource.UnPause();
    }
}
