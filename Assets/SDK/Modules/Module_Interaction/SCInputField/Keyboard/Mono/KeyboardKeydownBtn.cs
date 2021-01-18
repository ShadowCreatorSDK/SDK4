using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class KeyboardKeydownBtn : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector]
    public UnityEvent onClick = new UnityEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}
