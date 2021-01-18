using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand
{
    public class EffectHandModel : AbstractHandModel {
        
        [Header("Effect")]
        [SerializeField]
        private Renderer mRenderer;

        [SerializeField]
        private Material mEffectMaterial;

        [SerializeField]
        private Material mNomalMaterial;

        [SerializeField]
        private Material mNomalMaterialNoAlpha;

        public float EffectDuration = 2.4f;
        private const float mEffectTime = 1.2f;

        private float time = 0;
        Coroutine mEffectCoroutine;
        bool EffectFinish = false;

        [Header("Hand Model Transparent")]
        public bool IsTransparent = false;

        public override HandModelType handModelType => HandModelType.EffectHand;

        public override void OnSCAwake() {
            base.OnSCAwake();
            if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "GGT26DofHandEffectDuration")) {
                try {
                    EffectDuration = API_Module_SDKConfiguration.GetFloat("Module_InputSystem", "GGT26DofHandEffectDuration", 0);
                    DebugMy.Log("EffectDuration From SDKConfig:" + EffectDuration, this, true);
                } catch (Exception e) {
                    Debug.Log(e);
                }
            }
        }

        protected virtual void OnEnable() {
            if (!EffectFinish && mEffectMaterial && mRenderer && mNomalMaterial && mNomalMaterialNoAlpha) {
                mEffectCoroutine = StartCoroutine(TriggerEffect(EffectDuration));
            }
        }

        protected virtual void OnDisable() {
            if (mEffectCoroutine != null) {
                StopCoroutine(mEffectCoroutine);
            }
        }

        protected virtual void OnDestroy() {
            mEffectMaterial.SetFloat("_scTime", mEffectTime);
            mNomalMaterial.SetFloat("_Alpha", 0f);
        }

        IEnumerator TriggerEffect(float durationTime) {

            time = 0;
            mRenderer.sharedMaterial = mEffectMaterial;
            while (time < mEffectTime) {
                time += Time.deltaTime * mEffectTime / durationTime;
                mEffectMaterial.SetFloat("_scTime", time);
                yield return null;
            }
            mEffectMaterial.SetFloat("_scTime", mEffectTime);

            if ( ! IsTransparent) {
                time = 0;
                mNomalMaterial.SetFloat("_Alpha", time);
                mRenderer.sharedMaterial = mNomalMaterial;
                while (time <= 1) {
                    time += Time.deltaTime * 1.5f;
                    mNomalMaterial.SetFloat("_Alpha", time);
                    yield return null;
                }
                mRenderer.sharedMaterial = mNomalMaterialNoAlpha;
            }
            
            EffectFinish = true;
        }


        public override void UpdateTransform() {
            UpdateJointTransform();
        }

        public override Transform GetJointTransform(FINGER finger,JOINT joint) {

            if(finger == FINGER.small) {
                if(joint == JOINT.One) {
                    return fingerUI[(int)finger].jointGameObject[4].transform;
                } else if(joint == JOINT.Two) {
                    return fingerUI[(int)finger].jointGameObject[0].transform;
                } else if(joint == JOINT.Three) {
                    return fingerUI[(int)finger].jointGameObject[1].transform;
                } else if(joint == JOINT.Four) {
                    return fingerUI[(int)finger].jointGameObject[2].transform;
                } else if(joint == JOINT.Five) {
                    return fingerUI[(int)finger].jointGameObject[3].transform;
                }
            } else{
                if(joint == JOINT.One) {
                    return fingerUI[(int)finger].jointGameObject[3].transform;
                } else if(joint == JOINT.Two) {
                    return fingerUI[(int)finger].jointGameObject[0].transform;
                } else if(joint == JOINT.Three) {
                    return fingerUI[(int)finger].jointGameObject[1].transform;
                } else if(joint == JOINT.Four) {
                    return fingerUI[(int)finger].jointGameObject[2].transform;
                }
            }

            return null;
        }

        protected virtual void UpdateJointTransform() {
            Vector3 thumbFourPosition = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Four].localPosition;
            Vector3 indexFourPosition = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Four].localPosition;
            Vector3 pinkyFourPosition = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.small].joint[(int)JOINT.Four].localPosition;

            Vector3 palmForward = indexFourPosition - thumbFourPosition;
            Vector3 thumbToPinky = pinkyFourPosition - thumbFourPosition;
            Vector3 palmUp = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.PartType == InputDevicePartType.HandLeft ? Vector3.Cross(thumbToPinky, palmForward) : Vector3.Cross(palmForward, thumbToPinky);

            Quaternion palmRotation = Quaternion.LookRotation(palmForward, palmUp);

            if (modelHand.inputDeviceHandPartUI.inputDeviceHandPart.PartType == InputDevicePartType.HandLeft)
            {
                handJointContainer.transform.localPosition = thumbFourPosition - palmRotation * Vector3.forward * 0.04f - palmRotation * Vector3.right * 0.02f;
            }
            else
            {
                handJointContainer.transform.localPosition = thumbFourPosition - palmRotation * Vector3.forward * 0.04f + palmRotation * Vector3.right * 0.02f;
            }
            handJointContainer.transform.localRotation = palmRotation;

            Quaternion pinkyPreviousJointRotation = palmRotation * (modelHand.inputDeviceHandPartUI.inputDeviceHandPart.PartType == InputDevicePartType.HandLeft ? Quaternion.Euler(5.369f, -15.385f, 23.418f) : Quaternion.Euler(5.369f, 15.385f, -23.418f));

            //pinky
            for(int jointIdx = 2; jointIdx >= 0; --jointIdx) {
                Quaternion JointLocalRotation = Quaternion.identity;
                JointLocalRotation = Quaternion.Inverse(pinkyPreviousJointRotation) * modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.small].joint[jointIdx + 1].localRotation;
                pinkyPreviousJointRotation = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.small].joint[jointIdx + 1].localRotation;

                fingerUI[(int)FINGER.small].jointGameObject[jointIdx].transform.localRotation = JointLocalRotation;
            }

            Quaternion ringPreviousJointRotation = palmRotation;
            //ring
            for(int jointIdx = 2; jointIdx >= 0; --jointIdx) {
                Quaternion JointLocalRotation = Quaternion.identity;
                JointLocalRotation = Quaternion.Inverse(ringPreviousJointRotation) * modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.ring].joint[jointIdx + 1].localRotation;
                ringPreviousJointRotation = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.ring].joint[jointIdx + 1].localRotation;

                fingerUI[(int)FINGER.ring].jointGameObject[jointIdx].transform.localRotation = JointLocalRotation;
            }

            Quaternion middlePreviousJointRotation = palmRotation;
            //middle
            for(int jointIdx = 2; jointIdx >= 0; --jointIdx) {
                Quaternion JointLocalRotation = Quaternion.identity;
                JointLocalRotation = Quaternion.Inverse(middlePreviousJointRotation) * modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.middle].joint[jointIdx + 1].localRotation;
                middlePreviousJointRotation = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.middle].joint[jointIdx + 1].localRotation;

                fingerUI[(int)FINGER.middle].jointGameObject[jointIdx].transform.localRotation = JointLocalRotation;
            }

            Quaternion indexPreviousJointRotation = palmRotation;
            //index
            for(int jointIdx = 2; jointIdx >= 0; --jointIdx) {
                Quaternion JointLocalRotation = Quaternion.identity;
                JointLocalRotation = Quaternion.Inverse(indexPreviousJointRotation) * modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.forefinger].joint[jointIdx + 1].localRotation;
                indexPreviousJointRotation = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.forefinger].joint[jointIdx + 1].localRotation;

                fingerUI[(int)FINGER.forefinger].jointGameObject[jointIdx].transform.localRotation = JointLocalRotation;
            }

            Quaternion thumbPreviousJointRotation = palmRotation;
            //thumb
            for(int jointIdx = 2; jointIdx >= 0; --jointIdx) {
                Quaternion JointLocalRotation = Quaternion.identity;
                JointLocalRotation = Quaternion.Inverse(thumbPreviousJointRotation) * modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.thumb].joint[jointIdx + 1].localRotation;
                thumbPreviousJointRotation = modelHand.inputDeviceHandPartUI.inputDeviceHandPart.inputDataHand.handInfo.finger[(int)FINGER.thumb].joint[jointIdx + 1].localRotation;

                fingerUI[(int)FINGER.thumb].jointGameObject[jointIdx].transform.localRotation = JointLocalRotation;
            }


        }
    }
}