using Audio;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioClip sceneTheme;
    public bool playOnStart = true;
    public float optVolume = 1f;
    
    private void Start()
    {
        if (playOnStart)
            AudioManager.instance.PlayMusic(sceneTheme, 2f, optVolume);
    }

    public void PlayMusic()
    {
        AudioManager.instance.PlayMusic(sceneTheme, 2f, optVolume);
    }
}
