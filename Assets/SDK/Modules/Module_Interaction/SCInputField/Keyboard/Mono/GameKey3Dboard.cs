using System.Collections;
using System.Collections.Generic;
using UnityEngine;   

public class GameKey3Dboard : SCKeyboardMono 
{
    private void InitTween()
    {
        TweenBase[] tb = GetComponentsInChildren<TweenBase>();
        for (int i = 0; i < tb.Length; i++)
        {
            mlist.Add(tb[i]);
        }
    }

    private List<TweenBase> mlist = new List<TweenBase>();
    private void Begin()
    {
        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }
        for (int i = 0; i < mlist.Count; i++)
        {
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
}
