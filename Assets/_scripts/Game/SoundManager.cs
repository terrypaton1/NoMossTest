using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Studios.Utils
{
    /*
     * A class to easily manage single pop sounds
     * This class is designed to easily play sound effects that don't require a location in space, and can be set to fire & forget.
     * This class loads sound effects by their path in a Resources folder, so make sure all sound effects you want to play can be accessed from a Resources directory
     * 
     */ 
    public class SoundManager : MonoBehaviour
    {
        //Our audio source to play sounds
        private static AudioSource _src;
        private static AudioSource src
        {
            get
            {
                if (_src == null)
                {
                    _src = inst.GetComponent<AudioSource>();
                }
                return _src;
            }
        }

        //Our sound manager instance
        private static SoundManager _inst;
        private static SoundManager inst
        {
            get
            {
                if (_inst == null)
                {
                    GameObject g = new GameObject();
                    DontDestroyOnLoad(g);
                    g.name = "Sound Manager";
                    _inst = g.AddComponent<SoundManager>();
                    _src = g.AddComponent<AudioSource>();
                }

                return _inst;
            }
        }

        //Our dictionary of loaded sounds
        private static Dictionary<string, AudioClip> _clips;
        private static Dictionary<string, AudioClip> clips
        {
            get
            {
                if(_clips == null)
                {
                    _clips = new Dictionary<string, AudioClip>();
                }
                return _clips;
            }
        }

        //Our base level of sound effect distortion
        public static float basePitch = 1f;
        //The maximum amount of random distortion applied to sounds
        public static float pitchDistortionModifier = 0.2f;

        //The volume of the sound being played
        public static float volume = 1f;

        //Play a sound effect
        //The path for the sound effect is its path in Resources folder
        public static void PlayClip(string soundPath, float volumeMod = 1f, float distortionScale = 1f)
        {
            //Assign the pitch to the sound
            AudioClip clip;
            src.pitch = basePitch + (distortionScale * Random.Range(-1f * pitchDistortionModifier, pitchDistortionModifier));
            //If the clip already exists in the dictionary
            if (clips.TryGetValue(soundPath, out clip))
            {
                //Play the clip
                src.PlayOneShot(clip, volumeMod * volume);
            }
            else
            {
                //Otherwise, load the clip and add it to the dictionary
                clip = Resources.Load<AudioClip>(soundPath);
                if (clip == null)
                {
                    Debug.LogError("Could not find Audio Clip: " + soundPath);
                }
                else
                {
                    clips.Add(soundPath, clip);
                    src.PlayOneShot(clip, volumeMod * volume);
                }
            }
        }

        //Play a sound effect
        //Doesn't load it from resources
		public static void PlayClip(AudioClip clip, float volumeMod = 1f, float distortionScale = 1f)
        {
			src.pitch = basePitch + (distortionScale * Random.Range(-1f * pitchDistortionModifier, pitchDistortionModifier));
			src.PlayOneShot(clip, volumeMod * volume);
        }
    }

}