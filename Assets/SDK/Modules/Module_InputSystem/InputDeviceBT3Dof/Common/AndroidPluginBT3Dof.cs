using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class AndroidPluginBT3Dof : AndroidPluginBase {

        private static AndroidJavaClass mClassBT3DofManager = null;
        internal static AndroidJavaClass ClassBT3DofManager {
            get {
                if(mClassBT3DofManager == null) {
                    mClassBT3DofManager = GetAndroidJavaClass("com.invision.handshank.SDKHandShankManager");
                }
                return mClassBT3DofManager;
            }
        }

        private static AndroidJavaObject mBT3DofManager = null;
        internal static AndroidJavaObject BT3DofManager {
            get {
                if(mBT3DofManager == null) {
                    mBT3DofManager = ClassFunctionCallStatic<AndroidJavaObject>(ClassBT3DofManager, "getSDKHandShankManager", Context);
                }
                return mBT3DofManager;
            }
        }

    }
}
