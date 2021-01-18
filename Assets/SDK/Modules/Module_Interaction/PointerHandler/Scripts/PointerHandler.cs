using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;


[AddComponentMenu("SDK/PointerHandler")]
public class PointerHandler : MonoBehaviour, IPointerHandler {

    public PointerUnityEvent PointerEnter = new PointerUnityEvent();
    public PointerUnityEvent PointerDown = new PointerUnityEvent();
    public PointerUnityEvent PointerClick = new PointerUnityEvent();
    public PointerUnityEvent PointerUp = new PointerUnityEvent();
    public PointerUnityEvent PointerExit = new PointerUnityEvent();
    public PointerUnityEvent PointerDrag = new PointerUnityEvent();


    public virtual void OnDrag(PointerEventData eventData) {
        PointerDrag?.Invoke(eventData);
    }

    public virtual void OnPointerClick(PointerEventData eventData) {
        PointerClick?.Invoke(eventData);
    }

    public virtual void OnPointerDown(PointerEventData eventData) {
        PointerDown?.Invoke(eventData);
    }

    public virtual void OnPointerEnter(PointerEventData eventData) {
        PointerEnter?.Invoke(eventData);
    }

    public virtual void OnPointerExit(PointerEventData eventData) {
        PointerExit?.Invoke(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData) {
        PointerUp?.Invoke(eventData);
    }
}

[System.Serializable]
public class PointerUnityEvent : UnityEvent<PointerEventData> { }
