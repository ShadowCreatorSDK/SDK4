using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public class CursorBehavoir:MonoBehaviour {

        public enum PositionBehavoir { 
            AnchorPosition3D,
            Position3D
        }

        public PositionBehavoir positionBehavoir;


        public enum VisualBehavoir { 
            Scale,
            Drag,
        }

        public VisualBehavoir visualBehavoir;

    }
}
