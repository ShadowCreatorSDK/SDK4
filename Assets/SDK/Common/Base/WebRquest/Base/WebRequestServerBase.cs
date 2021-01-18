using System.Collections;
using System.Collections.Generic;

namespace SC.XR.Unity {
    /// <summary>
    /// 一个WebRequest发送系统的基类
    /// </summary>
    public class WebRequestServerBase : SCModuleMono {

        protected List<WebRequestBase> webRequestList = new List<WebRequestBase>();
        public void AddWebRequest(WebRequestBase webRequest) {
            webRequestList.Add(webRequest);
            CheckQueue();
        }

        void CheckQueue() {
            if(webRequestList.Count == 0) {
                return;
            }
            StartCoroutine(SendRequest());
        }

        IEnumerator SendRequest() {
            ///加yield return null目的是将Web请求延迟到下一帧开始，避免web回复委托还没添加，web已经回复，导致错过信息
            yield return null;

            WebRequestBase webRequest = webRequestList[0];
            webRequestList.RemoveAt(0);
            yield return StartCoroutine(webRequest.SendRequest());
            CheckQueue();
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            StopAllCoroutines();
            webRequestList.Clear();
        }

    }
}