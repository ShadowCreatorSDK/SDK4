using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public class DefaultCursor : CursorBase {


        public float DefaultDistance = 3f;
        public float pressLocalScale = 0.7f;

        public Dictionary<CursorPartType, CursorPartBase> CursorPartDir;



        #region Module Behavior
        public override void OnSCAwake() {
            base.OnSCAwake();

            CursorPartDir = new Dictionary<CursorPartType, CursorPartBase>();
            CursorPartBase[] cursorList = transform.GetComponentsInChildren<CursorPartBase>(true);

            foreach(var item in cursorList) {
                if(item.CursorType == CursorPartType.UnDefined) {
                    DebugMy.Log("Contain UnDefined Type ! Ignor This", this);
                    continue;
                }

                if(CursorPartDir.ContainsKey(item.CursorType)) {
                    DebugMy.Log("Contain Same Type CursorPart ! Ignor This", this);
                    continue;
                }
                CursorPartDir.Add(item.CursorType, item);
                //DebugMy.Log("Add CursorPart:" + item.CursorType, this);

                AddModule(item);
            }
            cursorList = null;

        }

        public override void OnSCStart() {
            base.OnSCStart();

            InitCursorPart();
        }


        public override void OnSCDisable() {
            base.OnSCDisable();
            AllCursorPartModuleStop();
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();

            CursorPartDir.Clear();
            CursorPartDir = null;
        }

        #endregion

        public override void UpdateCursorVisual() {

            UpdateNormalCursorVisual();
            UpdateOtherCursorVisual();
        }

        public virtual void InitCursorPart() {
            CusrorPartNormalStart(CursorPartType.Focus);
        }


        CursorBehavoir cursorBehavoir;
        public override void UpdateTransform() {

            ///Todo 更新位置，旋转等
            transform.rotation = pointerBase.transform.rotation;

            if(pointerBase.detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData.dragging) {

                cursorBehavoir = pointerBase.detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData.pointerDrag.GetComponent<CursorBehavoir>();
                if(cursorBehavoir) {
                    if(cursorBehavoir.positionBehavoir == CursorBehavoir.PositionBehavoir.AnchorPosition3D) {
                        transform.position = pointerBase.detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData.dragAnchorPosition3D;
                    } else if(cursorBehavoir.positionBehavoir == CursorBehavoir.PositionBehavoir.Position3D) {
                        transform.position = pointerBase.detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData.Position3D;
                    }

                    if(cursorBehavoir.visualBehavoir == CursorBehavoir.VisualBehavoir.Scale) {

                    }

                } else {
                    transform.position = pointerBase.detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData.Position3D;
                }

            } else if(pointerBase.detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData.pointerCurrentRaycast.gameObject) {
                transform.position = pointerBase.detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData.pointerCurrentRaycast.worldPosition;

            } else {
                transform.position = pointerBase.transform.TransformPoint(Vector3.forward * DefaultDistance);

            }
        }


        void OnDrawGizmos() {
            if(Application.isPlaying && IsModuleStarted) {
                if(pointerBase.detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData.dragging) {
                    // Draw a yellow sphere at the transform's position
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(pointerBase.detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData.Position3D, 0.005f);

                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(pointerBase.detectorBase.inputDevicePartBase.inputDataBase.SCPointEventData.dragObjPosition, 0.002f);
                }
            }

        }

        public virtual void UpdateNormalCursorVisual() {
            if(pointerBase.detectorBase.inputDevicePartBase.inputDataBase.inputKeys.GetKeyDown(InputKeyCode.Enter)) {
                CusrorPartNormalStart(CursorPartType.Press);
            } else if(pointerBase.detectorBase.inputDevicePartBase.inputDataBase.inputKeys.GetKeyUp(InputKeyCode.Enter)) {
                CusrorPartNormalStart(CursorPartType.Focus);
            }
        }


        public virtual void UpdateOtherCursorVisual() {
            //if(pointerBase.inputDevicePartBase.inputDataBase.SCPointEventData.pointerCurrentRaycast.gameObject) {
            //    cursorBehavoir = pointerBase.inputDevicePartBase.inputDataBase.SCPointEventData.pointerCurrentRaycast.gameObject.GetComponent<CursorBehavoir>();
            //    if(cursorBehavoir) {
            //        if(cursorBehavoir.visualBehavoir == CursorBehavoir.VisualBehavoir.Scale) {
            //            CursorPartModuleStart(CursorPartType.MoveArrowsMove);
            //        }
            //    }
            //} else { 

            //}

            //if(pointerBase.inputDevicePartBase.inputDataBase.SCPointEventData.pointerDrag) {
            //    cursorBehavoir = pointerBase.inputDevicePartBase.inputDataBase.SCPointEventData.pointerDrag.GetComponent<CursorBehavoir>();
            //    if(cursorBehavoir) {
            //        if(cursorBehavoir.visualBehavoir == CursorBehavoir.VisualBehavoir.Scale) {
            //            CursorPartModuleStart(CursorPartType.MoveArrowsMove);
            //        }
            //    } else {

            //    }

            //}
        }




        CursorPartBase cursorpartTemp;
        public CursorPartBase GetCursorPart(CursorPartType type) {
            CursorPartBase cursor = null;
            if(CursorPartDir.ContainsKey(type)) {
                cursor = CursorPartDir[type];
            }
            return cursor;
        }

        public void CursorPartModuleStart(CursorPartType type) {
            if(CursorPartDir.ContainsKey(type)) {
                CursorPartDir[type].ModuleStart();
            } else {
                DebugMy.Log("CursorPartModuleStart: Can not Found CursorPart " + type, this);
            }
        }

        public void CursorPartModuleStop(CursorPartType type) {
            if(CursorPartDir.ContainsKey(type)) {
                CursorPartDir[type].ModuleStop();
            } else {
                DebugMy.Log("CursorPartModuleStop: Can not Found CursorPart " + type, this);
            }
        }

        /// <summary>
        /// 启用正常中间光标
        /// </summary>
        /// <param name="type"></param>
        public void CusrorPartNormalStart(CursorPartType type) {
            foreach(var item in CursorPartDir.Values) {

                if((item.CursorType == CursorPartType.FingerPress ||
                    item.CursorType == CursorPartType.FingerFocus ||
                    item.CursorType == CursorPartType.Focus ||
                    item.CursorType == CursorPartType.Press ||
                    item.CursorType == CursorPartType.Reset) &&

                    item.CursorType == type) {

                    if(item.IsModuleStarted == false)
                        item.ModuleStart();

                } else if(
                     (item.CursorType == CursorPartType.FingerPress ||
                     item.CursorType == CursorPartType.FingerFocus ||
                     item.CursorType == CursorPartType.Focus ||
                     item.CursorType == CursorPartType.Press ||
                     item.CursorType == CursorPartType.Reset) &&

                     item.IsModuleStarted) {
                    item.ModuleStop();
                }
            }
        }

        public void AllCursorPartModuleStart() {
            foreach(var item in CursorPartDir.Values) {
                item.ModuleStart();
            }
        }
        public void AllCursorPartModuleStop() {
            foreach(var item in CursorPartDir.Values) {
                if(item.IsModuleStarted) {
                    item.ModuleStop();
                }
            }
        }

    }
}
