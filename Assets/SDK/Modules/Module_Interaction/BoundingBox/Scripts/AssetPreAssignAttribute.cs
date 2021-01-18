using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetPreAssignAttribute : PropertyAttribute
{
    public string assetPath;
    public Type assetType;

    public AssetPreAssignAttribute(string assetPath, Type assetType)
    {
        this.assetPath = assetPath;
        this.assetType = assetType;
    }
}
