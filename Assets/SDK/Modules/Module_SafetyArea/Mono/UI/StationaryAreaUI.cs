using System;
using UnityEngine;
using UnityEngine.UI;

public class StationaryAreaUI : MonoBehaviour
{
    public Action OnSwitchToPlayAreaClick
    {
        get;
        set;
    }

    public Action OnConfirmClick
    {
        get;
        set;
    }

    public Action OnCancelClick
    {
        get;
        set;
    }

    public Button switchToPlayAreaButton;
    public Button confirmButton;
    public Button cancelButton;

    public void Init()
    {
        confirmButton.onClick.AddListener(() =>
        {
            OnConfirmClick?.Invoke();
        });

        cancelButton.onClick.AddListener(() =>
        {
            OnCancelClick?.Invoke();
        });

        switchToPlayAreaButton.onClick.AddListener(() =>
        {
            OnSwitchToPlayAreaClick?.Invoke();
        });
    }

    public void Release()
    {
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        switchToPlayAreaButton.onClick.RemoveAllListeners();
    }
}
