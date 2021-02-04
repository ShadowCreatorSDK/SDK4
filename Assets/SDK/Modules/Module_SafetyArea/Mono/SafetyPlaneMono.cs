using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SafetyPlaneMono : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isFreeze = false;
    private bool canFill = false;
    private int lastPaintIndex = -1;
    private PointerEventData currentPointerEventData;
    private Mesh mesh;
    private Color[] colors;

    private GroundHeightStep groundHeightStep;

    private Action<PointerEventData> OnPointerClickDown
    {
        get;
        set;
    }

    private Action<PointerEventData> OnPointerClickUp
    {
        get;
        set;
    }

    public void Init()
    {

        if (groundHeightStep == null)
        {
            groundHeightStep = SafetyAreaManager.Instance.GetStep<GroundHeightStep>(SafetyAreaStepEnum.GroundHeight);
        }
        
        MeshFilter meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = SafetyAreaVertexHelper.GeneratePlaneMesh();
        mesh = meshFilter.mesh;
        MeshRenderer meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = Resources.Load<Material>("Material/SafetyPlaneMat");
        MeshCollider meshCollider = this.gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        ClearMeshColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFreeze)
        {
            GameObject interactiveObject = GetInteractiveGameObject();
            if (interactiveObject != null)
            {
                groundHeightStep.SetPlaneHeight(interactiveObject.transform.position.y);
            }
            groundHeightStep.SetHeadPosition(GetHeadTransform().position);
            this.gameObject.transform.position = Vector3.up * groundHeightStep.GetPlaneHeight();
        }
        else
        {
            if (canFill)
            {
                FillNearest(currentPointerEventData);
            }
        }
    }

    public void FreezePlaneHeight()
    {
        isFreeze = true;
    }

    public void UnFreezePlaneHeight()
    {
        isFreeze = false;
    }

    private void EnableFill(PointerEventData eventData)
    {
        canFill = true;
    }

    private void DisableFill(PointerEventData eventData)
    {
        canFill = false;
    }

    public void ResetPlaneHeight()
    {
        groundHeightStep.ResetPlaneHeight();
    }

    private GameObject GetInteractiveGameObject()
    {
        return null;
        //return API_Module_InputSystem_BT3Dof.GetPointer().gameObject;
    }

    private Transform GetHeadTransform()
    {
        return SvrManager.Instance.head.transform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentPointerEventData = eventData;
        OnPointerClickDown?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerClickUp?.Invoke(eventData);
    }

    public void RegistPointerDownFillEvent()
    {
        OnPointerClickDown += EnableFill;
    }

    public void UnRegistPointerDownFillEvent()
    {
        OnPointerClickDown -= EnableFill;
    }

    public void RegistPointerUpFillEvent()
    {
        OnPointerClickUp += DisableFill;
    }

    public void UnRegistPointerUpFillEvent()
    {
        OnPointerClickUp -= DisableFill;
    }

    public void RegistPointerUpEvent(Action<PointerEventData> callback)
    {
        OnPointerClickUp += callback;
    }

    public void UnRegistPointerUpEvent(Action<PointerEventData> callback)
    {
        OnPointerClickUp -= callback;
    }

    public void FillNearest(PointerEventData pointerEventData)
    {
        if(currentPointerEventData == null)
        {
            Debug.LogError("currentPointerEventData == null");
            return;
        }

        Vector3 raycastPosition = pointerEventData.pointerCurrentRaycast.worldPosition;
        Vector3 pointerLocalPosition = transform.InverseTransformPoint(raycastPosition);
        List<int> effectIndices = SafetyAreaVertexHelper.CaculateEffectVerticeIndices(pointerLocalPosition);
        for (int i = 0; i < effectIndices.Count; i++)
        {
            int index = effectIndices[i];
            colors[index] = Color.red;
            lastPaintIndex = index;
        }
        mesh.colors = colors;
    }

    public void GenerateEdgeMesh(Action<Mesh> onGenerateMesh)
    {
        if (lastPaintIndex == -1)
        {
            Debug.LogError("lastPaintIndex == -1");
            return;
        }
        SafetyAreaEightNeighbourHelper.EightNeighbours(lastPaintIndex, (index) =>
        {
            return colors[index] == Color.red;
        }, (edgeIndices) =>
        {
            float planeHeight = groundHeightStep.GetPlaneHeight();
            Mesh edgeMesh = SafetyAreaVertexHelper.GenerateEdgeMesh(mesh, edgeIndices, planeHeight + 5f, planeHeight);
            onGenerateMesh?.Invoke(edgeMesh);
        });
    }

    public void ClearMeshColor()
    { 
        colors = Enumerable.Repeat(Color.white, mesh.vertexCount).ToArray();
        mesh.colors = colors;
    }
}
