using UnityEngine;
using UnityEditor;

namespace SC.Menu {
    public abstract class MenuBase : MonoBehaviour {
        protected static void CreatePrefab(string ResourcesPath) {
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load(ResourcesPath));

            if(go) {
                GameObjectUtility.SetParentAndAlign(go, Selection.activeTransform == null ? null : Selection.activeTransform.gameObject);
                go.transform.SetParent(Selection.activeTransform);
                Undo.RegisterCreatedObjectUndo(go, go.name);
                Selection.activeTransform = go.transform;
                //Debug.Log("Create Sucess ! Prefab:" + ResourcesPath);
            } else {
                //Debug.LogError("Create Failed ! Prefab:" + ResourcesPath);
            }

        }
    }
}

