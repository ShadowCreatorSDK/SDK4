using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

class SvrPluginAndroidOther : SvrPlugin
{
	public static SvrPluginAndroidOther Create()
	{
		return new SvrPluginAndroidOther();
	}

    private SvrPluginAndroidOther() { }

    public override bool IsInitialized() { return svrCamera != null; }

    public override bool IsRunning() { return eyes != null; }

    public override IEnumerator Initialize()
	{
        yield return base.Initialize();

        deviceInfo = GetDeviceInfo();

        yield break;
	}

	public override IEnumerator BeginVr(int cpuPerfLevel, int gpuPerfLevel)
	{
        yield return base.BeginVr(cpuPerfLevel, gpuPerfLevel);

		yield break;
    }
	
    public override void SetVSyncCount(int vSyncCount)
    {
        QualitySettings.vSyncCount = vSyncCount;
    }

    Vector2 mouseNDCRotate = Vector2.zero;
    Vector2 mouseNDCPosition = Vector2.zero;

    Vector2 mousePressPointTemp1 = Vector2.zero;
    Vector3 mousePressEuler = Vector3.zero;


    public Vector3 GetPosition {
        get {
            if (Input.touchCount >= 1) {
                return Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            } else if (Input.mousePresent) {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction, Color.white);
                return Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            return Vector3.zero;
        }
    }

    Ray ray;

    public Quaternion GetRotation {
        get {
            if (Input.touchCount >= 1) {
                ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                return Quaternion.LookRotation(ray.direction, Camera.main.transform.up);
            } else if (Input.mousePresent) {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
                return Quaternion.LookRotation(ray.direction, Camera.main.transform.up);
            }
            return Quaternion.identity;
        }
    }


    public override int GetHeadPose(ref HeadPose headPose, int frameIndex)
	{
        int poseStatus = 0;
        headPose.orientation = GetRotation;
        headPose.position = GetPosition;
        
        poseStatus |= (int)TrackingMode.kTrackingOrientation;
        poseStatus |= (int)TrackingMode.kTrackingPosition;

        //Debug.Log("Input.mousePosition:"+ Input.mousePosition+"  "+ Screen.width+" "+Screen.height);

        //if (Input.GetMouseButton(0))    // 0/Left mouse button
        //{
        //    poseStatus |= (int)TrackingMode.kTrackingOrientation;
        //    poseStatus |= (int)TrackingMode.kTrackingPosition;
        //}

        //if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))    // 1/Right mouse button
        //{
        //    mousePressPointTemp1 = Input.mousePosition;
        //    mousePressEuler = SvrManager.Instance.head.eulerAngles;

        //} else if(Input.GetMouseButton(0) || Input.GetMouseButton(1)) {

        //    mouseNDCRotate.x = 2 * ((Input.mousePosition.x - mousePressPointTemp1.x) / Screen.width) ;
        //    mouseNDCRotate.y = 2 * ((Input.mousePosition.y - mousePressPointTemp1.y) / Screen.height) ;
        //    poseStatus |= (int)TrackingMode.kTrackingOrientation;

        //}

        //if(Input.GetKey(KeyCode.W)) {

        //    mouseNDCPosition.y += Time.deltaTime * 0.2f;
        //    poseStatus |= (int)TrackingMode.kTrackingPosition;
        //} else if(Input.GetKey(KeyCode.S)) {
        //    mouseNDCPosition.y -= Time.deltaTime * 0.2f;
        //    poseStatus |= (int)TrackingMode.kTrackingPosition;
        //} else {
        //    mouseNDCPosition.y = 0;
        //}

        //if(Input.GetKey(KeyCode.A)) {
        //    mouseNDCPosition.x -= Time.deltaTime * 0.2f;
        //    poseStatus |= (int)TrackingMode.kTrackingPosition;
        //} else if(Input.GetKey(KeyCode.D)) {
        //    mouseNDCPosition.x += Time.deltaTime * 0.2f;
        //    poseStatus |= (int)TrackingMode.kTrackingPosition;
        //} else {
        //    mouseNDCPosition.x = 0;
        //}
        ////if(Input.mouseScrollDelta.y != 0) {
        ////    mouseNDCPosition.x = 0;
        ////    mouseNDCPosition.y = Input.mouseScrollDelta.y * 0.2f;
        ////    poseStatus |= (int)TrackingMode.kTrackingPosition;
        ////} else {
        ////    mouseNDCPosition = Vector2.zero;
        ////}

        /////复位
        //if(Input.GetKey(KeyCode.Escape) == true) {

        //    mouseNDCRotate = Vector2.zero;
        //    mouseNDCPosition = Vector2.zero;

        //    mousePressPointTemp1 = Vector2.zero;
        //    mousePressEuler = Vector3.zero;

        //    SvrManager.Instance.head.position = Vector3.zero;

        //    poseStatus |= (int)TrackingMode.kTrackingOrientation;
        //    poseStatus |= (int)TrackingMode.kTrackingPosition;
        //}


        //headPose.orientation.eulerAngles = mousePressEuler + new Vector3(-mouseNDCRotate.y * 45f, mouseNDCRotate.x * 90f, 0);

        //headPose.position = new Vector3(mouseNDCPosition.x,0, mouseNDCPosition.y);
        //headPose.position = SvrManager.Instance.head.TransformPoint(headPose.position);

        return poseStatus;
    }

	public override DeviceInfo GetDeviceInfo()
	{
		DeviceInfo info 			= new DeviceInfo();

		info.displayWidthPixels 	= Screen.width;
		info.displayHeightPixels 	= Screen.height;
		info.displayRefreshRateHz 	= 60.0f;
		info.targetEyeWidthPixels 	= Screen.width / 2;
		info.targetEyeHeightPixels 	= Screen.height;
		info.targetFovXRad			= Mathf.Deg2Rad * 47;
		info.targetFovYRad			= Mathf.Deg2Rad * 20.1f;
		info.targetFrustumLeft.left 	= -0.02208847f;
		info.targetFrustumLeft.right    = 0.02208847f;
		info.targetFrustumLeft.top      = 0.0123837f;
		info.targetFrustumLeft.bottom   = -0.0123837f;
        info.targetFrustumLeft.near     = 0.0508f;
        info.targetFrustumLeft.far      = 100f;
		info.targetFrustumRight.left    = -0.02208847f;
		info.targetFrustumRight.right   = 0.02208847f;
		info.targetFrustumRight.top     = 0.0123837f;
		info.targetFrustumRight.bottom  = -0.0123837f;
        info.targetFrustumRight.near    = 0.0508f;
        info.targetFrustumRight.far     = 100f;
        return info;
	}

	public override void SubmitFrame(int frameIndex, float fieldOfView, int frameType)
	{
	}


    public override int HandShank_Getbond(int lr) { return 17; }

    public override void Shutdown()
	{
        base.Shutdown();
    }
}
