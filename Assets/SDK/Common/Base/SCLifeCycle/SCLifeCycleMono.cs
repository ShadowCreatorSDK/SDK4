using UnityEngine;

namespace SC.XR.Unity {
    public abstract class SCLifeCycleMono : MonoBehaviour, ISCLifeCycle {


        public virtual void OnSCAwake() {
            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCAwake", this);
        }

        public virtual void OnSCEnable() {
            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCEnable", this);
        }

        public virtual void OnSCStart() {
            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCStart", this);
        }

        public virtual void OnSCUpdate() {
            //DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCUpdate", this);
        }

        public virtual void OnSCLateUpdate() {
            //DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCLateUpdate", this);
        }

        public virtual void OnSCFuncitonWaitForEndOfFrame() {
            //DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCFuncitonWaitForEndOfFrame", this);
        }

        public virtual void OnSCDisable() {
            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCDisable", this);
        }

        public virtual void OnSCDestroy() {
            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCDestroy", this);
        }
    }
}
