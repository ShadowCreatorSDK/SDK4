using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayAreaWaitingDrawUI : MonoBehaviour
{
    public Action OnSwitchToStationaryAreaClick
    {
        get;
        set;
    }

    public Action OnBackClick
    {
        get;
        set;
    }

    public Button switchToStationaryAreaButton;
    public Button backButton;

    public void Init()
    {
        switchToStationaryAreaButton.onClick.AddListener(()=>
        {
            OnSwitchToStationaryAreaClick?.Invoke();
        });

        backButton.onClick.AddListener(()=>
        {
            OnBackClick?.Invoke();
        });
    }

    public void Release()
    {
        switchToStationaryAreaButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }
}
