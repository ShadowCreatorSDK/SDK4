using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPlayAreaUI : MonoBehaviour
{
    public Action OnConfirmClick
    {
        get;
        set;
    }

    public Action OnBackClick
    {
        get;
        set;
    }

    public Button confirmClick;
    public Button backClick;

    public void Init()
    {
        confirmClick.onClick.AddListener(() =>
        {
            OnConfirmClick?.Invoke();
        });

        backClick.onClick.AddListener(()=>
        {
            OnBackClick?.Invoke();
        });
    }

    public void Release()
    {
        confirmClick.onClick.RemoveAllListeners();
        backClick.onClick.RemoveAllListeners();
    }
}
