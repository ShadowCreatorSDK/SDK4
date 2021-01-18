using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(menuName = "SCMenu/SCAudiosConfig")]
public class SCAudiosConfig:ScriptableObject {

    public enum AudioType {
        Null,
        ButtonPress,
        ButtonUnpress,
        Manipulation_End,
        Manipulation_Start,
        Move_End,
        Move_Start,
        Notification,
        Rotate_Start,
        Rotate_Stop,
        Scale_Start,
        Scale_Stop,
        Select_Main,
        Select_Secondary,
        Shell_Click_In,
        Shell_Click_Init,
        Shell_Click_Out,
        Slate_Grab,
        Slate_Release,
        Slate_Touch,
        Slider_Pass_Notch,
        Slider_Release,
        Tap,
        Voice_Confirmation,
    }


    [Serializable]
    public class SCAudio {
        public AudioType audioType;
        public AudioClip audioClip;
        [Range(0.2f, 1)]
        public float volume = 1;
    }

    public List<SCAudio> SCAudioList;

}