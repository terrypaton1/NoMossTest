using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource source;
    public float volume = 0.75f;

    void Start(){
        source = GetComponent<AudioSource>();
    }

    public void SetActive(bool b){
        float adjustedVol = (b ? 1f : 0f) * volume;

        source.volume = adjustedVol;
    }
}
