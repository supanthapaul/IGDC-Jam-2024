using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioClip sceneTheme;
    
    private void Start()
    {
        AudioManager.instance.PlayMusic(sceneTheme, 2f);
    }
}
