using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxUtils
{
    public static void ApplyMaterialToAllRenderers(GameObject root, Material material)
    {
        if (material != null)
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>();

            for (int i = 0; i < renderers.Length; ++i)
            {
                renderers[i].sharedMaterial = material;
            }
        }
    }

    public static Bounds GetMaxBounds(GameObject g)
    {
        var b = new Bounds();
        Mesh currentMesh;
        foreach (MeshFilter r in g.GetComponentsInChildren<MeshFilter>())
        {
            if ((currentMesh = r.sharedMesh) == null) { continue; }

            if (b.size == Vector3.zero)
            {
                b = currentMesh.bounds;
            }
            else
            {
                b.Encapsulate(currentMesh.bounds);
            }
        }
        return b;
    }
}
