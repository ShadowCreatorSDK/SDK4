using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class AndroidPluginKS : AndroidPluginBase {

        private static AndroidJavaClass mClassKSManager = null;
        internal static AndroidJavaClass ClassKSManager {
            get {
                if(mClassKSManager == null) {
                    mClassKSManager = GetAndroidJavaClass("com.invision.handshank.SDKHandShankManager");
                }
                return mClassKSManager;
            }
        }

        private static AndroidJavaObject mKSManager = null;
        internal static AndroidJavaObject KSManager {
            get {
                if(mKSManager == null) {
                    mKSManager = ClassFunctionCallStatic<AndroidJavaObject>(ClassKSManager, "getSDKHandShankManager", Context);
                }
                return mKSManager;
            }
        }

    }
}
