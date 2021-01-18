using LitJson;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace SC.XR.Unity {
    /// <summary>
    /// 一个WebRequest的基类
    /// </summary>
    public abstract class WebRequestBase {
        public string url;
        public WWWForm wwwForm;
        public WebRequestType webRequestType;
        protected JsonData responseJsonData;

        protected Action<JsonData> mSuccess;
        protected Action<JsonData> mFailed;
        protected Action<JsonData> mErrorCallBack;


        public enum WebRequestType {
            GET,
            POST,
            PUT,
        }

        public string LOGTAG = "[WebRequestBase]:";

        public WebRequestBase(string _url, WWWForm _form, Action<JsonData> success, Action<JsonData> failed, Action<JsonData> errorCallBack = null) {
            url = _url;
            wwwForm = _form;
            mSuccess = success;
            mFailed = failed;
            mErrorCallBack = errorCallBack;
        }

        //public abstract void Success(JsonData responseJson);
        //public abstract void Failed(JsonData responseJson);

        public virtual void NetWorkError(string errText) {
            Debug.Log(LOGTAG + "NetWorkError");
            //try {
            //    UISystem.Instant.PushUIPanel(UIPanelsType.InfoType1Panel, "ERROR: NetWorkError,Please Open Wifi");
            //} catch (Exception e) {
            //    Debug.Log(e);
            //}
        }


        public virtual IEnumerator SendRequest() {

            if(webRequestType == WebRequestType.GET) {
                UnityWebRequest www = UnityWebRequest.Get(url);
                yield return www.SendWebRequest();
                if(www.isNetworkError || www.isHttpError) {
                    Debug.Log(www.error);
                    //responseJsonData = JsonMapper.ToObject(www.error);
                    NetWorkError(www.error);
                } else {
                    Debug.Log(www.downloadHandler.text);
                    responseJsonData = JsonMapper.ToObject(www.downloadHandler.text);
                    if(mSuccess != null) {
                        mSuccess(responseJsonData);
                    }

                }

            } else if(webRequestType == WebRequestType.POST) {
                try {
                    //if (UserSystem.Instant.SysInfo.UserToken != null) {
                    //    wwwForm.AddField("token", UserSystem.Instant.SysInfo.UserToken);
                    //    Debug.Log("[" + GetType().ToString() + "]: " + "Form Add UserToken !");
                    //}
                } catch(Exception e) {
                    Debug.Log(e + "\nUserSystem.Instant.SysInfo.UserToken is Null");
                }

                Debug.Log("[" + GetType().ToString() + "]: " + "SendRequest:" + "URL:" + url + "\nFormData:" + System.Text.Encoding.Default.GetString(wwwForm.data));
                UnityWebRequest www = UnityWebRequest.Post(url, wwwForm);
                yield return www.SendWebRequest();
                if(www.isNetworkError || www.isHttpError) {
                    Debug.Log(www.error);
                    //responseJsonData = JsonMapper.ToObject(www.error);
                    if(mErrorCallBack != null) {
                        mErrorCallBack(www.error);
                    }
                    NetWorkError(www.error);
                } else {
                    Debug.Log("[" + GetType().ToString() + "]: \n" + www.downloadHandler.text);
                    try {
                        responseJsonData = JsonMapper.ToObject(www.downloadHandler.text);
                        if("200" == responseJsonData["code"].ToString()) {
                            if(mSuccess != null) {
                                mSuccess(responseJsonData);
                            }
                        } else if("-1" == responseJsonData["code"].ToString()) {

                            try {
                                if(responseJsonData["msg"].ToString() == "验证失败，请重新获取系统token") {
                                    // UserSystem.Instant.SysInfo.ResetUserInfo();
                                }
                            } catch(Exception e) {
                                Debug.Log(e);
                            }


                            if(mFailed != null) {
                                mFailed(responseJsonData);
                            }
                        } else {
                            if(mFailed != null) {
                                mFailed(responseJsonData);
                            }
                        }
                    } catch(Exception e) {
                        Debug.Log(e);

                    }


                }
            }
        }

    }

}