using SC.XR.Unity.Module_Device;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowQVRCamera : MonoBehaviour
{
    public RawImage _showLeftImage;
    public RawImage _showRightImage;

    bool isPreview = false;


    int imageWidth ;
    int imageHeight ;
    bool outBUdate = true;
    uint outCurrFrameIndex = 0;
    ulong outFrameExposureNano = 0;
    byte[] outLeftFrameData;
    byte[] outRightFrameData;
    TextureFormat textureFormat = TextureFormat.Alpha8;

    void Awake() {
        Init();
    }

    private void LateUpdate()
    {
        if (isPreview)
        {
            ShowCamera();
        }
        else
        {
            _showLeftImage.texture = null;
            _showRightImage.texture = null;

        }
    }
    public void PreBtn()
    {
        isPreview = !isPreview;
        Debug.Log("LGS:是否预览：" + isPreview);
    }


    public void Init()
    {
        imageWidth = (int)Module_Device.getInstance.Current.GreyCameraResolution.x;
        imageHeight = (int)Module_Device.getInstance.Current.GreyCameraResolution.y;
        outBUdate = true;
        outCurrFrameIndex = 0;
        outFrameExposureNano = 0;
        outLeftFrameData = new byte[imageWidth * imageHeight];
        outRightFrameData = new byte[imageWidth * imageHeight];
        textureFormat = TextureFormat.Alpha8;
    }
    public void ShowCamera()
    {
        // if (!SvrManager.Instance.Initialized) return;
        if (Application.platform == RuntimePlatform.Android)
        {

            API_SVR.GetLatestQVRCameraBinocularData(ref outBUdate, ref outCurrFrameIndex, ref outFrameExposureNano, outLeftFrameData, outRightFrameData);

            Debug.Log("LGS:outBUdate=>" + outBUdate + " outCurrFrameIndex:" + outCurrFrameIndex + "  outFrameExposureNano" + outFrameExposureNano);
            if (outBUdate)
            {
                _showLeftImage.texture = GetTexture(outLeftFrameData);
                _showLeftImage.rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);

                _showRightImage.texture = GetTexture(outRightFrameData);
                _showRightImage.rectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);

            }
            else
            {
                Debug.Log("Error: Please Check Svrconfig prop: gUseQVRCamera = true");
            }
        }
    }

    public Texture2D GetTexture(byte[] outFrameData)
    {
        Texture2D textureTemp = new Texture2D(imageWidth, imageHeight, textureFormat, false);
        textureTemp.LoadRawTextureData(outFrameData);
        textureTemp.Apply();
        return textureTemp;
    }

}
