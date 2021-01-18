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
    public abstract class LineBase : SCModuleMono {


        PointerBase mPointerBase;
        public PointerBase pointerBase {
            get {
                if(mPointerBase == null) {
                    mPointerBase = GetComponentInParent<PointerBase>();
                }
                return mPointerBase;
            }
            private set {
                mPointerBase = value;
            }
        }


        public LineRenderer lineRenderer;
        protected int linePointerCount = 24;
        [Header("The line properties When Draging Status")]
        public Gradient dragLineGradient;
        public Material dragLineMaterial;
        public AnimationCurve dragCurve;

        [Header("The line properties When Normal Status")]
        public Gradient normalLineGradient;
        public Material normalLineMaterial;
        public AnimationCurve normalCurve;

        private InputKeyState inputKeyState;


        public override void OnSCAwake() {
            base.OnSCAwake();
            lineRenderer = GetComponent<LineRenderer>();
            if(lineRenderer == null) {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
            lineRenderer.enabled = false;
        }

        public override void OnSCStart() {
            base.OnSCStart();
            lineRenderer.enabled = true;
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            UpdateTransform();
            DrawLineIndicate();
        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            lineRenderer.enabled = false;
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            pointerBase = null;
        }

        public virtual void UpdateTransform() {
            transform.position = pointerBase.transform.position;
            transform.rotation = pointerBase.transform.rotation;
        }
        public virtual void DrawLineIndicate() {

            // Set line renderer properties
            pointerBase.detectorBase.inputDevicePartBase.inputDataBase.inputKeys.inputKeyDic.TryGetValue(InputKeyCode.Enter, out inputKeyState);
            if(inputKeyState == InputKeyState.DOWN || inputKeyState == InputKeyState.LONG) {
                lineRenderer.sharedMaterial = dragLineMaterial;
                lineRenderer.colorGradient = dragLineGradient;
                lineRenderer.widthCurve = dragCurve;
            } else {
                lineRenderer.sharedMaterial = normalLineMaterial;
                lineRenderer.colorGradient = normalLineGradient;
                lineRenderer.widthCurve = normalCurve;
            }

            lineRenderer.alignment = LineAlignment.View;
            lineRenderer.loop = false;
            lineRenderer.textureMode = LineTextureMode.RepeatPerSegment;
            lineRenderer.numCapVertices = 8;
            lineRenderer.numCornerVertices = 8;
            lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
            lineRenderer.lightProbeUsage = LightProbeUsage.Off;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;


        }
    }
}
