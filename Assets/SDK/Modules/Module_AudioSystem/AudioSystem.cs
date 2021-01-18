using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour {

    private static AudioSystem instance;
    public static AudioSystem getInstance {
        get {
            if(instance == null) {
                instance = new GameObject("AudioSystem").AddComponent<AudioSystem>();
            }
            return instance;
        }
    }
    private SCAudiosConfig mSCAudiosConfig;
    public SCAudiosConfig SCAudiosConfig {
        get {
            if(mSCAudiosConfig == null) {
                mSCAudiosConfig = Resources.Load<SCAudiosConfig>("Configs/SCAudiosConfig");
            }
            return mSCAudiosConfig;
        }
    }

    public void PlayAudioOneShot(GameObject target, SCAudiosConfig.AudioType audioType, float volumeScale = 1f) {
        if(audioType ==  SCAudiosConfig.AudioType.Null) {
            //Debug.Log("audioType Null");
            return;
        }

        if(SCAudiosConfig == null) {
            Debug.Log("SCAudiosConfig Null");
            return;
        }

        if(target) {

            AudioClip impact = null;
            foreach(var audio in SCAudiosConfig.SCAudioList) {
                if(audio.audioType == audioType) {
                    impact = audio.audioClip;

                    volumeScale = audio.volume * volumeScale;

                    break;
                }
            }

            AudioSource audioS = target.GetComponent<AudioSource>();
            if(audioS == null) {
                audioS = target.AddComponent<AudioSource>();
            }
            audioS.spatialBlend = 1f;
            audioS.minDistance = 1f;
            audioS.maxDistance = 50f;
            AudioClip impactOld = audioS.clip;
            if(impact != null) {
                audioS.PlayOneShot(impact, volumeScale);
                audioS.clip = impactOld??impact;
            }
        }

    }
}
