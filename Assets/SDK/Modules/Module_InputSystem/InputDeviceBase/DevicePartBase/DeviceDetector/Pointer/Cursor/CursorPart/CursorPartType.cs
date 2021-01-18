using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.XR.Unity.Module_InputSystem {
    public enum CursorPartType {

        UnDefined,

        Reset,
        Press,
        Focus,
        MoveArrowsEastWest,
        MoveArrowsMove,
        MoveArrowsNortheastSouthwest,
        MoveArrowsNorthSouth,
        MoveArrowsNorthwestSoutheast,
        RotateArrowsHorizontal,
        RotateArrowsVertical,

        FingerPress,
        FingerFocus,

    }
}
