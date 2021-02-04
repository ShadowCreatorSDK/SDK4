using System;
using UnityEngine;
using UnityEngine.UI;

public class GroundHeightUI : MonoBehaviour
{
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

    public Action OnResetClick
    {
        get;
        set;
    }

    public Button confirmButton;
    public Button cancelButton;
    public Button resetButton;

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

        resetButton.onClick.AddListener(() =>
        {
            OnResetClick?.Invoke();
        });
    }

    public void Release()
    {
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        resetButton.onClick.RemoveAllListeners();
    }
}
