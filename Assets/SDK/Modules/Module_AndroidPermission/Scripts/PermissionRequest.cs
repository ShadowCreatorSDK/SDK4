using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PermissionRequest : MonoBehaviour
{

    public bool Camera = true;
    public bool ExternalStorageRead = true;
    public bool ExternalStorageWrite = true;
    public bool Microphone = false;
    public bool FineLocation = false;
    public bool CoarseLocation = false;

    void Awake() {
        List<string> permissionList = new List<string>();

        if(Camera && !Permission.HasUserAuthorizedPermission(Permission.Camera)) {
            permissionList.Add(Permission.Camera);
        }
        if(ExternalStorageRead && !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead)) {
            permissionList.Add(Permission.ExternalStorageRead);
        }
        if(ExternalStorageWrite && !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite)) {
            permissionList.Add(Permission.ExternalStorageWrite);
        }
        if(Microphone && !Permission.HasUserAuthorizedPermission(Permission.Microphone)) {
            permissionList.Add(Permission.Microphone);
        }
        if(FineLocation && !Permission.HasUserAuthorizedPermission(Permission.FineLocation)) {
            permissionList.Add(Permission.FineLocation);
        }
        if(CoarseLocation && !Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)) {
            permissionList.Add(Permission.CoarseLocation);
        }

        if(permissionList.Count > 0) {
            AndroidPermission.getInstant.GetPermission(permissionList);
        }
    }



}
