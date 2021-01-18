using SC.XR.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SC.XR.Unity
{
    public class SCToggleSwitch3D : SCToggleBase
    {
        public GameObject dot;
        public Renderer toggleBG;
        private MaterialPropertyBlock materialPropertyBlock;

        public Color switchOffColor;
        public Color switchOnColor;
        public Vector3 dotSwitchOffLocalPosition;
        public Vector3 dotSwitchOnLocalPosition;

        protected override void PlayEffect()
        {
            if (materialPropertyBlock == null)
            {
                materialPropertyBlock = new MaterialPropertyBlock();
            }

            if (m_IsOn)
            {
                materialPropertyBlock.SetColor("_Color", switchOnColor);
                dot.transform.localPosition = dotSwitchOnLocalPosition;
            }
            else
            {
                materialPropertyBlock.SetColor("_Color", switchOffColor);
                dot.transform.localPosition = dotSwitchOffLocalPosition;
            }

            toggleBG.SetPropertyBlock(materialPropertyBlock);
        }
    }
}
