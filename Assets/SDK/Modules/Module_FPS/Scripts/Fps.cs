using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fps : MonoBehaviour {

    int FrameCount = 0;
    private float _framesPerSecond;
    private TextMesh textMesh;
    // Use this for initialization
    void Start () {
        textMesh = GetComponent<TextMesh>();
        if(textMesh) {
            StartCoroutine(CalculateFramesPerSecond());
        }
    }
    // Update is called once per frame
    void Update () {
        FrameCount++;
        textMesh.text = string.Format("{0:F2}", _framesPerSecond);
    }

    private IEnumerator CalculateFramesPerSecond() {
        int lastFrameCount = 0;

        while (true) {
            yield return new WaitForSecondsRealtime(1f);
            
            var elapsedFrames = FrameCount - lastFrameCount;
            _framesPerSecond = elapsedFrames / 1f;
            
            lastFrameCount = FrameCount;
        }
    }

}
