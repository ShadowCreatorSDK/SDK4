using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity
{
    public class SCKeyboard2D : SCKeyboardBase
    {
        public SCKeyboard2D(Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
            : base(parent, position, rotation, scale) { }

        public override string PrefabResourceName
        {
            get
            {
                return "Keyboard/2DKeyboard";
            }
        }
    }
}
