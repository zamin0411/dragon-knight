using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    //Singleton object
    public static SoundManager instance { get; private set; }
    private AudioSource source;
  

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        //Keep the sound even when new level loads  
        if (instance == null)
        {
            instance = this;
            //if (scene.name.Contains("Level"))
            //    DontDestroyOnLoad(gameObject);
        }
        //If we have a duplicate sound object, destroy it
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip _sound)
    {
        //Play only once
        source.PlayOneShot(_sound);
    }

    // Update is called once per frame
    void Update()
    {

    }
 

}
