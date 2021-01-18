using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SC.Menu {
    public class MenuSDKSystem : MenuBase {

        [MenuItem("GameObject/SDK/SDKSystem", false, 10)]
        public static void createAction() {
            CreatePrefab("Prefabs/SDKSystem");
        }

        [MenuItem("SDK/SDKSystem", false, 10)]
        public static void createAction1() {
            CreatePrefab("Prefabs/SDKSystem");
        }

    }
}
