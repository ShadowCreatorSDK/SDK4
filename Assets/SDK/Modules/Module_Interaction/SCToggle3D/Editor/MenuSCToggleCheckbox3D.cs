﻿using SC.XR.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SC.Menu
{
    public class MenuSCCheckbox3D : MenuBase
    {
        [MenuItem("GameObject/SC3DUI/SCTogglCheckbox3D", priority = 0)]
        public static void createButton()
        {
            CreatePrefab("Prefabs/" + typeof(SCToggleCheckbox3D).Name);
        }
    }
}