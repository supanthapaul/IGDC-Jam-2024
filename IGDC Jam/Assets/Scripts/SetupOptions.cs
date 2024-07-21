using UnityEngine;
using UnityEngine.UI;
using static PlayerPrefStatics;
public class SetupOptions : MonoBehaviour
{
    [SerializeField] Slider musicVolume, sfxVolume, narratorVolume, mouseSens;
    void Start()
    {
        musicVolume.value = PlayerPrefs.GetFloat(MusicVolume, 0.7f);
        sfxVolume.value = PlayerPrefs.GetFloat(SFXVolume, 0.5f);
        narratorVolume.value = PlayerPrefs.GetFloat(NarratorVolume, 0.9f);
        mouseSens.value = PlayerPrefs.GetFloat(MouseSens, 0.5f);
    }

    public void AfterValueUpdate()
    {
        PlayerPrefs.SetFloat(MusicVolume, musicVolume.value);
        PlayerPrefs.SetFloat(SFXVolume, sfxVolume.value);
        PlayerPrefs.SetFloat(NarratorVolume, narratorVolume.value);
        PlayerPrefs.SetFloat(MouseSens, mouseSens.value);
    }
}
