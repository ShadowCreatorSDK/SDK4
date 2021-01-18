using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public abstract class CursorPartBase : SCModuleMono {
        /// <summary>
        /// Cursor Type
        /// </summary>
        public CursorPartType CursorType;

        public CursorBase Cursor { get; protected set; }

        public override void OnSCAwake() {
            base.OnSCAwake();
            if(CursorType == CursorPartType.UnDefined) {
                DebugMy.Log("CursorType UnDefined !", this);
            }
            Cursor = GetComponentInParent<CursorBase>();
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            Cursor = null;
        }

    }
}
