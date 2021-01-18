using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SCKeyboardKey : MonoBehaviour
{
    protected virtual void OnEnable()
    {
    }

    protected virtual void Start()
    {
        RegistKey();
    }

    protected virtual void RegistKey()
    {
        PressableButton pressableButton = this.GetComponent<PressableButton>();
        TouchableButton touchableButton = this.GetComponent<TouchableButton>();
        if (pressableButton && touchableButton)
        {
            InteractionEvent interActionEvent = new InteractionEvent();
            interActionEvent.AddListener(OnNormalKeyClick);
            pressableButton.Triggers.Add(new InteractionPressableEntry() { eventID = InteractionPressableType.PointerClick, callback = interActionEvent });
            touchableButton.Triggers.Add(new InteractionTouchableEntry() { eventID = InteractionTouchableType.PokePress, callback = interActionEvent });
        }
    }

    protected virtual void OnNormalKeyClick(BaseEventData eventData)
    {
        TextMesh textMesh = this.GetComponentInChildren<TextMesh>();
        GameKey3Dboard gameKey3Dboard = this.GetComponentInParent<GameKey3Dboard>();
        gameKey3Dboard.OnNormalKeyClick(textMesh.text);
    }
}
