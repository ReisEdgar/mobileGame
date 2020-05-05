using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    public static Options Instance;
    public AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effects", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("music", volume);
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }
}
