using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    private AudioListener listener;
    private AudioSource source;
    private bool isPlay = false;

    private float distance = 0;
    private float tempdistance = 0;
    private  AudioMixer mixer;
    public float gain=8f;//空间音效的音量增益
    public bool isLogartiehmic=false;//是否用对数衰减
    // Start is called before the first frame update
    void Start()
    {      
        listener =  SvrManager.Instance.head.gameObject.GetComponent<AudioListener>();
        source = GetComponent<AudioSource>();
        if (source.spatialize)
        {
            mixer = source.outputAudioMixerGroup.audioMixer;
            mixer.SetFloat("gain", gain);
        }
    }
    // Update is called once per frame
    void Update()
    {
 
        if (isPlay && !isLogartiehmic)
        {
            distance = Vector3.Distance(listener.transform.position, source.transform.position);

            if (distance <= 1)
            {
                source.volume = 1;
            }
            else if (distance > 1 && distance <= 100)
            {
                 source.volume = 1 / (distance * distance);
            }
            else if (distance > 100)
            {
                source.volume = 0;
            }
          
        }

        
    }
    public void ClickBtn()
    {
        if (isPlay)
        {
            isPlay = false;
            source.Pause();

        }
        else
        {            
            isPlay = true;                       
            source.Play();
        }
    }

    private void OnDestroy()
    {
        listener = null;
        mixer = null;
        source.enabled = false;
        source = null;
    }

}
