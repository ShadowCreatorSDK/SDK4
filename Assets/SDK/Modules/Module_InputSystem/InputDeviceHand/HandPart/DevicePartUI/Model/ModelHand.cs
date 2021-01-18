using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand
{

    public class ModelHand : ModelBase {
        public InputDeviceHandPartUI inputDeviceHandPartUI {
            get {
                return inputDevicePartUIBase as InputDeviceHandPartUI;
            }
        }

        [Tooltip("All Vaild HandModel")]
        [SerializeField]
        private List<AbstractHandModel> mAllHandModelList;
        protected List<AbstractHandModel> AllHandModelList {
            get {
                if (mAllHandModelList.Count == 0) {
                    mAllHandModelList = new List<AbstractHandModel>(GetComponentsInChildren<AbstractHandModel>(true)) ;
                }
                return mAllHandModelList;
            }
        }

        /// <summary>
        /// which HandModel Used For PointerModuel
        /// </summary>
        private AbstractHandModel mActiveHandModel;
        public AbstractHandModel ActiveHandModel {
            get {
                if (mActiveHandModel == null) {
                    
                    if (VisualHandModelList==null || VisualHandModelList.Count == 0) {
                        //DebugMy.LogError("No Visual HandModel",this);
                        return null;
                    }

                    foreach (var handmodel in VisualHandModelList) {
                        if (handmodel.handModelType == PointerHand) {
                            mActiveHandModel = handmodel;
                            DebugMy.Log("PointerHand:"+ handmodel.handModelType, this);
                            break;
                        }
                    }

                    if (mActiveHandModel == null) {
                        mActiveHandModel = VisualHandModelList[0];
                        DebugMy.Log("No Find PointerHand, PointerHand" + mActiveHandModel.handModelType, this, true);
                    }
                }
                return mActiveHandModel;
            }
        }

        [Tooltip("All Vaild Visual HandModel")]
        public List<AbstractHandModel> VisualHandModelList;

        /// <summary>
        /// Which HandModel Used For PointerModule
        /// </summary>
        [Tooltip("Which HandModel Used For PointerModule")]
        public HandModelType PointerHand = HandModelType.EffectHand;

        public FingerUI[] fingerUI {
            get {
                if (ActiveHandModel != null) {
                    return ActiveHandModel.fingerUI;
                }
                return null;
            }
        }

        public override void OnSCStart() {
            base.OnSCStart();

            if (VisualHandModelList.Count == 0) {
                VisualHandModelList = new List<AbstractHandModel>();
                foreach (var handmodel in AllHandModelList) {
                    AddModule(handmodel);
                    if (handmodel.handModelType == HandModelType.EffectHand && inputDeviceHandPartUI.inputDeviceHandPart.EnableEffectHandModel) {
                        VisualHandModelList.Add(handmodel);
                    } else if (handmodel.handModelType == HandModelType.CubeHand && inputDeviceHandPartUI.inputDeviceHandPart.EnableCubeHandModel) {
                        VisualHandModelList.Add(handmodel);
                    }
                }
            }

            foreach (var handmodel in VisualHandModelList) {
                handmodel.ModuleStart();
            }
        }

        void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.black * 0.3f;
                Gizmos.DrawSphere(inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.centerPosition, 0.01f);

                Gizmos.color = Color.black * 0.2f;
                Gizmos.DrawSphere(inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.centerPosition + inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.normal * 0.05f, 0.01f);

                Gizmos.color = Color.black * 0.2f;
                Gizmos.DrawSphere(inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.centerPosition + inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.right * 0.05f, 0.01f);
            }
        }
    }
}