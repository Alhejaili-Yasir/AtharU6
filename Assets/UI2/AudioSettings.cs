using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField]private AudioMixer Mixer;
    public AudioMixer mixer;

    public void SetMusicVolume(float sildervolume)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sildervolume) * 20);
    }

    public void SetCharacterVolume(float volume)
    {
        mixer.SetFloat("CharacterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }
}
