using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SC.XR.Unity
{
    public class SCKeyboard3D : SCKeyboardBase
    {
        public SCKeyboard3D(Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
            : base(parent, position, rotation, scale) { }

        public override string PrefabResourceName
        {
            get
            {
                return "Keyboard/3DKeyboard";
            }
        }
    }
}