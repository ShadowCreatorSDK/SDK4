using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity {
    public class CameraFollower : FollowerBase {

        bool isInit = false;

        protected override void OnEnable() {
            base.OnEnable(); 
            isInit = false;
        }

        protected override void Follow() {
            if (SvrManager.Instance?.head == null) {
                return;
            }

            if (isInit == false) {
                transform.position = CalculateWindowPosition(SvrManager.Instance.head);
                transform.rotation = CalculateWindowRotation(SvrManager.Instance.head);
                isInit = true;
            }

            transform.position = Vector3.Lerp(transform.position, CalculateWindowPosition(SvrManager.Instance.head), WindowFollowSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, CalculateWindowRotation(SvrManager.Instance.head), WindowFollowSpeed * Time.deltaTime);
        }
    }
}
