using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module_RGBCamera : MonoBehaviour
{
    Vector3 offset = new Vector3(0.03f, -0.01f, 0);
    [HideInInspector]
    public WebCamTexture webTex;
    [HideInInspector]
    public string deviceName;
    public static Module_RGBCamera Instance;
    private static MeshRenderer mat;
    float _width;
    float _height;

    void Start()
    {
        if (Instance == null)
        {
           
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Instance = this;
            this.gameObject.transform.localPosition = offset;
            _width = Screen.width / 2.0f;
            _height = Screen.height;
            this.gameObject.transform.localScale = new Vector3(1, _height / _width, 1);
            mat = this.gameObject.GetComponent<MeshRenderer>();

            if (API_Module_Device.CurrentAndroid.HasRGB)
            {
                StartCoroutine(CallCamera());
                
            }
            else
            {
                Debug.Log("LGS:This device does not support the RGB");
            }
        }
    }

    public void StartCamera()
    {

        if (API_Module_Device.CurrentAndroid.HasRGB)
        {
            StartCoroutine(CallCamera());
        }
    }
   
    
    IEnumerator CallCamera()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            deviceName = devices[0].name;
            if (webTex != null)
            {
                webTex.Stop();
                webTex = null;
            }
            //Set RGBCamera rect   
            webTex = new WebCamTexture(deviceName, Screen.width / 2, Screen.height, 30);//
            mat.material.mainTexture = webTex;

            webTex.Play();//Start
        }
    }

    public void StopCamera()
    {
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            mat.material.mainTexture = null;
            webTex.Stop();
            webTex = null;
            StopCoroutine(CallCamera());
        }
    }
}
