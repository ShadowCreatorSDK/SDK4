using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VideoSlider : PointerHandler
{

    public VideoPlayer vidoPlayer;
    public Slider slider;
    bool isDown = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(slider && vidoPlayer && isDown==false) {
            slider.value = float.Parse(vidoPlayer.frame.ToString()) / float.Parse(vidoPlayer.frameCount.ToString());
        }
    }

    void sliderChanged(float per) {
        if(slider && vidoPlayer) {
            float targetFrame = vidoPlayer.frameCount * per;
            vidoPlayer.frame = (long) targetFrame;
        }
    }

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        isDown = true;
    }

    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        sliderChanged(slider.value);
        isDown = false;
    }
}
