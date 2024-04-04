using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationData : MonoBehaviour
{
   // public static NarrationData instance;
    public List<AudioClip> narrationClips = new List<AudioClip>();
    public AudioSource soundEffect;

    private void Awake()
    {
        //if (instance != null) Destroy(instance);
        //instance = this;
    }
    void Start()
    {
        
    }
    
    public AudioClip GetNarrByIndex(int index)
    {

        if (index < narrationClips.Count)
        {
            return narrationClips[index];
        }
        else
        {
            return null;
        }
    }

    public void SoundEffectPlay(AudioClip clip,bool play)
    {
        if(soundEffect)
        {
            if(play)
            {
                soundEffect.clip = clip;
                soundEffect.Play();
            }
            else
            {
                soundEffect.Stop();
            }
       
        }
    }


}
