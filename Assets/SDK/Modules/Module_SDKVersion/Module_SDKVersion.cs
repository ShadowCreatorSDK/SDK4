using SC.XR.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_SDKVersion {
    public class Module_SDKVersion : MonoBehaviour {

        private static Module_SDKVersion mInstance;

        public static Module_SDKVersion getInstance {
            get {
                if (mInstance == null) {
                    mInstance = new GameObject("SDKVersion").AddComponent<Module_SDKVersion>();
                }
                return mInstance;
            }
        }

        SDKVersion SDKVersion;
        void Awake() {
            SDKVersion = Resources.Load<SDKVersion>("SDKVersion");
            if (SDKVersion == null) {
                DebugMy.Log("SDKVersion Not Exist !", this, true);
            }
        }

        public string GetVersion {
            get {
                return SDKVersion != null ? SDKVersion.GetVersion : "Version File Not Found !";
            }
        }

    }
}
