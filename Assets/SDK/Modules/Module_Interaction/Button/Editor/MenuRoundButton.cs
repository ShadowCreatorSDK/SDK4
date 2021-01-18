using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SC.Menu {
    public class MenuRoundButton : MenuBase {


        [MenuItem("GameObject/SDK/SCRoundButton", false, 200)]
        public static void createRoundButton() {
            CreatePrefab("Prefabs/SCRoundButton");
        }
    }
}
