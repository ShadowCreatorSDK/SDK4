using SC.XR.Unity;
using UnityEngine;

public class ChineseIMEManager : Singleton<ChineseIMEManager>
{
    private AndroidJavaObject unityIME;

    public ChineseIMEManager()
    {
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        unityIME = new AndroidJavaObject("com.invision.pinyinime.UnityIME");
        unityIME.Call("OpenIME", unityActivity);
    }

    ~ChineseIMEManager()
    {
        unityIME.Call("CloseIME");
    }

    public int GetWordCount(string englishWord)
    {
#if UNITY_EDITOR
        return 8;
#endif
        int wordCount = unityIME.Call<int>("SearchWord", englishWord);
        return wordCount;
    }

    public string GetWord(int i)
    {
#if UNITY_EDITOR
        return "测试";
#endif
        return unityIME.Call<string>("GetWord", i);
    }
}