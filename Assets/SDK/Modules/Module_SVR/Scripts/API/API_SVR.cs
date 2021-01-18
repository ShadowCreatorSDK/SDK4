using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class API_SVR {

    public enum TrackMode {
        Mode_3Dof,
        Mode_6Dof,
    }

    ///API-No.1
    /// <summary>
    /// 设置眼镜进入模式,运行过程中可修改
    /// </summary>
    public static void SetTrackMode(TrackMode mode) {
        if(SvrManager.Instance != null) {
            SvrManager.Instance.settings.trackPosition = (mode == TrackMode.Mode_6Dof ? true : false);
        }
    }

    ///API-No.2
    /// <summary>
    /// Svr系统是否在运行
    /// </summary>
    /// <returns>true表示在运行，false表示未运行（Pause时为false）</returns>
    public static bool IsSvrRunning() {
        if(SvrManager.Instance != null) {
            return SvrManager.Instance.status.running;
        }
        return false;
    }

    ///API-No.3
    /// <summary>
    /// Svr系统是否初始化完成
    /// </summary>
    /// <returns></returns>
    public static bool IsSvrInitialized() {
        if(SvrManager.Instance != null) {
            return SvrManager.Instance.status.initialized;
        }
        return false;
    }

    ///API-No.4
    /// <summary>
    /// 设置Svr初始化完成时的回调
    /// </summary>
    /// <param name="action"></param>
    public static void AddInitializedCallBack(Action action) {
        SvrManager.Instance.SvrInitializedCallBack += action;
    }

    ///API-No.5
    public static void RemoveInitializedCallBack(Action action) {
        SvrManager.Instance.SvrInitializedCallBack -= action;
    }

    ///API-No.6
    /// <summary>
    /// 设置渲染帧率,只能在Start中调用
    /// </summary>
    /// <param name="frameRate">默认-1表示系统默认帧率,设置范围0-200</param>
    public static void SetRenderFrame(int frameRate = -1) {
        if(SvrManager.Instance != null) {
            if(frameRate == -1) {
                SvrManager.Instance.plugin.SetVSyncCount((int)(SvrManager.Instance.settings.vSyncCount = SvrManager.SvrSettings.eVSyncCount.k1));
                QualitySettings.vSyncCount = (int)(SvrManager.Instance.settings.vSyncCount = SvrManager.SvrSettings.eVSyncCount.k1);//Vsync
            } else {
                SvrManager.Instance.plugin.SetVSyncCount((int)(SvrManager.Instance.settings.vSyncCount = SvrManager.SvrSettings.eVSyncCount.k0));
                QualitySettings.vSyncCount = (int)(SvrManager.Instance.settings.vSyncCount = SvrManager.SvrSettings.eVSyncCount.k0);//Don't sync
                Application.targetFrameRate = (frameRate >= 0 && frameRate < 200) ? frameRate : 75;
            }
        }
    }

    ///API-No.7
    /// <summary>
    /// 获取左右眼摄像头
    /// </summary>
    /// <returns>List[0]左眼 List[1]右眼，空表示系统未启动完成</returns>
    public static List<Camera> GetEyeCameras() {
        List<Camera> cameraList = new List<Camera>(2);
        if(SvrManager.Instance != null && SvrManager.Instance.status.running == true) {
            cameraList.Add(SvrManager.Instance.leftCamera);
            cameraList.Add(SvrManager.Instance.rightCamera);
        }
        return cameraList;
    }

    ///API-No.8
    /// <summary>
    /// 获取左右眼渲染的画面，为获取当前帧的渲染结果，当前帧结束时调用
    /// </summary>
    /// <returns>List[0]左眼 List[1]右眼，空表示系统未启动完成</returns>
    public static List<RenderTexture> GetRenderTexure() {
        List<Camera> cameraList = GetEyeCameras();
        List<RenderTexture> RTList = new List<RenderTexture>(2);
        foreach(var item in cameraList) {
            RTList.Add(item.targetTexture);
        }
        return RTList;
    }

    ///API-No.9
    /// <summary>
    /// 获取头部物体，如果想获取头部的旋转移动等数据，在LateUpdate方法里调用
    /// </summary>
    /// <returns>空表示系统未启动完成</returns>
    public static Transform GetHead() {
        if(SvrManager.Instance != null && SvrManager.Instance.status.running == true) {
            return SvrManager.Instance.head;
        }
        return null;
    }

    ///API-No.10
    /// <summary>
    /// 设置瞳距，Awake时调用，Start后调用无效
    /// </summary>
    /// <param name="offset">瞳距的偏移量，单位米</param>
    public static void SetPD(float offset = 0) {
        if(SvrManager.Instance != null) {
            //SvrManager.Instance.leftCameraOffsetPostion += offset / 2 * Vector3.left;
            //SvrManager.Instance.rightCameraOffsetPostion += offset / 2 * Vector3.right;
        }
    }

    ///API-No.11
    /// <summary>
    /// 重定位,若无效果，表示系统初始化未完成,且只有在眼镜上有效
    /// </summary>
    public static void RecenterTracking() {
        if(SvrManager.Instance != null) {
            SvrManager.Instance.RecenterTracking();
        }
    }

    ///API-No.12
    /// <summary>
    /// StartSlam
    /// </summary>
    public static void StartSlam() {
        if (SvrManager.Instance != null) {
            SvrManager.Instance.StartSlam();
        }
    }

    ///API-No.13
    /// <summary>
    /// StopSlam
    /// When a StartSlam is running (not completed), calling StopSlam will not work
    /// </summary>
    public static void StopSlam() {
        if (SvrManager.Instance != null) {
            SvrManager.Instance.StopSlam();
        }
    }

    ///API-No.14
    /// <summary>
    /// ResetSlam
    /// </summary>
    public static void ResetSlam() {
        if (SvrManager.Instance != null) {
            SvrManager.Instance.ResetSlam();
        }
    }

    ///API-No.15
    /// <summary>
    /// IS Slam 6Dof DataLost
    /// </summary>
    public static bool IsSlamDataLost {
        get {
            if (SvrManager.Instance != null) {
                return SvrManager.Instance.IsTrackingValid;
            }
            return true;
        }
    }

    ///API-No.16
    /// <summary>
    /// Get QvrCamera Data
    /// </summary>
    public static int GetLatestQVRCameraBinocularData(ref bool outBUdate, ref uint outCurrFrameIndex, ref ulong outFrameExposureNano, byte[] outLeftFrameData, byte[] outRightFrameData) {
        if (SvrManager.Instance && SvrManager.Instance.plugin != null) {
            AndroidPermission.getInstant.GetPermission(true,false,false);
            return SvrManager.Instance.plugin.SVRGetLatestQVRCameraBinocularData(ref outBUdate, ref outCurrFrameIndex, ref outFrameExposureNano, outLeftFrameData, outRightFrameData);
        }
        return 0;
    }
}
