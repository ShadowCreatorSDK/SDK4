using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    /// <summary>
    /// For SCInputModule know how many Canvas will be Detect, Canvas don't add the Component will ignor by SCInputModule
    /// </summary>
    public class CanvasCollection : MonoBehaviour {

        public static List<Canvas> CanvasList = new List<Canvas>();

        Canvas canvas;
        void OnEnable() {
            canvas = GetComponent<Canvas>();
            if(canvas) {
                CanvasList.Add(canvas);
            }
        }

        void OnDisable() {
            if(canvas) {
                CanvasList.Remove(canvas);
            }
        }

    }
}