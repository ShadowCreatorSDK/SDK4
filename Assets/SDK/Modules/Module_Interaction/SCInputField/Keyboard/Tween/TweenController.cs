using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenController : MonoBehaviour
{
    public List<TweenBase> mlist = new List<TweenBase>();
    void Awake()
    {
        TweenBase[] tb = GetComponents<TweenBase>();
        for (int i = 0; i < tb.Length; i++)
        {
            mlist.Add(tb[i]);
        }
    }

    public void Begin()
    {
        for (int i = 0; i < mlist.Count; i++)
        {
            if(mlist[i] == null)
            {
                Debug.LogError("tween有数组是空"  + i);
                continue;
            }
            mlist[i].Init();
            if (mlist[i].delaytime > 0)
            {
                StartCoroutine(DelayStart(mlist[i].delaytime, mlist[i]));
            }
            else
            {
                mlist[i].StartAction();
            }
            
        }
    }

    IEnumerator DelayStart(float time, TweenBase tb)
    {
        yield return new WaitForSeconds(time);
        tb.StartAction();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Begin();
        }
    }
}
