using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SC.XR.Unity;

public class VuforiaLocation : MonoBehaviour {
    public Vector3 RGBPositionOffset = new Vector3(0,0f,0.05f);
    public Vector3 RGBRotationOffset;
    // Use this for initialization
    void Start() {
        StartCoroutine(InitARCamera());
   
    }

    IEnumerator InitARCamera() {
        while(true) {
            if(SvrManager.Instance.IsRunning) {
                transform.SetParent(SvrManager.Instance.head, false);

                if (API_Module_Device.Current != null) {
                    transform.localEulerAngles = -API_Module_Device.Current.RGBRotationOffset + RGBRotationOffset;
                    transform.localPosition = -API_Module_Device.Current.RGBPositionOffset + RGBPositionOffset;
                } else {
                    transform.localEulerAngles = RGBRotationOffset;
                    transform.localPosition = RGBPositionOffset;
                }

                yield break;
            }
            yield return null;
        }
    }
    // Update is called once per frame
    void Update() {
        //gameObject.transform.position = SvrManager.Instance.modifyPosition;
        //gameObject.transform.rotation = SvrManager.Instance.modifyOrientation;
    }
}
