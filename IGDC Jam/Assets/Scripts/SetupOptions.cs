using UnityEngine;
using UnityEngine.UI;
using static PlayerPrefStatics;
public class SetupOptions : MonoBehaviour
{
    [SerializeField] Slider musicVolume, sfxVolume, narratorVolume, mouseSens;
    void Start()
    {
        musicVolume.value = PlayerPrefs.GetFloat(MusicVolume, 0.15f);
        sfxVolume.value = PlayerPrefs.GetFloat(SFXVolume, 0.5f);
        narratorVolume.value = PlayerPrefs.GetFloat(NarratorVolume, 1f);
        mouseSens.value = PlayerPrefs.GetFloat(MouseSens, 0.5f);
    }

    public void AfterValueUpdate()
    {
        PlayerPrefs.SetFloat(MusicVolume, musicVolume.value);
        PlayerPrefs.SetFloat(SFXVolume, sfxVolume.value);
        PlayerPrefs.SetFloat(NarratorVolume, narratorVolume.value);
        PlayerPrefs.SetFloat(MouseSens, mouseSens.value);
        AudioManager.instance.SetVolume(musicVolume.value, AudioManager.AudioChannel.Music);
        AudioManager.instance.SetVolume(sfxVolume.value, AudioManager.AudioChannel.Sfx);
        AudioManager.instance.SetVolume(narratorVolume.value, AudioManager.AudioChannel.Narrator);
    }
}
