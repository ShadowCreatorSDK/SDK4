using SC.XR.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace SC.XR.Unity {

    [CreateAssetMenu(menuName = "SCConfig/SDKVersion")]
    public class SDKVersion : ScriptableObject {
        public Version version;

        public string GetVersion {
            get {
                if (string.IsNullOrEmpty(version.Build)) {
                    return version.Major + "." + version.Minor + "." + version.Revision;
                } else {
                    return version.Major + "." + version.Minor + "." + version.Revision + "." + version.Build;
                }
            }
        }
    }


    [Serializable]
    public class Version {
        public string Major;
        public string Minor;
        public string Revision;
        public string Build;
    }
}