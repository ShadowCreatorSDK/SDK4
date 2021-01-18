using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace SC.XR.Unity.Module_InputSystem {

    /// <summary>
    /// 负责显示
    /// </summary>
    public class BezierLine : LineBase {


        BezierDataProvider bezierDataProvider;
        public override void OnSCAwake() {
            base.OnSCAwake();
            //bezier = new Bezier(transform);
            bezierDataProvider = new BezierDataProvider(this);
        }

        public override void DrawLineIndicate() {
            base.DrawLineIndicate();

            bezierDataProvider.FirstPoint = pointerBase.transform.position;
            bezierDataProvider.LastPoint = pointerBase.cursorBase.transform.position;
            bezierDataProvider.UpdatePointe1_2();
            lineRenderer.positionCount = linePointerCount;
            lineRenderer.SetPositions(bezierDataProvider.GetLinePointers(lineRenderer.positionCount));

            //bezier.SetPoint(3, pointerBase.cursorBase.transform.position);
            //lineRenderer.positionCount = linePointerCount;
            //lineRenderer.SetPositions(bezier.UpdateControlPointsAndGetLinePoints(lineRenderer.positionCount));


        }

        //void OnDrawGizmos() {
        //    if(Application.isPlaying && bezierDataProvider!=null) {

        //        Gizmos.color = Color.black * 0.5f;
        //        Gizmos.DrawSphere(bezierDataProvider.GetPoint(0), 0.01f);
        //        Gizmos.color = Color.black * 0.5f;
        //        Gizmos.DrawSphere(bezierDataProvider.GetPoint(1), 0.02f);
        //        Gizmos.color = Color.black * 0.5f;
        //        Gizmos.DrawSphere(bezierDataProvider.GetPoint(2), 0.03f);
        //        Gizmos.color = Color.blue * 0.5f;
        //        Gizmos.DrawSphere(bezierDataProvider.GetPoint(3), 0.04f);
        //    }
        //}
    }
}
