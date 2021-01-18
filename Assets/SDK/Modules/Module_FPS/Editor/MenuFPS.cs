using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SC.Menu {
    public class MenuFPS : MenuBase {

        [MenuItem("GameObject/SDK/FPS", false, 200)]

        public static void createAction() {
            CreatePrefab("Prefabs/FPS");
        }

    }
}
