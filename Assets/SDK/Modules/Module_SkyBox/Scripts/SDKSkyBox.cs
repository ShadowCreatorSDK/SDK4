using SC.XR.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_SkyBox {

    [CreateAssetMenu(menuName = "SCConfig/SDKSkyBox")]
    public class SDKSkyBox : ScriptableObject {
        public List<skybox> SkyBoxList;
    }


    [Serializable]
    public class skybox {
        public SkyBoxType type;
        public Material Material;
    }
}