using UnityEngine;
using DG.Tweening;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public class GrabCursor : DefaultCursor {

        public GrabPointer grabPointer {
            get {
                return Transition<GrabPointer>(pointerBase);
            }
        }

    }
}
