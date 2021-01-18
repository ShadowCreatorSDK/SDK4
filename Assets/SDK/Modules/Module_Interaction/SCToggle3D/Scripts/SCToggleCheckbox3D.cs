using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SC.XR.Unity
{
    public class SCToggleCheckbox3D : SCToggleBase
    {
        public GameObject m_CheckObject;

        protected override void PlayEffect()
        {
            if (m_CheckObject == null)
                return;
            m_CheckObject.SetActive(m_IsOn);
        }
    }
}
