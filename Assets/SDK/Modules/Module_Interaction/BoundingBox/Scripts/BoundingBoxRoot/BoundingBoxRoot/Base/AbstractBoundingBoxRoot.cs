using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AbstractBoundingBoxRoot : IBoundingBoxRoot
{
    protected BoundingBox boundingBox;

    public AbstractBoundingBoxRoot(BoundingBox boundingBox)
    {
        this.boundingBox = boundingBox;
    }

    public virtual void Init()
    {
        AddListener();
    }

    public abstract void AddListener();

    public abstract void OnHandlerPointerDown(PointerEventData eventData);

    public abstract void OnHandlerPointerUp(PointerEventData eventData);

    public abstract void OnHandlerDrag(PointerEventData eventData);

    public abstract void OnHandlerPointerEnter(PointerEventData eventData);

    public abstract void OnHandlerPointerExit(PointerEventData eventData);

    public abstract void ReDraw();

    public abstract void SetHighLight(Transform activeHandle, bool hideOtherHandle);

    public abstract void SetVisible(bool visible);

    public abstract void SetFlattenMode();

    public abstract void SetMaterial(Material material);
}
