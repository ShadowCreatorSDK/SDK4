using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IBoundingBoxRoot
{
    void Init();
    void AddListener();
    void OnHandlerPointerDown(PointerEventData eventData);
    void OnHandlerPointerUp(PointerEventData eventData);
    void OnHandlerDrag(PointerEventData eventData);
    void OnHandlerPointerEnter(PointerEventData eventData);
    void OnHandlerPointerExit(PointerEventData eventData);
    void ReDraw();
    void SetHighLight(Transform activeHandle, bool hideOtherHandle);
    void SetVisible(bool visible);
    void SetFlattenMode();
    void SetMaterial(Material material);
}
