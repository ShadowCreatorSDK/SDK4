using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace SC.XR.Unity
{
  
    [RequireComponent(typeof(CameraFollower))]
    public class Module_Notice : MonoBehaviour
    {
        private static Module_Notice instance;
        public static Module_Notice getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = Instantiate(Resources.Load<GameObject>("prefabs/Module_Notice")).GetComponent<Module_Notice>();
                }
                return instance;
            }
        }

        public string _mainText = "";
        public string _minorText = "";
        public FollowType _isFollow = FollowType.False;
        [Range(0.1f, 10.0f)]
        public float _distance = 1.2f;
        public float _durationTime = 3f;
        public NoticeType _type = NoticeType.None;
        public AlignmentType _anchorType = AlignmentType.Center;
      
        private List<Image> _imageList;
        public List<Image> ImageList
        {
            get
            {
                if (_imageList == null)
                {
                    _imageList = new List<Image>(GetComponentsInChildren<Image>());
                }
                return _imageList;
            }
        }

        private List<TextMeshProUGUI> _textMeshProUGUIList;
        public List<TextMeshProUGUI> TextMeshProUGUIList
        {
            get
            {
                if (_textMeshProUGUIList == null)
                {
                    _textMeshProUGUIList = new List<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>());
                }
                return _textMeshProUGUIList;
            }
        }

        private CameraFollower _follower;
        public CameraFollower _Follower
        {
            get
            {
                if (_follower == null)
                {
                    _follower = GetComponentInChildren<CameraFollower>();
                }
                return _follower;
            }
        }

        private NoticeEffect _effect;
        public NoticeEffect _Effect
        {
            get
            {
                if (_effect == null)
                {
                    _effect = GetComponentInChildren<NoticeEffect>();
                }
                return _effect;
            }
        }

        private void Init()
        {
            if (_textMeshProUGUIList == null)
            {
                _textMeshProUGUIList = new List<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>());
            }
            if (_imageList == null)
            {
                _imageList = new List<Image>(GetComponentsInChildren<Image>());
            }
            if (_follower == null)
            {
                _follower = GetComponentInChildren<CameraFollower>();
            }
            if (_effect == null)
            {
                _effect = GetComponentInChildren<NoticeEffect>();
            }
        }
        public void RefreshInfo()
        {
            Init();
            foreach (var item in TextMeshProUGUIList)
            {
#if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(item, " modify Property1");
#endif
            }
            foreach (var item in ImageList)
            {
#if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(item, " modify Property2");
#endif
            }
            SetMainText(_mainText);
            SetSubText(_minorText);
            SetIsFollow(_isFollow);
            SetDistance(_distance);
            SetDurationTime(_durationTime);
            SetIconTip(_type);
        }





        public void SetNoticeInfo(string mainString, string subString, NoticeType type = NoticeType.Warning, float distance = 0.8f, AlignmentType _anchorType = AlignmentType.Center, FollowType isFollower = FollowType.True)
        {
            SetMainText(mainString);
            SetSubText(subString);
            SetIsFollow(isFollower);
            SetDistance(distance);
            SetIconTip(type);
            SetTextAnchor(_anchorType);
        }
        public void SetNoticeInfo(string mainString, string subString)
        {
            SetMainText(mainString);
            SetSubText(subString);          
        }
        private void OnEnable()
        {
            // StartNotice();
            foreach (var item in ImageList)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, 0);
            }
            foreach (var item in TextMeshProUGUIList)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, 0);
            }
        }
        public void StartNotice()
        {
            RefreshInfo();
            if (_Effect != null)
            {
                _Effect.enabled = true;
            }
        }

        public void StartNotice(float time)
        {
            SetDurationTime(time);
            if (_Effect != null && !_Effect.enabled)
            {
                _Effect.enabled = true;
            }
        }
        public void StopNotice()
        {
            if (_Effect != null && _Effect.enabled)
            {
                _Effect.enabled = false;
            }
        }
        private void OnDisable()
        {
            _textMeshProUGUIList = null;
            _follower = null;
            _effect = null;
        }
        private void SetTextAnchor(AlignmentType anchorType )
        {
            foreach (var item in TextMeshProUGUIList)
            {
                switch (anchorType)
                {                   
                    case AlignmentType.Left:
                        item.alignment = TextAlignmentOptions.Left;
                        break;
                    case AlignmentType.Center:
                        item.alignment = TextAlignmentOptions.Center;
                        break;
                    case AlignmentType.Right:
                        item.alignment = TextAlignmentOptions.Right;
                        break;
                    default:
                        break;
                }
            }
        }
        private void SetMainText(string str)
        {
            foreach (var item in TextMeshProUGUIList)
            {
                if (item.gameObject.name == "MainTip" && str != null)
                {
                    item.text = str;

                }
            }

        }
        private void SetSubText(string str)
        {
            foreach (var item in TextMeshProUGUIList)
            {
                if (item.gameObject.name == "MinorTip" && str != null)
                {
                    item.text = str;
                }
            }
        }
        private void SetIsFollow(FollowType isFollow)
        {
            if (isFollow== FollowType.True)
            {
                if (_Follower?.enabled == false)
                {
                    _Follower.enabled = true;
                }
            }
            else if(isFollow == FollowType.False)
            {
                if (_Follower?.enabled == true)
                {
                    _Follower.enabled = false;
                }
            }
        }
        private void SetDistance(float distance)
        {
            if (_Follower != null)
            {
                _Follower.WindowDistance = distance;
            }
        }
        private void SetDurationTime(float time)
        {
            if (_Effect != null)
            {
                _Effect.effectDurtion = time;
            }

        }
        private void SetIconTip(NoticeType type)
        {
            foreach (var item in ImageList)
            {
                if (item.gameObject.name == "IconTip")
                {
                    switch (type)
                    {
                        case NoticeType.None:
                            item.enabled = false;
                            break;
                        case NoticeType.Warning:
                            item.enabled = true;
                            item.color =new Color(1,0,0,0);
                            item.sprite = Resources.Load<Sprite>("Sprites/warning");
                            break;
                        case NoticeType.Normal:
                            item.enabled = true;
                            item.color = new Color(1, 1, 1, 0);
                            item.sprite = Resources.Load<Sprite>("Sprites/Normal");
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        List<string> mainstrs = new List<string>();
        List<string> substrs = new List<string>();
        Coroutine multiple;
        public void AddStrsList(string mainstr,string substr)
        {
            mainstrs.Add(mainstr);
            substrs.Add(substr);
        }
        public  void StartMultipleNotice(float time)
        {
            if (mainstrs != null && substrs !=null)
            {
                multiple = StartCoroutine(MultipleNotice(time, mainstrs, substrs));
            }
           
        }
        public  void StopMultipleNotice()
        {
            if (multiple !=null)
            {
                mainstrs.Clear();
                substrs.Clear();
                StopCoroutine(multiple);
            }
        }
        IEnumerator MultipleNotice(float time,List<string> mainstrs,List<string> substrs)
        {
            int count =Mathf.Min(mainstrs.Count, substrs.Count);
            for (int i = 0; i < count; i++)
            {
                yield return null;
                SetMainText(mainstrs[i]);
                SetSubText(substrs[i]);
                StartNotice(time);                           
                yield return new WaitWhile(()=>_Effect.isEnable);
            }
            yield break;
        }

     
    }
}
