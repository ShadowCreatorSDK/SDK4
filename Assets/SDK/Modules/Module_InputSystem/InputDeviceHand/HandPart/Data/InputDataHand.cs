using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class InputDataHand: InputDataBase {

        public InputDeviceHandPart inputDeviceHandPart;
        public InputDataHand(InputDeviceHandPart inputDeviceHandPart) : base(inputDeviceHandPart) {
            this.inputDeviceHandPart = inputDeviceHandPart;
        }


        public class HandsInfo {
            /// <summary>
            /// 手原始数据
            /// </summary>
            public float[] originDataMode = new float[256];
            public float[] originDataPose = new float[128];

            public int handAmount = 0;

            /// <summary>
            /// 数据里是否存在双手
            /// </summary>
            public bool handLeftFind = false;
            public bool handRightFind = false;

            /// <summary>
            /// 数据里左右手的index
            /// </summary>
            public int handLeftIndex = 0;
            public int handRighIndex = 0;

            /// <summary>
            /// 左右手数据存储结构
            /// </summary>
            public handInfo handLeft;
            public handInfo handRight;
        }




        public bool isFound = false;//是否有数据
        public handInfo handInfo;

        public virtual void ResetHandData(InputDevicePartType type) {
            handInfo.localPosition.x = 0;
            handInfo.localPosition.y = 0;
            handInfo.localPosition.z = 0;
            if(type == InputDevicePartType.HandRight) {

                handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.One].localPosition = new Vector3(13.3f, -51.9f, 283.2f) / 1000f;
                handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Two].localPosition = new Vector3(26.3f, -58.6f, 273.4f) / 1000f;
                handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Three].localPosition = new Vector3(41.4f, -66.1f, 266.9f) / 1000f;
                handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Four].localPosition = new Vector3(92.9f, -78.0f, 260.9f) / 1000f;

                handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.One].localPosition = new Vector3(8.5f, 8.1f, 300.0f) / 1000f;
                handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Two].localPosition = new Vector3(18.4f, 4.1f, 289.7f) / 1000f;
                handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Three].localPosition = new Vector3(30.9f, -6.8f, 279.0f) / 1000f;
                handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Four].localPosition = new Vector3(51.5f, -22.4f, 267.4f) / 1000f;

                handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.One].localPosition = new Vector3(23.8f, 27.0f, 321.9f) / 1000f;
                handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.Two].localPosition = new Vector3(35.6f, 20.0f, 310.6f) / 1000f;
                handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.Three].localPosition = new Vector3(51.2f, 8.7f, 293.9f) / 1000f;
                handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.Four].localPosition = new Vector3(65.9f, -10.9f, 282.6f) / 1000f;

                handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.One].localPosition = new Vector3(42.4f, 18.1f, 331.2f) / 1000f;
                handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Two].localPosition = new Vector3(51.8f, 12.2f, 322.2f) / 1000f;
                handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Three].localPosition = new Vector3(66.0f, 3.8f, 308.3f) / 1000f;
                handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Four].localPosition = new Vector3(79.1f, -12.5f, 296.5f) / 1000f;

                handInfo.finger[(int)FINGER.small].joint[(int)JOINT.One].localPosition = new Vector3(62.2f, 2.9f, 341.0f) / 1000f;
                handInfo.finger[(int)FINGER.small].joint[(int)JOINT.Two].localPosition = new Vector3(70.2f, -2.0f, 333.9f) / 1000f;
                handInfo.finger[(int)FINGER.small].joint[(int)JOINT.Three].localPosition = new Vector3(82.0f, -10.0f, 322.7f) / 1000f;
                handInfo.finger[(int)FINGER.small].joint[(int)JOINT.Four].localPosition = new Vector3(92.1f, -23.5f, 310.2f) / 1000f;
                handInfo.finger[(int)FINGER.small].joint[(int)JOINT.Five].localPosition = new Vector3(105.6f, -73.9f, 269.9f) / 1000f;

            } else if(type == InputDevicePartType.HandLeft) {

                handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.One].localPosition = new Vector3(-56.1f, -45.2f, 297.1f) / 1000f;
                handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Two].localPosition = new Vector3(-76.5f, -49.0f, 289.2f) / 1000f;
                handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Three].localPosition = new Vector3(-95.2f, -54.3f, 283.2f) / 1000f;
                handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Four].localPosition = new Vector3(-146.3f, -60.3f, 281.8f) / 1000f;

                handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.One].localPosition = new Vector3(-49.3f, 10.4f, 326.8f) / 1000f;
                handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Two].localPosition = new Vector3(-61.6f, 9.0f, 318.4f) / 1000f;
                handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Three].localPosition = new Vector3(-79.6f, 1.5f, 307.1f) / 1000f;
                handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Four].localPosition = new Vector3(-99.5f, -8.3f, 296.9f) / 1000f;

                handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.One].localPosition = new Vector3(-62.0f, 24.6f, 352.0f) / 1000f;
                handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.Two].localPosition = new Vector3(-75.8f, 20.4f, 341.9f) / 1000f;
                handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.Three].localPosition = new Vector3(-93.5f, 13.7f, 328.5f) / 1000f;
                handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.Four].localPosition = new Vector3(-113.8f, -2.1f, 315.1f) / 1000f;

                handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.One].localPosition = new Vector3(-77.2f, 14.6f, 364.6f) / 1000f;
                handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Two].localPosition = new Vector3(-88.5f, 11.6f, 355.6f) / 1000f;
                handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Three].localPosition = new Vector3(-105.7f, 5.6f, 344.3f) / 1000f;
                handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Four].localPosition = new Vector3(-123.6f, -8.0f, 331.9f) / 1000f;

                handInfo.finger[(int)FINGER.small].joint[(int)JOINT.One].localPosition = new Vector3(-97.7f, -3.4f, 371.5f) / 1000f;
                handInfo.finger[(int)FINGER.small].joint[(int)JOINT.Two].localPosition = new Vector3(-106.5f, -5.9f, 366.0f) / 1000f;
                handInfo.finger[(int)FINGER.small].joint[(int)JOINT.Three].localPosition = new Vector3(-120.1f, -11.3f, 357.0f) / 1000f;
                handInfo.finger[(int)FINGER.small].joint[(int)JOINT.Four].localPosition = new Vector3(-134.8f, -22.3f, 344.1f) / 1000f;
                handInfo.finger[(int)FINGER.small].joint[(int)JOINT.Five].localPosition = new Vector3(-156.3f, -59.8f, 294.4f) / 1000f;
            }
        }


    }
}
