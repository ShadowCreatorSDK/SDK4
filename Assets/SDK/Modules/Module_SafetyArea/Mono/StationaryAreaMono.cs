using System;
using UnityEngine;

public class StationaryAreaMono : MonoBehaviour
{
    private bool isFreeze = false;
    private StationaryAreaStep stationaryAreaStep;
    private GroundHeightStep groundHeightStep;
    private Transform headTransform;
    private MeshRenderer meshRenderer;

    void Start()
    {
        if (stationaryAreaStep == null)
        {
            stationaryAreaStep = SafetyAreaManager.Instance.GetStep<StationaryAreaStep>(SafetyAreaStepEnum.StationaryArea);
        }

        if (groundHeightStep == null)
        {
            groundHeightStep = SafetyAreaManager.Instance.GetStep<GroundHeightStep>(SafetyAreaStepEnum.GroundHeight);
        }

        if (headTransform == null)
        {
            headTransform = SvrManager.Instance.head;
        }

        float groundHeight = groundHeightStep.GetPlaneHeight();
        MeshFilter meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = Resources.Load<Material>("Material/SafetyEdgeMat");
        meshFilter.mesh = SafetyAreaVertexHelper.GenerateCylinderMesh(new Vector3(0, groundHeight, 0), groundHeight + 5, groundHeight);
    }

    void Update()
    {
        if (!isFreeze)
        {
            Vector3 headPosition = SvrManager.Instance.head.position;
            stationaryAreaStep.SetCircleCenter(headPosition);
            Vector2 circleCenter = stationaryAreaStep.GetCircleCenter();
            this.gameObject.transform.position = new Vector3(circleCenter.x, groundHeightStep.GetPlaneHeight(), circleCenter.y);
        }
        meshRenderer.sharedMaterial.SetVector("headPosition", new Vector4(headTransform.position.x, headTransform.position.y, headTransform.position.z, 1f));
    }

    public void FreezeStationaryAreaPosition()
    {
        isFreeze = true;
    }

    public void UnFreezeStationaryAreaPosition()
    {
        isFreeze = false;
    }
}
