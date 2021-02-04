using SC.XR.Unity.Module_InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CornerHandle : BoundingBox.Handle
{

}

public class CornerBoundingBoxRoot : AbstractBoundingBoxRoot
{
    private const int CORNER_NUM = 8;

    public CornerHandle[] handles;

    private int[] disActiveList = new int[4] { 0, 0, 0, 0 };

    private bool isHighLight = false;
    private bool isHideOtherHandle = false;
    private bool isVisible = false;
    private Transform currentActiveHandle = null;

    private Vector3 initialGrabPoint;
    private Vector3 currentGrabPoint;
    private Vector3 oppositeCorner;
    private Vector3 diagonalDir;
    private Vector3 initialScaleOnGrabStart;
    private Vector3 initialPositionOnGrabStart;

    public CornerBoundingBoxRoot(BoundingBox boundingBox) : base(boundingBox)
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

        Vector3 newScale = initialScaleOnGrabStart * scaleFactor;

        if (scaleFactor > 0) {

            boundingBox.transform.localScale = newScale;
            boundingBox.transform.position = initialPositionOnGrabStart * scaleFactor + (1 - scaleFactor) * oppositeCorner;
        }

    }

    public override void OnHandlerPointerDown(PointerEventData eventData)
    {
        SCPointEventData scPointEventData = eventData as SCPointEventData;
        if (scPointEventData == null)
        {
            return;
        }

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
                break;
            case BoundingBox.FlattenModeType.FlattenY:
                disActiveList[0] = 0;
                disActiveList[1] = 1;
                disActiveList[2] = 4;
                disActiveList[3] = 5;
                break;
            case BoundingBox.FlattenModeType.FlattenZ:
                disActiveList[0] = 0;
                disActiveList[1] = 1;
                disActiveList[2] = 2;
                disActiveList[3] = 3;
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

    private void ReCreateVisual()
    {
        if (boundingBox.BoundingBoxContainer == null)
        {
            return;
        }

        handles = new CornerHandle[CORNER_NUM];
        for (int i = 0; i < CORNER_NUM; i++)
        {
            handles[i] = new CornerHandle();
        }

        Vector3 currentBoundsExtents = this.boundingBox.CurrentBoundsExtents;
        for (int i = 0; i < CORNER_NUM; i++)
        {
            handles[i].localPosition = new Vector3(
                    (i & (1 << 0)) == 0 ? -currentBoundsExtents[0] : currentBoundsExtents[0],
                    (i & (1 << 1)) == 0 ? -currentBoundsExtents[1] : currentBoundsExtents[1],
                    (i & (1 << 2)) == 0 ? -currentBoundsExtents[2] : currentBoundsExtents[2]);
        }

        Transform boundingBoxContainer = this.boundingBox.BoundingBoxContainer;
        for (int i = 0; i < CORNER_NUM; i++)
        {
            string rootName = "CornerRoot_" + i.ToString();

            Transform existRoot = boundingBoxContainer.Find(rootName);
            if (existRoot != null)
            {
                GameObject.Destroy(existRoot.gameObject);
            }

            handles[i].root = new GameObject("CornerRoot_" + i).transform;
            handles[i].root.transform.parent = boundingBoxContainer;
            handles[i].root.transform.localPosition = handles[i].localPosition;
            handles[i].root.transform.localRotation = Quaternion.identity;

            handles[i].visualsScale = new GameObject("visualsScale").transform;
            handles[i].visualsScale.transform.parent = handles[i].root.transform;
            handles[i].visualsScale.transform.localPosition = Vector3.zero;
            if (Mathf.Sign(handles[i].localPosition[0]) < 0 && Mathf.Sign(handles[i].localPosition[1]) < 0 && Mathf.Sign(handles[i].localPosition[2]) < 0)
            {
                handles[i].visualsScale.transform.localEulerAngles = new Vector3(90, 90, 270);

            }
            else if (Mathf.Sign(handles[i].localPosition[0]) > 0 && Mathf.Sign(handles[i].localPosition[1]) < 0 && Mathf.Sign(handles[i].localPosition[2]) < 0)
            {
                handles[i].visualsScale.transform.localEulerAngles = new Vector3(90, 90, 0);

            }
            else if (Mathf.Sign(handles[i].localPosition[0]) < 0 && Mathf.Sign(handles[i].localPosition[1]) > 0 && Mathf.Sign(handles[i].localPosition[2]) < 0)
            {
                handles[i].visualsScale.transform.localEulerAngles = new Vector3(270, 90, 0);

            }
            else if (Mathf.Sign(handles[i].localPosition[0]) > 0 && Mathf.Sign(handles[i].localPosition[1]) > 0 && Mathf.Sign(handles[i].localPosition[2]) < 0)
            {
                handles[i].visualsScale.transform.localEulerAngles = new Vector3(0, 90, 0);

            }
            else if (Mathf.Sign(handles[i].localPosition[0]) < 0 && Mathf.Sign(handles[i].localPosition[1]) < 0 && Mathf.Sign(handles[i].localPosition[2]) > 0)
            {
                handles[i].visualsScale.transform.localEulerAngles = new Vector3(90, 90, 180);

            }
            else if (Mathf.Sign(handles[i].localPosition[0]) < 0 && Mathf.Sign(handles[i].localPosition[1]) > 0 && Mathf.Sign(handles[i].localPosition[2]) > 0)
            {
                handles[i].visualsScale.transform.localEulerAngles = new Vector3(0, 0, 90);

            }
            else if (Mathf.Sign(handles[i].localPosition[0]) > 0 && Mathf.Sign(handles[i].localPosition[1]) < 0 && Mathf.Sign(handles[i].localPosition[2]) > 0)
            {
                handles[i].visualsScale.transform.localEulerAngles = new Vector3(90, 0, 0);
            }
            else
            {
                handles[i].visualsScale.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            handles[i].visualsScale.transform.localScale = new Vector3(1, 1, 1);

            GameObject prefabToInstantiate = (this.boundingBox.FlattenAxis != BoundingBox.FlattenModeType.DoNotFlatten) ? this.boundingBox.CornerSlatePrefab : this.boundingBox.CornerPrefab;
            if (prefabToInstantiate == null)
            {
                handles[i].visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
                handles[i].visual.transform.parent = handles[i].visualsScale;
                handles[i].visual.transform.localPosition = Vector3.zero;
            }
            else
            {
                handles[i].visual = GameObject.Instantiate(prefabToInstantiate, handles[i].visualsScale);
            }

            handles[i].visual.name = "Visual";

            if (handles[i].visual.GetComponent<Collider>())
            {
                GameObject.Destroy(handles[i].visual.GetComponent<Collider>());
            }

            if (this.boundingBox.FlattenAxis == BoundingBox.FlattenModeType.FlattenX)
            {
                if (Mathf.Sign(handles[i].localPosition[1]) > 0 && Mathf.Sign(handles[i].localPosition[2]) > 0)
                {
                    handles[i].visual.transform.Rotate(0, 90, 0);
                }
                else if (Mathf.Sign(handles[i].localPosition[1]) < 0 && Mathf.Sign(handles[i].localPosition[2]) > 0)
                {
                    handles[i].visual.transform.Rotate(0, -90, -90);
                }
                else if (Mathf.Sign(handles[i].localPosition[1]) > 0 && Mathf.Sign(handles[i].localPosition[2]) < 0)
                {
                    handles[i].visual.transform.Rotate(0, 0, -90);
                }
                else if (Mathf.Sign(handles[i].localPosition[1]) < 0 && Mathf.Sign(handles[i].localPosition[2]) < 0)
                {
                    handles[i].visual.transform.Rotate(90, 90, 0);
                }
                //handles[i].visual.transform.Rotate(0, -90, -90);
            }
            else if (this.boundingBox.FlattenAxis == BoundingBox.FlattenModeType.FlattenY)
            {
                if (Mathf.Sign(handles[i].localPosition[0]) > 0 && Mathf.Sign(handles[i].localPosition[2]) > 0)
                {
                    handles[i].visual.transform.Rotate(90, 90, 0);
                }
                else if (Mathf.Sign(handles[i].localPosition[0]) > 0 && Mathf.Sign(handles[i].localPosition[2]) < 0)
                {
                    handles[i].visual.transform.Rotate(90, 0, -90);
                }
                else if (Mathf.Sign(handles[i].localPosition[0]) < 0 && Mathf.Sign(handles[i].localPosition[2]) > 0)
                {
                    handles[i].visual.transform.Rotate(0, 90, 0);
                }
                else if (Mathf.Sign(handles[i].localPosition[0]) < 0 && Mathf.Sign(handles[i].localPosition[2]) < 0)
                {
                    handles[i].visual.transform.Rotate(0, 0, -90);
                }
            }
            else if (this.boundingBox.FlattenAxis == BoundingBox.FlattenModeType.FlattenZ)
            {
                if (Mathf.Sign(handles[i].localPosition[0]) > 0 && Mathf.Sign(handles[i].localPosition[1]) > 0)
                {
                    handles[i].visual.transform.Rotate(0, 0, -90);
                }
                else if (Mathf.Sign(handles[i].localPosition[0]) > 0 && Mathf.Sign(handles[i].localPosition[1]) < 0)
                {
                    handles[i].visual.transform.Rotate(90, 0, -90);
                }
                else if (Mathf.Sign(handles[i].localPosition[0]) < 0 && Mathf.Sign(handles[i].localPosition[1]) > 0)
                {
                    handles[i].visual.transform.Rotate(0, 0, -90);
                }
                else if (Mathf.Sign(handles[i].localPosition[0]) < 0 && Mathf.Sign(handles[i].localPosition[1]) < 0)
                {
                    handles[i].visual.transform.Rotate(0, 90, 0);
                }
            }

            handles[i].bounds = BoundingBoxUtils.GetMaxBounds(handles[i].visual);
            float maxDim = Mathf.Max(Mathf.Max(handles[i].bounds.size.x, handles[i].bounds.size.y), handles[i].bounds.size.z);
            handles[i].bounds.size = maxDim * Vector3.one;

            handles[i].bounds.center = new Vector3(
                (i & (1 << 0)) == 0 ? handles[i].bounds.center.x : -handles[i].bounds.center.x,
                (i & (1 << 1)) == 0 ? -handles[i].bounds.center.y : handles[i].bounds.center.y,
                (i & (1 << 2)) == 0 ? -handles[i].bounds.center.z : handles[i].bounds.center.z
            );

            float scaleHandleSize = this.boundingBox.ScaleHandleSize;
            var invScale = scaleHandleSize / handles[i].bounds.size.x;
            handles[i].visual.transform.localScale = new Vector3(invScale, invScale, invScale);

            BoxCollider collider = handles[i].root.gameObject.AddComponent<BoxCollider>();
            collider.size = handles[i].bounds.size * invScale + new Vector3(0.02f, 0.02f, 0.02f);
            collider.center = handles[i].bounds.center * invScale;

            handles[i].root.gameObject.AddComponent<NearInterationGrabbable>();

            var cursorBehavoir = handles[i].root.gameObject.AddComponent<CursorBehavoir>();
            cursorBehavoir.positionBehavoir = CursorBehavoir.PositionBehavoir.AnchorPosition3D;
            cursorBehavoir.visualBehavoir = CursorBehavoir.VisualBehavoir.Scale;
        }
    }

    private void SetInactiveHandleType()
    {
        if (((int)boundingBox.ActiveHandle & (int)BoundingBox.HandleType.Scale) == 0)
        {
            foreach (var handle in handles)
            {
                handle.SetActive(false);
            }
        }
    }
}
