using SC.XR.Unity.Module_InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SideHandle : BoundingBox.Handle
{
    public BoundingBox.AxisType axis;
}


public class SideBoundingBoxRoot : AbstractBoundingBoxRoot
{
    private const int SIDE_NUM = 12;
    private SideHandle[] handles;
    private int[] disActiveList = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

    private bool isHighLight = false;
    private bool isHideOtherHandle = false;
    private bool isVisible = false;
    private Transform currentActiveHandle = null;

    private Vector3 preDragObjPosition;
    private Vector3 currDragObjPosition;
    private Vector3 currentRotationAxis;

    public SideBoundingBoxRoot(BoundingBox boundingBox) : base(boundingBox)
    {
        ReCreateVisual();
    }

    public override void AddListener()
    {
        PointerHandler pointerHandler;
        foreach (var handle in handles)
        {
            pointerHandler = handle.root.gameObject.GetComponent<PointerHandler>();
            if (pointerHandler == null)
            {
                pointerHandler = handle.root.gameObject.AddComponent<PointerHandler>();
            }

            pointerHandler.PointerDown.RemoveAllListeners();
            pointerHandler.PointerDown.AddListener(OnHandlerPointerDown);

            pointerHandler.PointerDrag.RemoveAllListeners();
            pointerHandler.PointerDrag.AddListener(OnHandlerDrag);

            pointerHandler.PointerUp.RemoveAllListeners();
            pointerHandler.PointerUp.AddListener(OnHandlerPointerUp);

            pointerHandler.PointerEnter.RemoveAllListeners();
            pointerHandler.PointerEnter.AddListener(OnHandlerPointerEnter);

            pointerHandler.PointerExit.RemoveAllListeners();
            pointerHandler.PointerExit.AddListener(OnHandlerPointerExit);
        }
    }

    public override void OnHandlerDrag(PointerEventData eventData)
    {
        SCPointEventData scPointEventData = eventData as SCPointEventData;
        if (scPointEventData == null)
        {
            return;
        }


        preDragObjPosition = currDragObjPosition;
        currDragObjPosition = Vector3.Lerp(currDragObjPosition, scPointEventData.Position3D, 0.5f);

        Vector3 prevDir = Vector3.ProjectOnPlane(preDragObjPosition - scPointEventData.inputDevicePartBase.detectorBase.pointerBase.transform.position, currentRotationAxis).normalized;
        Vector3 currentDir = Vector3.ProjectOnPlane(currDragObjPosition - scPointEventData.inputDevicePartBase.detectorBase.pointerBase.transform.position, currentRotationAxis).normalized;
        Quaternion q = Quaternion.FromToRotation(prevDir, currentDir);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        boundingBox.transform.RotateAround(boundingBox.BoundingBoxContainer.transform.position, axis, -3 * angle);

        //    Vector3 prevDir = Vector3.ProjectOnPlane(preDragObjPosition - boundingBox.BoundingBoxContainer.transform.position, currentRotationAxis).normalized;
        //    Vector3 currentDir = Vector3.ProjectOnPlane(currDragObjPosition - boundingBox.BoundingBoxContainer.transform.position, currentRotationAxis).normalized;
        //    Quaternion q = Quaternion.FromToRotation(prevDir, currentDir);
        //    q.ToAngleAxis(out float angle, out Vector3 axis);
        //    boundingBox.transform.RotateAround(boundingBox.BoundingBoxContainer.transform.position, axis, angle);

    }

    public override void OnHandlerPointerDown(PointerEventData eventData)
    {
        SCPointEventData scPointEventData = eventData as SCPointEventData;
        if (scPointEventData == null)
        {
            return;
        }

        currDragObjPosition = scPointEventData.Position3D;
        currentRotationAxis = GetRotationAxis(scPointEventData.pointerCurrentRaycast.gameObject.transform);

        AudioSystem.getInstance.PlayAudioOneShot(boundingBox.gameObject, boundingBox.RotateStartAudio);
        boundingBox.RotateStarted?.Invoke();
        boundingBox.SetHighLight(eventData.pointerCurrentRaycast.gameObject?.transform, true);
    }

    public override void OnHandlerPointerUp(PointerEventData eventData)
    {
        AudioSystem.getInstance.PlayAudioOneShot(boundingBox.gameObject, boundingBox.RotateStopAudio);
        boundingBox.RotateStopped?.Invoke();
        boundingBox.SetHighLight(eventData.pointerCurrentRaycast.gameObject?.transform, false);
    }

    public override void OnHandlerPointerEnter(PointerEventData eventData)
    {
        SCPointEventData scPointEventData = eventData as SCPointEventData;
        if (scPointEventData == null)
        {
            return;
        }

        if (scPointEventData.DownPressGameObject == null)
        {
            SetHighLight(eventData.pointerCurrentRaycast.gameObject.transform, false);
        }
    }

    public override void OnHandlerPointerExit(PointerEventData eventData)
    {
        SCPointEventData scPointEventData = eventData as SCPointEventData;
        if (scPointEventData == null)
        {
            return;
        }

        if (scPointEventData.DownPressGameObject == null)
        {
            SetVisible(true);
        }
    }

    public override void ReDraw()
    {
        ReCreateVisual();
        Init();
        if (isHighLight)
        {
            SetHighLight(currentActiveHandle, isHideOtherHandle);
            return;
        }
        SetVisible(isVisible);
    }

    public override void SetHighLight(Transform activeHandle, bool hideOtherHandle)
    {
        SetVisible(true);
        isHighLight = true;
        isHideOtherHandle = hideOtherHandle;
        this.currentActiveHandle = null;
        foreach (var handle in handles)
        {
            if (handle.root.transform != activeHandle)
            {
                if (hideOtherHandle)
                {
                    handle.SetActive(false);
                }
            }
            else
            {
                this.currentActiveHandle = activeHandle;
                BoundingBoxUtils.ApplyMaterialToAllRenderers(handle.root.gameObject, boundingBox.HandleGrabMaterial);
            }
        }
    }

    public override void SetVisible(bool visible)
    {
        SetMaterial(boundingBox.HandleMaterial);

        isVisible = visible;
        isHighLight = false;

        foreach (var handle in handles)
        {
            handle.SetActive(visible);
        }

        SetActiveAxis(BoundingBox.AxisType.X, boundingBox.ShowRotationHandleForX);
        SetActiveAxis(BoundingBox.AxisType.Y, boundingBox.ShowRotationHandleForY);
        SetActiveAxis(BoundingBox.AxisType.Z, boundingBox.ShowRotationHandleForZ);
        SetFlattenMode();
        SetInactiveHandleType();
    }

    public override void SetFlattenMode()
    {
        BoundingBox.FlattenModeType flattenMode = this.boundingBox.FlattenAxis;
        switch (flattenMode)
        {
            case BoundingBox.FlattenModeType.FlattenX:
                disActiveList[0] = 0;
                disActiveList[1] = 2;
                disActiveList[2] = 4;
                disActiveList[3] = 6;
                disActiveList[4] = 1;
                disActiveList[5] = 8;
                disActiveList[6] = 5;
                disActiveList[7] = 10;
                break;
            case BoundingBox.FlattenModeType.FlattenY:
                disActiveList[0] = 1;
                disActiveList[1] = 3;
                disActiveList[2] = 5;
                disActiveList[3] = 7;
                disActiveList[4] = 0;
                disActiveList[5] = 4;
                disActiveList[6] = 8;
                disActiveList[7] = 9;
                break;
            case BoundingBox.FlattenModeType.FlattenZ:
                disActiveList[0] = 8;
                disActiveList[1] = 9;
                disActiveList[2] = 10;
                disActiveList[3] = 11;
                disActiveList[4] = 0;
                disActiveList[5] = 1;
                disActiveList[6] = 2;
                disActiveList[7] = 3;
                break;
        }

        if (flattenMode != BoundingBox.FlattenModeType.DoNotFlatten)
        {
            foreach (var index in disActiveList)
            {
                handles[index].SetActive(false);
            }
        }
    }

    public override void SetMaterial(Material material)
    {
        foreach (var handle in handles)
        {
            BoundingBoxUtils.ApplyMaterialToAllRenderers(handle.visual, material);
        }
    }

    private Vector3 GetRotationAxis(Transform handle)
    {
        foreach (var sideHandle in handles)
        {
            if (handle == sideHandle.root.transform)
            {
                switch (sideHandle.axis)
                {
                    case BoundingBox.AxisType.X:
                        return boundingBox.BoundingBoxContainer.right;
                    case BoundingBox.AxisType.Y:
                        return boundingBox.BoundingBoxContainer.up;
                    case BoundingBox.AxisType.Z:
                        return boundingBox.BoundingBoxContainer.forward;
                    default: break;
                }
            }
        }
        return Vector3.zero;
    }

    private void SetInactiveHandleType()
    {
        if (((int)boundingBox.ActiveHandle & (int)BoundingBox.HandleType.Rotation) == 0)
        {
            foreach (var handle in handles)
            {
                handle.SetActive(false);
            }
        }
    }

    private void SetActiveAxis(BoundingBox.AxisType axis, bool enable)
    {
        foreach (var handle in handles)
        {
            if (handle.axis == axis && !enable)
            {
                handle.SetActive(enable);
            }
        }
    }

    private void ReCreateVisual()
    {
        if (boundingBox.CornerBoundingBoxRoot == null)
        {
            return;
        }

        handles = new SideHandle[SIDE_NUM];
        for (int i = 0; i < SIDE_NUM; i++)
        {
            handles[i] = new SideHandle();
        }

        CornerBoundingBoxRoot cornerBoundingBoxRoot = boundingBox.CornerBoundingBoxRoot;

        handles[0].localPosition = (cornerBoundingBoxRoot.handles[0].localPosition + cornerBoundingBoxRoot.handles[1].localPosition) * 0.5f;
        handles[1].localPosition = (cornerBoundingBoxRoot.handles[0].localPosition + cornerBoundingBoxRoot.handles[2].localPosition) * 0.5f;
        handles[2].localPosition = (cornerBoundingBoxRoot.handles[3].localPosition + cornerBoundingBoxRoot.handles[2].localPosition) * 0.5f;
        handles[3].localPosition = (cornerBoundingBoxRoot.handles[3].localPosition + cornerBoundingBoxRoot.handles[1].localPosition) * 0.5f;

        handles[4].localPosition = (cornerBoundingBoxRoot.handles[4].localPosition + cornerBoundingBoxRoot.handles[5].localPosition) * 0.5f;
        handles[5].localPosition = (cornerBoundingBoxRoot.handles[4].localPosition + cornerBoundingBoxRoot.handles[6].localPosition) * 0.5f;
        handles[6].localPosition = (cornerBoundingBoxRoot.handles[7].localPosition + cornerBoundingBoxRoot.handles[6].localPosition) * 0.5f;
        handles[7].localPosition = (cornerBoundingBoxRoot.handles[7].localPosition + cornerBoundingBoxRoot.handles[5].localPosition) * 0.5f;

        handles[8].localPosition = (cornerBoundingBoxRoot.handles[0].localPosition + cornerBoundingBoxRoot.handles[4].localPosition) * 0.5f;
        handles[9].localPosition = (cornerBoundingBoxRoot.handles[1].localPosition + cornerBoundingBoxRoot.handles[5].localPosition) * 0.5f;
        handles[10].localPosition = (cornerBoundingBoxRoot.handles[2].localPosition + cornerBoundingBoxRoot.handles[6].localPosition) * 0.5f;
        handles[11].localPosition = (cornerBoundingBoxRoot.handles[3].localPosition + cornerBoundingBoxRoot.handles[7].localPosition) * 0.5f;

        handles[0].axis = BoundingBox.AxisType.X;
        handles[1].axis = BoundingBox.AxisType.Y;
        handles[2].axis = BoundingBox.AxisType.X;
        handles[3].axis = BoundingBox.AxisType.Y;
        handles[4].axis = BoundingBox.AxisType.X;
        handles[5].axis = BoundingBox.AxisType.Y;
        handles[6].axis = BoundingBox.AxisType.X;
        handles[7].axis = BoundingBox.AxisType.Y;
        handles[8].axis = BoundingBox.AxisType.Z;
        handles[9].axis = BoundingBox.AxisType.Z;
        handles[10].axis = BoundingBox.AxisType.Z;
        handles[11].axis = BoundingBox.AxisType.Z;

        for (int i = 0; i < SIDE_NUM; i++)
        {
            string rootName = "MidpointRoot_" + i.ToString();

            Transform existRoot = boundingBox.BoundingBoxContainer.Find(rootName);
            if (existRoot != null)
            {
                GameObject.Destroy(existRoot.gameObject);
            }

            handles[i].root = new GameObject("MidpointRoot_" + i).transform;
            handles[i].root.transform.parent = boundingBox.BoundingBoxContainer;
            handles[i].root.transform.localPosition = handles[i].localPosition;
            handles[i].root.transform.localRotation = Quaternion.identity;

            handles[i].visual = boundingBox.SidePrefab == null ? GameObject.CreatePrimitive(PrimitiveType.Sphere) : GameObject.Instantiate(boundingBox.SidePrefab);
            handles[i].visual.name = "visuals";

            handles[i].bounds = BoundingBoxUtils.GetMaxBounds(handles[i].visual);
            float maxDim = Mathf.Max(Mathf.Max(handles[i].bounds.size.x, handles[i].bounds.size.y), handles[i].bounds.size.z);
            float invScale = boundingBox.RotationHandleSize / maxDim;

            handles[i].visual.transform.parent = handles[i].root.transform;
            handles[i].visual.transform.localScale = new Vector3(invScale, invScale, invScale);
            handles[i].visual.transform.localPosition = Vector3.zero;
            handles[i].visual.transform.localRotation = Quaternion.identity;

            if (handles[i].axis == BoundingBox.AxisType.X)
            {
                Quaternion realignment = Quaternion.FromToRotation(Vector3.up, Vector3.right);
                handles[i].visual.transform.localRotation = realignment * handles[i].visual.transform.localRotation;
            }
            else if (handles[i].axis == BoundingBox.AxisType.Z)
            {
                Quaternion realignment = Quaternion.FromToRotation(Vector3.up, Vector3.forward);
                handles[i].visual.transform.localRotation = realignment * handles[i].visual.transform.localRotation;
            }

            Bounds bounds = new Bounds(handles[i].bounds.center * invScale, handles[i].bounds.size * invScale);
            if (handles[i].axis == BoundingBox.AxisType.X)
            {
                bounds.size = new Vector3(bounds.size.y, bounds.size.x, bounds.size.z);
            }
            else if (handles[i].axis == BoundingBox.AxisType.Z)
            {
                bounds.size = new Vector3(bounds.size.x, bounds.size.z, bounds.size.y);
            }

            BoxCollider collider = handles[i].root.gameObject.AddComponent<BoxCollider>();
            collider.size = bounds.size + new Vector3(0.02f, 0.02f, 0.02f);
            collider.center = bounds.center;

            handles[i].root.gameObject.AddComponent<NearInterationGrabbable>();

            var cursorBehavoir = handles[i].root.gameObject.AddComponent<CursorBehavoir>();
            cursorBehavoir.positionBehavoir = CursorBehavoir.PositionBehavoir.AnchorPosition3D;
            cursorBehavoir.visualBehavoir = CursorBehavoir.VisualBehavoir.Scale;
        }
    }
}
