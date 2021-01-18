using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SC.Menu
{
    public class MenuSCSlider3DType1 : MenuBase
    {
        [MenuItem("GameObject/SC3DUI/MenuSCSlider3DType1", priority = 0)]
        public static void createButton()
        {
            CreatePrefab("Prefabs/SCSlider3DType1");
        }
    }
}
