using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxHandle : BoundingBox.Handle
{

}

public class BoundingBoxRoot : AbstractBoundingBoxRoot
{
    private BoxHandle boxHandle;
    private bool isHighLight = false;
    private bool isVisible = false;

    public BoundingBoxRoot(BoundingBox boundingBox)
        : base(boundingBox)
    {
        ReCreateVisual();
    }

    public override void AddListener()
    {

    }

    public override void OnHandlerDrag(PointerEventData eventData)
    {

    }

    public override void OnHandlerPointerDown(PointerEventData eventData)
    {

    }

    public override void OnHandlerPointerEnter(PointerEventData eventData)
    {

    }

    public override void OnHandlerPointerExit(PointerEventData eventData)
    {

    }

    public override void OnHandlerPointerUp(PointerEventData eventData)
    {

    }

    public override void ReDraw()
    {
        ReCreateVisual();
        Init();
        if (isHighLight)
        {
            SetHighLight(null, true);
            return;
        }
        SetVisible(isVisible);
    }

    public override void SetFlattenMode()
    {

    }

    public override void SetHighLight(Transform activeHandle, bool hideOtherHandle)
    {
        SetVisible(true);
        if (hideOtherHandle)
        {
            SetMaterial(boundingBox.boxGrabDisplayMat);
            isHighLight = true;
        }
    }

    public override void SetVisible(bool visible)
    {
        boxHandle.SetActive(visible);
        SetMaterial(boundingBox.boxFocusDisplayMat);
        isVisible = visible;
        isHighLight = false;
    }

    public override void SetMaterial(Material material)
    {
        BoundingBoxUtils.ApplyMaterialToAllRenderers(boxHandle.root.gameObject, material);
    }

    private void ReCreateVisual()
    {
        string rootName = "BoxRoot";

        Transform existRoot = boundingBox.BoundingBoxContainer.Find(rootName);
        if (existRoot != null)
        {
            GameObject.Destroy(existRoot.gameObject);
        }

        this.boxHandle = new BoxHandle();
        this.boxHandle.root = GameObject.CreatePrimitive(boundingBox.FlattenAxis != BoundingBox.FlattenModeType.DoNotFlatten ? PrimitiveType.Quad : PrimitiveType.Cube).transform;
        GameObject.Destroy(this.boxHandle.root.GetComponent<Collider>());
        this.boxHandle.root.name = "BoxRoot";

        this.boxHandle.root.localScale = boundingBox.CurrentBoundsExtents * 2f;
        this.boxHandle.root.parent = boundingBox.BoundingBoxContainer.transform;
        this.boxHandle.root.localPosition = Vector3.zero;
        this.boxHandle.root.localRotation = Quaternion.identity;
    }
}
