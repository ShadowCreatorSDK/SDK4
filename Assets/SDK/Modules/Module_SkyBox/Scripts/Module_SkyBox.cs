using SC.XR.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_SkyBox {
    public class Module_SkyBox : MonoBehaviour {

        private static Module_SkyBox mInstance;

        public static Module_SkyBox getInstance {
            get {
                if (mInstance == null) {
                    mInstance = new GameObject("SDKVersion").AddComponent<Module_SkyBox>();
                }
                return mInstance;
            }
        }

        SDKSkyBox SDKSkyBox;
        void Awake() {
            SDKSkyBox = Resources.Load<SDKSkyBox>("SDKSkyBox");
            if (SDKSkyBox == null) {
                DebugMy.Log("SDKSkyBox Not Exist !", this, true);
            }
        }

        public Material GetSkyBox(SkyBoxType type) {
            if (getInstance.SDKSkyBox != null) {
                foreach (var skybox in getInstance.SDKSkyBox.SkyBoxList)
                    if (skybox.type == type) {
                        return skybox.Material;
                    }
            }
            return null;
        }


    }
}
