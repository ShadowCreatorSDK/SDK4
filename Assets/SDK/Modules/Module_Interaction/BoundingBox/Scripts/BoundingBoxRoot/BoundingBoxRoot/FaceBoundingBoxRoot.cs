using SC.XR.Unity.Module_InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FaceHandle : BoundingBox.Handle
{
    public BoundingBox.AxisType axis;
}

public class FaceBoundingBoxRoot : AbstractBoundingBoxRoot
{
    private const int FACE_NUM = 6;
    private FaceHandle[] handles;

    private bool isHighLight = false;
    private bool isHideOtherHandle = false;
    private bool isVisible = false;
    private Transform currentActiveHandle = null;

    private BoundingBox.Handle currentHandle;
    private Vector3 initialGrabPoint;
    private Vector3 currentGrabPoint;
    private Vector3 oppositeCorner;
    private Vector3 diagonalDir;
    private Vector3 initialScaleOnGrabStart;
    private Vector3 initialPositionOnGrabStart;

    public FaceBoundingBoxRoot(BoundingBox boundingBox) : base(boundingBox)
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

        if (currentGrabPoint == Vector3.zero)
        {
            initialGrabPoint = currentGrabPoint = scPointEventData.Position3D;
        }
        currentGrabPoint = scPointEventData.Position3D;

        float initialDist = Vector3.Dot(initialGrabPoint - oppositeCorner, diagonalDir);
        float currentDist = Vector3.Dot(currentGrabPoint - oppositeCorner, diagonalDir);
        float scaleFactor = 1 + (currentDist - initialDist) / initialDist;

        Vector3 newScale = initialScaleOnGrabStart;// * scaleFactor;

        FaceHandle facePointHandle = currentHandle as FaceHandle;

        Vector3 axisScaleFactor = Vector3.one;

        if (facePointHandle.axis == BoundingBox.AxisType.X || facePointHandle.axis == BoundingBox.AxisType.NX)
        {
            axisScaleFactor = new Vector3(scaleFactor, 1, 1);

        }
        else if (facePointHandle.axis == BoundingBox.AxisType.Y || facePointHandle.axis == BoundingBox.AxisType.NY)
        {
            axisScaleFactor = new Vector3(1, scaleFactor, 1);
        }
        else if (facePointHandle.axis == BoundingBox.AxisType.Z || facePointHandle.axis == BoundingBox.AxisType.NZ)
        {
            axisScaleFactor = new Vector3(1, 1, scaleFactor);
        }
        newScale = Vector3.Scale(initialScaleOnGrabStart, axisScaleFactor);

        boundingBox.transform.localScale = newScale;

        float distance = Vector3.Distance(initialPositionOnGrabStart, oppositeCorner);
        float coef = (facePointHandle.axis == BoundingBox.AxisType.X || facePointHandle.axis == BoundingBox.AxisType.Y || facePointHandle.axis == BoundingBox.AxisType.Z)
            ? -1 : 1;

        boundingBox.transform.position = initialPositionOnGrabStart + coef * (boundingBox.transform.rotation * (Vector3.one - axisScaleFactor) * distance);
    }

    public override void OnHandlerPointerDown(PointerEventData eventData)
    {
        SCPointEventData scPointEventData = eventData as SCPointEventData;
        if (scPointEventData == null)
        {
            return;
        }

        currentHandle = GetHandle(scPointEventData.pointerCurrentRaycast.gameObject);

        currentGrabPoint = initialGrabPoint = Vector3.zero;
        initialScaleOnGrabStart = boundingBox.transform.localScale;
        initialPositionOnGrabStart = boundingBox.transform.position;

        oppositeCorner = boundingBox.BoundingBoxContainer.transform.TransformPoint(-scPointEventData.pointerCurrentRaycast.gameObject.transform.localPosition);
        diagonalDir = (scPointEventData.pointerCurrentRaycast.gameObject.transform.position - oppositeCorner).normalized;

        AudioSystem.getInstance.PlayAudioOneShot(boundingBox.gameObject, boundingBox.ScaleStartAudio);
        boundingBox.ScaleStarted?.Invoke();
        boundingBox.SetHighLight(scPointEventData.pointerCurrentRaycast.gameObject.transform, true);
    }

    public override void OnHandlerPointerUp(PointerEventData eventData)
    {
        AudioSystem.getInstance.PlayAudioOneShot(boundingBox.gameObject, boundingBox.ScaleStopAudio);
        boundingBox.ScaleStopped?.Invoke();
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
            boundingBox.SetHighLight(eventData.pointerCurrentRaycast.gameObject.transform, false);
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
            boundingBox.SetVisibility(true);
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
        SetFlattenMode();
        SetInactiveAxis(boundingBox.ActiveAxis);
        SetInactiveHandleType();
    }

    //TODO
    public override void SetFlattenMode()
    {
        BoundingBox.FlattenModeType flattenMode = this.boundingBox.FlattenAxis;

        foreach (var handle in handles)
        {
            if (flattenMode == BoundingBox.FlattenModeType.FlattenX && (((int)handle.axis & ((int)BoundingBox.AxisType.X | (int)BoundingBox.AxisType.NX)) != 0))
            {
                handle.SetActive(false);
            }

            if (flattenMode == BoundingBox.FlattenModeType.FlattenY && (((int)handle.axis & ((int)BoundingBox.AxisType.Y | (int)BoundingBox.AxisType.NY)) != 0))
            {
                handle.SetActive(false);
            }

            if (flattenMode == BoundingBox.FlattenModeType.FlattenY && (((int)handle.axis & ((int)BoundingBox.AxisType.Y | (int)BoundingBox.AxisType.NY)) != 0))
            {
                handle.SetActive(false);
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

    private BoundingBox.Handle GetHandle(GameObject raycastGameObject)
    {
        if (raycastGameObject == null)
        {
            return null;
        }

        foreach (var handle in handles)
        {
            if (handle.root.gameObject == raycastGameObject)
            {
                return handle;
            }
        }

        return null;
    }

    private void SetInactiveHandleType()
    {
        if (((int)boundingBox.ActiveHandle & (int)BoundingBox.HandleType.AxisScale) == 0)
        {
            foreach (var handle in handles)
            {
                handle.SetActive(false);
            }
        }
    }

    private void SetInactiveAxis(BoundingBox.AxisType axis)
    {
        foreach (var handle in handles)
        {
            if (((int)handle.axis & (int)axis) == 0)
            {
                handle.SetActive(false);
            }
        }
    }

    private void ReCreateVisual()
    {
        if (boundingBox.CornerBoundingBoxRoot == null)
        {
            return;
        }

        handles = new FaceHandle[FACE_NUM];
        for (int i = 0; i < FACE_NUM; i++)
        {
            handles[i] = new FaceHandle();
        }

        CornerBoundingBoxRoot cornerBoundingBoxRoot = boundingBox.CornerBoundingBoxRoot;

        handles[0].localPosition = (cornerBoundingBoxRoot.handles[0].localPosition + cornerBoundingBoxRoot.handles[3].localPosition) * 0.5f;
        handles[1].localPosition = (cornerBoundingBoxRoot.handles[3].localPosition + cornerBoundingBoxRoot.handles[6].localPosition) * 0.5f;
        handles[2].localPosition = (cornerBoundingBoxRoot.handles[5].localPosition + cornerBoundingBoxRoot.handles[6].localPosition) * 0.5f;
        handles[3].localPosition = (cornerBoundingBoxRoot.handles[0].localPosition + cornerBoundingBoxRoot.handles[5].localPosition) * 0.5f;
        handles[4].localPosition = (cornerBoundingBoxRoot.handles[0].localPosition + cornerBoundingBoxRoot.handles[6].localPosition) * 0.5f;
        handles[5].localPosition = (cornerBoundingBoxRoot.handles[1].localPosition + cornerBoundingBoxRoot.handles[7].localPosition) * 0.5f;

        handles[0].axis = BoundingBox.AxisType.NZ;
        handles[1].axis = BoundingBox.AxisType.Y;
        handles[2].axis = BoundingBox.AxisType.Z;
        handles[3].axis = BoundingBox.AxisType.NY;
        handles[4].axis = BoundingBox.AxisType.NX;
        handles[5].axis = BoundingBox.AxisType.X;

        for (int i = 0; i < FACE_NUM; i++)
        {
            string rootName = "FacePointRoot_" + i.ToString();

            Transform existRoot = boundingBox.BoundingBoxContainer.Find(rootName);
            if (existRoot != null)
            {
                GameObject.Destroy(existRoot.gameObject);
            }

            handles[i].root = new GameObject("FacePointRoot_" + i).transform;
            handles[i].root.transform.parent = boundingBox.BoundingBoxContainer;
            handles[i].root.transform.localPosition = handles[i].localPosition;
            handles[i].root.transform.localRotation = Quaternion.identity;

            handles[i].visual = boundingBox.facePrefab == null ? GameObject.CreatePrimitive(PrimitiveType.Cube) : GameObject.Instantiate(boundingBox.facePrefab);
            handles[i].visual.name = "visuals";

            if (handles[i].visual.GetComponent<Collider>())
            {
                GameObject.Destroy(handles[i].visual.GetComponent<Collider>());
            }

            handles[i].bounds = BoundingBoxUtils.GetMaxBounds(handles[i].visual);
            float maxDim = Mathf.Max(Mathf.Max(handles[i].bounds.size.x, handles[i].bounds.size.y), handles[i].bounds.size.z);
            float invScale = boundingBox.AxisScaleHandleSize / maxDim;

            handles[i].visual.transform.parent = handles[i].root.transform;
            handles[i].visual.transform.localScale = new Vector3(invScale, invScale, invScale);
            handles[i].visual.transform.localPosition = Vector3.zero;
            handles[i].visual.transform.localRotation = Quaternion.identity;

            Bounds bounds = new Bounds(handles[i].bounds.center * invScale, handles[i].bounds.size * invScale);

            BoxCollider collider = handles[i].root.gameObject.AddComponent<BoxCollider>();
            collider.size = bounds.size;
            collider.center = bounds.center;

            handles[i].root.gameObject.AddComponent<NearInterationGrabbable>();

            var cursorBehavoir = handles[i].root.gameObject.AddComponent<CursorBehavoir>();
            cursorBehavoir.positionBehavoir = CursorBehavoir.PositionBehavoir.AnchorPosition3D;
            cursorBehavoir.visualBehavoir = CursorBehavoir.VisualBehavoir.Scale;
        }
    }
}
