using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class SCUI3D : MonoBehaviour
{
    protected bool isDirty = false;

    protected virtual void Awake()
    {
        MarkUIDirty();
    }

    protected virtual void Update()
    {
        if (!isDirty)
        {
            return;
        }
        isDirty = false;
        RebuildUI();
    }

    public virtual void MarkUIDirty()
    {
        isDirty = true;
    }

    public abstract void RebuildUI();
}
