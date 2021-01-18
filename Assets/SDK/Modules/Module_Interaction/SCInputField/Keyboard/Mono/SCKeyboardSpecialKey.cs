using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SpecialKeyEnum
{ 
    Done,
    Clear,
    Delete,
    Space,
    Shift,
    ShowNum,
    ShowSymbol,
    ShowEnUp,
    ShowEnLow,
    ShowCn
}

public class SCKeyboardSpecialKey : SCKeyboardKey
{
    public SpecialKeyEnum specialKeyEnum;

    private static Dictionary<SpecialKeyEnum, Action> specialCallbackDic;

    private static GameKey3Dboard game3DKayboardCache;

    protected override void OnEnable()
    {
        InitSpecialCallbackDic();
    }

    protected override void RegistKey()
    {
        PressableButton pressableButton = this.GetComponent<PressableButton>();
        TouchableButton touchableButton = this.GetComponent<TouchableButton>();
        if (pressableButton && touchableButton)
        {
            InteractionEvent interActionEvent = new InteractionEvent();
            interActionEvent.AddListener(OnSpecialKeyClick);
            pressableButton.Triggers.Add(new InteractionPressableEntry() { eventID = InteractionPressableType.PointerClick, callback = interActionEvent });
            touchableButton.Triggers.Add(new InteractionTouchableEntry() { eventID = InteractionTouchableType.PokePress, callback = interActionEvent });
        }
    }

    private void OnSpecialKeyClick(BaseEventData eventData)
    {
        if (specialCallbackDic != null && specialCallbackDic.ContainsKey(specialKeyEnum))
        {
            Action callback = specialCallbackDic[specialKeyEnum];
            callback?.Invoke();
        }
    }

    private void InitSpecialCallbackDic()
    {
        if (specialCallbackDic == null)
        {
            specialCallbackDic = new Dictionary<SpecialKeyEnum, Action>();
        }

        GameKey3Dboard gameKey3Dboard = this.GetComponentInParent<GameKey3Dboard>();
        if ( game3DKayboardCache != gameKey3Dboard)
        {
            specialCallbackDic.Clear();
            specialCallbackDic.Add(SpecialKeyEnum.Done, gameKey3Dboard.OnDoneClick);
            specialCallbackDic.Add(SpecialKeyEnum.Clear, gameKey3Dboard.OnClearKeyClick);
            specialCallbackDic.Add(SpecialKeyEnum.Delete, gameKey3Dboard.OnDeleteKeyClick);
            specialCallbackDic.Add(SpecialKeyEnum.ShowNum, gameKey3Dboard.ShowNum);
            specialCallbackDic.Add(SpecialKeyEnum.ShowSymbol, gameKey3Dboard.ShowSymbol);
            specialCallbackDic.Add(SpecialKeyEnum.ShowEnUp, gameKey3Dboard.ShowEnUp);
            specialCallbackDic.Add(SpecialKeyEnum.ShowEnLow, gameKey3Dboard.ShowEnLow);
            specialCallbackDic.Add(SpecialKeyEnum.Space, gameKey3Dboard.OnSpaceClick);
            specialCallbackDic.Add(SpecialKeyEnum.Shift, gameKey3Dboard.OnShiftClick);
            specialCallbackDic.Add(SpecialKeyEnum.ShowCn, gameKey3Dboard.ShowCN);
        }
    }
}
