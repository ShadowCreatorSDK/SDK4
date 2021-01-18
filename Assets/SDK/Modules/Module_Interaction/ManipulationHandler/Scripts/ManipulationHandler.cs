using SC.XR.Unity.Module_InputSystem.InputDeviceGC;
using SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof;
using SC.XR.Unity.Module_InputSystem.InputDeviceHand;
using SC.XR.Unity.Module_InputSystem.InputDeviceHead;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem
{

    public struct SCPose
    {
        public Vector3 position;
        public Quaternion rotation;

        public SCPose(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    [RequireComponent(typeof(NearInterationGrabbable))]
    [RequireComponent(typeof(BoxCollider))]
    public class ManipulationHandler : PointerHandler
    {

        public Transform Target;

        [SerializeField]
        protected SCAudiosConfig.AudioType StartAudio = SCAudiosConfig.AudioType.Move_Start;
        [SerializeField]
        protected SCAudiosConfig.AudioType EndAudio = SCAudiosConfig.AudioType.Move_End;

        protected Vector3 TargetRelativeTo;
        protected SCPointEventData sCPointEventData;

        protected Matrix4x4 matrix;

        private Rigidbody rigidBody;
        private bool wasKinematic = false;

        private IDevicePartManipulation devicePartManipulation;
        private IDevicePartCountManipulation devicePartCountManipulation;
        private Dictionary<InputDevicePartType, SCPointEventData> eventDataDic;

        private ScaleLogic scaleLogic;
        private RotateLogic rotateLogic;
        private MoveLogic moveLogic;

        private Vector3 targetStartScale;

        public bool canOneHandRotate = false;
        public bool canTwoHandRotate = false;
        public bool canTwoHandScale = false;
        public float minScaleRatio = 0.8f;
        public float maxScaleRatio = 3f;

        public virtual void Start()
        {
            targetStartScale = Target == null ? transform.localScale : Target.localScale;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (eventDataDic == null)
            {
                eventDataDic = new Dictionary<InputDevicePartType, SCPointEventData>();
            }

            if (scaleLogic == null)
            {
                scaleLogic = new ScaleLogic();
            }

            if (rotateLogic == null)
            {
                rotateLogic = new RotateLogic();
            }

            if (moveLogic == null)
            {
                moveLogic = new MoveLogic();
            }

            if (eventData is SCPointEventData)
            {
                SCPointEventData scPointEventData = eventData as SCPointEventData;
                eventDataDic[scPointEventData.inputDevicePartBase.PartType] = scPointEventData;
                if (scPointEventData.inputDevicePartBase is InputDeviceHandPart)
                {
                    devicePartManipulation = new HandDevicePartManipulation();
                }
                else if (scPointEventData.inputDevicePartBase is InputDeviceGCPart)
                {
                    devicePartManipulation = new GameControllerDevicePartManipulation();
                }
                else if (scPointEventData.inputDevicePartBase is InputDeviceHeadPart)
                {
                    devicePartManipulation = new HeadDevicePartManipulation();
                }
            }

            //Two DevicePart
            if (eventDataDic.Count > 1)
            {
                devicePartCountManipulation = new TwoDevicePartCountManipulation();
                devicePartCountManipulation.Init(devicePartManipulation, eventDataDic, Target == null ? this.transform : Target.transform, moveLogic, canTwoHandRotate ? rotateLogic : null, canTwoHandScale ? scaleLogic : null);
            }
            //One DevicePart
            else if (eventDataDic.Count == 1)
            {
                devicePartCountManipulation = new OneDevicePartCountManipulation();
                devicePartCountManipulation.Init(devicePartManipulation, eventDataDic, Target == null ? this.transform : Target.transform, moveLogic, canOneHandRotate ? rotateLogic : null, null);
            }

            rigidBody = Target == null ? this.GetComponent<Rigidbody>() : Target.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                wasKinematic = rigidBody.isKinematic;
                rigidBody.isKinematic = true;
            }

            AudioSystem.getInstance.PlayAudioOneShot(gameObject, StartAudio);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (eventData is SCPointEventData)
            {
                SCPointEventData scPointEventData = eventData as SCPointEventData;
                if (eventDataDic.ContainsKey(scPointEventData.inputDevicePartBase.PartType))
                {
                    eventDataDic.Remove(scPointEventData.inputDevicePartBase.PartType);
                }

                //from two hand to one hand
                if (eventDataDic.Count == 1)
                {
                    devicePartCountManipulation = new OneDevicePartCountManipulation();
                    devicePartCountManipulation.Init(devicePartManipulation, eventDataDic, Target == null ? this.transform : Target.transform, moveLogic, canOneHandRotate ? rotateLogic : null, null);
                }
            }

            if (rigidBody != null)
            {
                rigidBody.isKinematic = wasKinematic;
                rigidBody = null;
            }
            AudioSystem.getInstance.PlayAudioOneShot(gameObject, EndAudio);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            sCPointEventData = eventData as SCPointEventData;
            if (sCPointEventData == null)
            {
                return;
            }

            Tuple<Vector3, Quaternion, Vector3> result = devicePartCountManipulation.Update(CaculateScaleConstraint);
            Vector3 position = result.Item1;
            Quaternion rotation = result.Item2;
            Vector3 scale = result.Item3;

            if (Target)
            {
                Target.position = position;
                Target.rotation = rotation;
                Target.localScale = scale;
            }
            else
            {
                transform.position = position;
                transform.rotation = rotation;
                transform.localScale = scale;
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }

        private Vector3 CaculateScaleConstraint(Vector3 toScale)
        {
            Vector3 minimumScale = targetStartScale * minScaleRatio;
            Vector3 maximumScale = targetStartScale * maxScaleRatio;

            if (Vector3.Min(maximumScale, toScale) != toScale)
            {
                float maxRatio = 0.0f;
                int maxIdx = -1;

                for (int i = 0; i < 3; i++)
                {
                    if (maximumScale[i] > 0)
                    {
                        float ratio = toScale[i] / maximumScale[i];
                        if (ratio > maxRatio)
                        {
                            maxRatio = ratio;
                            maxIdx = i;
                        }
                    }
                }

                if (maxIdx != -1)
                {
                    toScale /= maxRatio;
                }
            }

            if (Vector3.Max(minimumScale, toScale) != toScale)
            {
                float minRatio = 1.0f;
                int minIdx = -1;

                // Find out the component with the minimum ratio to its minimum allowed value
                for (int i = 0; i < 3; ++i)
                {
                    if (minimumScale[i] > 0)
                    {
                        float ratio = toScale[i] / minimumScale[i];
                        if (ratio < minRatio)
                        {
                            minRatio = ratio;
                            minIdx = i;
                        }
                    }
                }

                if (minIdx != -1)
                {
                    toScale /= minRatio;
                }
            }
            return toScale;
        }
    }
}