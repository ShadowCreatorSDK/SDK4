using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using SC.XR.Unity;
using SC.XR.Unity.Module_Device;

public abstract class SvrPlugin
{
	private static SvrPlugin instance;

	public static SvrPlugin Instance
	{
		get
		{
			if (instance == null)
			{
				if(!Application.isEditor && Application.platform == RuntimePlatform.Android)
				{
                    if (API_Module_Device.CurrentAndroid != null && API_Module_Device.CurrentAndroid.type == AndroidDeviceType.Other) {
                        instance = SvrPluginAndroidOther.Create();
                    } else {
                        instance = SvrPluginAndroid.Create();
                    }
				}
				else
				{
					instance = SvrPluginWin.Create();
				}
			}
			return instance;
		}
	}

    public SvrManager svrCamera = null;
    public SvrEye[] eyes = null;
    public SvrOverlay[] overlays = null;
    public DeviceInfo deviceInfo;

    public enum PerfLevel
	{
        kPerfSystem = 0,
        kPerfMaximum = 1,
		kPerfNormal = 2,
		kPerfMinimum = 3
	}

    public enum TrackingMode
    {
        kTrackingOrientation = (1 << 0),
        kTrackingPosition = (1 << 1),
        kTrackingEye = (1 << 2),
    }

    public enum EyePoseStatus
    {
        kGazePointValid = (1 << 0),
        kGazeVectorValid = (1 << 1),
        kEyeOpennessValid = (1 << 2),
        kEyePupilDilationValid = (1 << 3),
        kEyePositionGuideValid = (1 << 4)
    };

    public enum FrameOption
    {
        kDisableDistortionCorrection = (1 << 0),    //!< Disables the lens distortion correction (useful for debugging)
        kDisableReprojection = (1 << 1),            //!< Disables re-projection
        kEnableMotionToPhoton = (1 << 2),           //!< Enables motion to photon testing 
        kDisableChromaticCorrection = (1 << 3)      //!< Disables the lens chromatic aberration correction (performance optimization)
    };

    public struct HeadPose
    {
        public Vector3 position;
        public Quaternion orientation;
    }

    public struct EyePose
    {
        public int leftStatus;          //!< Bit field (svrEyePoseStatus) indicating left eye pose status
        public int rightStatus;         //!< Bit field (svrEyePoseStatus) indicating right eye pose status
        public int combinedStatus;      //!< Bit field (svrEyePoseStatus) indicating combined eye pose status

        public Vector3 leftPosition;        //!< Left Eye Gaze Point
        public Vector3 rightPosition;       //!< Right Eye Gaze Point
        public Vector3 combinedPosition;    //!< Combined Eye Gaze Point (HMD center-eye point)

        public Vector3 leftDirection;       //!< Left Eye Gaze Point
        public Vector3 rightDirection;      //!< Right Eye Gaze Point
        public Vector3 combinedDirection;   //!< Comnbined Eye Gaze Vector (HMD center-eye point)

        public float leftOpenness;          //!< Left eye value between 0.0 and 1.0 where 1.0 means fully open and 0.0 closed.
        public float rightOpenness;         //!< Right eye value between 0.0 and 1.0 where 1.0 means fully open and 0.0 closed.

        //public float leftDilation;       //!< Left eye value in millimeters indicating the pupil dilation
        //public float rightDilation;      //!< Right eye value in millimeters indicating the pupil dilation

        //public Vector3 leftGuide;    //!< Position of the inner corner of the left eye in meters from the HMD center-eye coordinate system's origin.
        //public Vector3 rightGuide;   //!< Position of the inner corner of the right eye in meters from the HMD center-eye coordinate system's origin.
    }

    public struct ViewFrustum
    {
        public float left;           //!< Left Plane of Frustum
        public float right;          //!< Right Plane of Frustum
        public float top;            //!< Top Plane of Frustum
        public float bottom;         //!< Bottom Plane of Frustum

        public float near;           //!< Near Plane of Frustum
        public float far;            //!< Far Plane of Frustum (Arbitrary)
    }

    public struct DeviceInfo
	{
		public int 		displayWidthPixels;
		public int    	displayHeightPixels;
		public float  	displayRefreshRateHz;
		public int    	targetEyeWidthPixels;
		public int    	targetEyeHeightPixels;
		public float  	targetFovXRad;
		public float  	targetFovYRad;
        public ViewFrustum targetFrustumLeft;
        public ViewFrustum targetFrustumRight;
        public float    targetFrustumConvergence;
        public float    targetFrustumPitch;
	}

	public virtual int  GetPredictedPoseModify(ref Quaternion orientation, ref Vector3 position, int frameIndex = -1)
	{
		return 0;
	}
    public virtual bool PollEvent(ref SvrManager.SvrEvent frameEvent) { return false; }

    public virtual bool IsInitialized() { return false; }
    public virtual bool IsRunning() { return false; }
    public virtual IEnumerator Initialize ()
    {
        svrCamera = SvrManager.Instance;
        if (svrCamera == null)
        {
            Debug.LogError("SvrManager object not found!");
            yield break;
        }

        yield break;
    }
	public virtual IEnumerator BeginVr(int cpuPerfLevel =0, int gpuPerfLevel =0)
    {
        if (eyes == null)
        {
            eyes = SvrEye.Instances.ToArray();
            if (eyes == null)
            {
                Debug.Log("Components with SvrEye not found!");
            }

            Array.Sort(eyes);
        }

        if (overlays == null)
        {
            overlays = SvrOverlay.Instances.ToArray();
            if (overlays == null)
            {
                Debug.Log("Components with SvrOverlay not found!");
            }

            Array.Sort(overlays);
        }

        yield break;
    }
    public virtual void EndVr()
    {
        eyes = null;
        overlays = null;
    }
	public virtual void BeginEye(int sideMask, float[] frameDelta) { }
    public virtual void EndEye(int sideMask, int layerMask) { }
    public virtual void SetTrackingMode(int mode) { }
	public virtual void SetFoveationParameters(int textureId, int previousId, float focalPointX, float focalPointY, float foveationGainX, float foveationGainY, float foveationArea, float foveationMinimum) {}
    public virtual void ApplyFoveation() { }
    public virtual int  GetTrackingMode() { return 0; }
    public virtual void SetPerformanceLevels(int newCpuPerfLevel, int newGpuPerfLevel) { }
    public virtual void SetFrameOption(FrameOption frameOption) { }
    public virtual void UnsetFrameOption(FrameOption frameOption) { }
    public virtual void SetVSyncCount(int vSyncCount) { }
    public virtual bool RecenterTracking() { return true; }
    public virtual void SubmitFrame(int frameIndex, float fieldOfView, int frameType) { }
    public virtual int GetPredictedPose(ref Quaternion orientation, ref Vector3 position, int frameIndex = -1)
    {
        orientation = Quaternion.identity;
        position = Vector3.zero;
        return 0;
    }
    public virtual int GetHeadPose(ref HeadPose headPose, int frameIndex = -1)
    {
        headPose.orientation = Quaternion.identity;
        headPose.position = Vector3.zero;
        return 0;
    }
    public virtual int GetEyePose(ref EyePose eyePose, int frameIndex = -1)
    {
        eyePose.leftStatus = 0;
        eyePose.rightStatus = 0;
        eyePose.combinedStatus = 0;
        return 0;
    }
    public abstract DeviceInfo GetDeviceInfo ();

    public virtual int SVRGetTransformMatrix(ref bool outBLoaded, float[] outTransformArray) { outBLoaded = false; return 0; }

    public virtual int SVRGetLatestQVRCameraBinocularData(ref bool outBUdate, ref uint outCurrFrameIndex, ref ulong outFrameExposureNano, byte[] outLeftFrameData, byte[] outRightFrameData) { outBUdate = false; return 0; }

    //public Matrix4x4 leftViewMatrix, rightViewMatrix;
    public virtual int SVRGetLatestEyeMatrices(float[] outLeftEyeMatrix,
                                           float[] outRightEyeMatrix,
                                           float[] outT,
                                           float[] outR,
                                           int frameIndex,
                                           bool isMultiThreadedRender) { return 0; }
	public virtual void Shutdown()
    {
        SvrPlugin.instance = null;
    }

	//---------------------------------------------------------------------------------------------
	public virtual int ControllerStartTracking(string desc) {
		return -1;
	}

	//---------------------------------------------------------------------------------------------
	public virtual void ControllerStopTracking(int handle) {
	}

	//---------------------------------------------------------------------------------------------
	public virtual SvrControllerState ControllerGetState(int handle, int space) {
		return new SvrControllerState();
	}

	//---------------------------------------------------------------------------------------------
	public virtual void ControllerSendMessage(int handle, SvrController.svrControllerMessageType what, int arg1, int arg2) {
	}

	//---------------------------------------------------------------------------------------------
	public virtual object ControllerQuery(int handle, SvrController.svrControllerQueryType what) {
		return null;
	}
	public virtual void StartHandTracking() {}

    public virtual void StopHandTracking() {}
    public virtual void GetHandData(float[] mode, float[] pose) {
    //public virtual int GetHandData(ref HandPose[] handsPos) {
        return ;
    }



    //-----------------------KS---------------------

    public delegate void OnKeyEvent(int keycode, int action, int lr);
    public delegate void OnKeyTouchEvent(bool key1, bool key2, bool key3, bool key4, int lr);
    public delegate void OnTouchEvent(int touch_x, int touch_y, int lr);
    public delegate void OnHallEvent(int hall_x, int hall_y, int lr);
    public delegate void OnChargingEvent(bool isCharging, int lr);
    public delegate void OnBatteryEvent(int battery, int lr);
    public delegate void OnConnectEvent(bool isConnected, int lr);

    public virtual void HandShank_SetKeyEventCallback(OnKeyEvent _event) { }

    public virtual void HandShank_SetKeyTouchEventCallback(OnKeyTouchEvent _event) { }

    public virtual void HandShank_SetTouchEventCallback(OnTouchEvent _event) { }

    public virtual void HandShank_SetHallEventCallback(OnHallEvent _event) { }

    public virtual void HandShank_SetChargingEventCallback(OnChargingEvent _event) { }

    public virtual void HandShank_SetBatteryEventCallback(OnBatteryEvent _event) { }

    public virtual void HandShank_SetConnectEventCallback(OnConnectEvent _event) { }


    public virtual int HandShank_GetBattery(int lr) { return -1; }

    public virtual int HandShank_GetVersion(int lr) { return -1; }

    public virtual int HandShank_Getbond(int lr) { return -1; }
    public virtual bool HandShank_GetConnectState(int lr) { return false; }

    public virtual int Fetch3dofHandShank(float[] outOrientationArray, int lr) { return -1; }

    public virtual int Fetch6dofHandShank(float[] outOrientationArray, int lr) { return -1; }


    //-----------------------For A11G---------------------
    public delegate void GlassDisconnectedCallBack();
    public virtual void SetGlassDisconnectedCallBack(GlassDisconnectedCallBack callBack) { }

    //-----------------------For HeadsetInput---------------------
    public virtual bool HeadSetEnterKeyDown() { return Input.GetMouseButtonDown(0); }
    public virtual bool HeadSetEnterKeyUp() { return Input.GetMouseButtonUp(0); }
    public virtual bool HeadSetBackKeyDown() { return Input.GetKeyDown(KeyCode.Escape); }
    public virtual bool HeadSetBackKeyUp() { return Input.GetKeyUp(KeyCode.Escape); }

    //-----------------------For GreyCamera---------------------
    public virtual void SCSvrGetLatestQVRCameraFrameData(ref bool outBUdate, ref uint outCurrFrameIndex, ref ulong outFrameExposureNano,
            byte[] outFrameData, float[] outTRDataArray) { outBUdate = false;}
    public virtual void SCSvrGetLatestQVRCameraFrameDataNoTransform(ref bool outBUdate, ref uint outCurrFrameIndex, ref ulong outFrameExposureNano,
            byte[] outFrameData, float[] outTRDataArray) { outBUdate = false; }
		
	//-----------------------For luncher---------------------	
	// david start
    public virtual int initLayer() { return -1; }
    public virtual int startLayerRendering() { return -1; }
    public virtual int getAllLayersData(ref SvrManager.SCAllLayers outAllLayers) { return -1; }
    public virtual int endLayerRendering(ref SvrManager.SCAllLayers allLayers) { return -1; }
    public virtual int destroyLayer() { return -1; }
    public virtual int updateModelMatrix(UInt32 layerId, float[] modelMatrixArray) { return -1; }
    public virtual int sendActionBarCMD(UInt32 layerId, int cmd) { return -1; }
    public virtual int injectMotionEvent(UInt32 layerId, int displayID, int action, float x, float y) { return -1; }
    // david end
}

