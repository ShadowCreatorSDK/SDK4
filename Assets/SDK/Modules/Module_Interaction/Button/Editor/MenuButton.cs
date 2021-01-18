using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SC.Menu {
    public class MenuButton : MenuBase {

        [MenuItem("GameObject/SDK/SCButton", false, 200)]
        public static void createButton() {
            CreatePrefab("Prefabs/SCButton");
        }
    }
}
