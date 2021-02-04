using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaMono : MonoBehaviour
{
    private Transform headTransform;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private Mesh edgeMesh;

    // Start is called before the first frame update
    void Start()
    {
        if (headTransform == null)
        {
            headTransform = SvrManager.Instance.head;
        }

        meshFilter = this.gameObject.AddComponent<MeshFilter>();
        meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = Resources.Load<Material>("Material/SafetyEdgeMat");
        if(edgeMesh == null)
        {
            Debug.LogError("EdgeMesh is Null");
            return;
        }
        meshFilter.mesh = edgeMesh;
    }

    // Update is called once per frame
    void Update()
    {
        meshRenderer.sharedMaterial.SetVector("headPosition", new Vector4(headTransform.position.x, headTransform.position.y, headTransform.position.z, 1f));
    }

    public void SetMesh(Mesh mesh)
    {
        this.edgeMesh = mesh;
    }
}
