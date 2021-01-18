using SC.XR.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace SC.XR.Unity {

    [CreateAssetMenu(menuName = "SCConfig/SDKConfiguration")]
    public class SDKConfiguration : ScriptableObject {
        public List<Section> Configs;
    }


    [Serializable]
    public class Section {
        public string section;
        public List<KEY_VALUE> KEY_VALUE;
    }

    [Serializable]
    public class KEY_VALUE {
        public string Name;
        public string Value;
    }
}