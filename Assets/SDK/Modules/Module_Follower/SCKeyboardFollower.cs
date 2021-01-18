using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace SC.XR.Unity {
    [RequireComponent(typeof(SCKeyboardMono))]
    public class SCKeyboardFollower : FollowerBase {

        Vector3 viewPoint = Vector3.zero;

        bool isFollowing = false;
        Vector2 initViewPoint;

        protected override void OnEnable() {
            base.OnEnable();
            initViewPoint = SvrManager.Instance.leftCamera.WorldToViewportPoint(CalculateWindowPosition(SvrManager.Instance.head));
            isFollowing = true;
        }

        protected override void Follow() {

            if (SvrManager.Instance.leftCamera == null || SvrManager.Instance.head == null)
                return;

            viewPoint = SvrManager.Instance.leftCamera.WorldToViewportPoint(transform.position);

            if (viewPoint.x < (initViewPoint.x - 1f) || viewPoint.y < (initViewPoint.y - 0.5f) || viewPoint.x > (initViewPoint.x + 1f) || viewPoint.y > (initViewPoint.y + 1f) || Vector3.Magnitude(SvrManager.Instance.head.position - transform.position) > (WindowDistance+0.1f)) {
                isFollowing = true;
            } else if (Mathf.Abs(viewPoint.x - initViewPoint.x) < 0.03f && Mathf.Abs(viewPoint.y - initViewPoint.y) < 0.03f) {
                isFollowing = false;
            }

            if (isFollowing) {
                transform.position = Vector3.Lerp(transform.position, CalculateWindowPosition(SvrManager.Instance.head), WindowFollowSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, CalculateWindowRotation(SvrManager.Instance.head), WindowFollowSpeed * Time.deltaTime);
            }

        }
    }
}
