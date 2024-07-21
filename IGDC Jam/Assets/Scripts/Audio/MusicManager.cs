using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioClip sceneTheme;
    public bool playOnStart = true;
    
    private void Start()
    {
        if (playOnStart)
            AudioManager.instance.PlayMusic(sceneTheme, 2f);
    }

    public void PlayMusic()
    {
        AudioManager.instance.PlayMusic(sceneTheme, 2f);
    }
}
