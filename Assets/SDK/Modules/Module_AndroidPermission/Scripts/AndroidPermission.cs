using SC.XR.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class AndroidPermission : AndroidPluginBase
{
    private static AndroidPermission Instant;
    public static AndroidPermission getInstant {
        get {
            if(Instant == null) {
                Instant = new AndroidPermission();
            }
            return Instant;
        }
    }
    private AndroidJavaClass AndroidPermissionClass { get; set; }
    private AndroidJavaObject AndroidPermissionObject { get; set; }
    private AndroidPermission() {
        AndroidPermissionClass = GetAndroidJavaClass("com.example.libpermission.PermissionRequest");
        AndroidPermissionObject = ClassFunctionCallStatic<AndroidJavaObject>(AndroidPermissionClass, "getInstant", CurrentActivity);

    }

    void RequestPermission(string[] permissionList) {
      ObjectFunctionCall2(AndroidPermissionObject, "RequestPermission", permissionList);
    }

    string[] permissionArray;
    public void GetPermission(bool Camera = true, bool ExternalStorageRead = true, bool ExternalStorageWrite = true, bool Microphone = false, bool FineLocation = false, bool CoarseLocation = false) {

        List<string> permissionList = new List<string>();

        if(Camera) {
            permissionList.Add(Permission.Camera);
            //Debug.Log("PermissionRequest :Camera");
        }
        if(ExternalStorageRead) {
            permissionList.Add(Permission.ExternalStorageRead);
            //Debug.Log("PermissionRequest :ExternalStorageRead");
        }
        if(ExternalStorageWrite) {
            permissionList.Add(Permission.ExternalStorageWrite);
            //Debug.Log("PermissionRequest :ExternalStorageWrite");
        }
        if(Microphone) {
            permissionList.Add(Permission.Microphone);
            //Debug.Log("PermissionRequest :Microphone");
        }
        if(FineLocation) {
            permissionList.Add(Permission.FineLocation);
            //Debug.Log("PermissionRequest :FineLocation");
        }
        if(CoarseLocation) {
            permissionList.Add(Permission.CoarseLocation);
            //Debug.Log("PermissionRequest :CoarseLocation");
        }

        permissionArray = permissionList.ToArray();

        if(permissionArray.Length > 0) {
            RequestPermission(permissionArray);
        }
    }
    public void GetPermission(List<string> permissionList) {

        permissionArray = permissionList.ToArray();

        if(permissionArray.Length > 0) {
            RequestPermission(permissionArray);
        }
    }
}
