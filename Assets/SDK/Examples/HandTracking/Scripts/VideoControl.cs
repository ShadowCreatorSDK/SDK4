using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
public class VideoControl : MonoBehaviour {
    public GameObject Icon;
    public Material material;
    bool? isStart = true;
    public VideoPlayer videoPlayer;

    [Serializable]
    public class VideoInfo {
        public VideoClip video;
        public int index;
    }

    public List<VideoInfo> videoList;

    public void VideoStartSwitch() {
        isStart = videoPlayer.isPlaying;
        isStart = !isStart;
        Icon.SetActive(!isStart.Value);

        if(isStart.Value) {
            videoPlayer?.Play();
        } else {
            videoPlayer?.Pause();
        }
    }



    public void PlayVideo(int index) {
        if(videoPlayer == null)
            return;

        foreach(var item in videoList) {
            if(item.index == index) {
                videoPlayer.clip = item.video;
                videoPlayer?.Play();
                Icon.SetActive(false);
                break;
            }
        }
    }

}
