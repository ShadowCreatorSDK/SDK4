using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SCBaseLayoutGroup : MonoBehaviour
{
    public Action<SCBaseLayoutGroup> OnUpdatedCollection{ get;set; }

    [HideInInspector]
    [SerializeField]
    private List<GroupObj> objList = new List<GroupObj>();

    protected List<GroupObj> ObjList {
        get { return objList; }
    }

    [SerializeField]
    private bool isIgnoreInactiveObj = true;//Ignore the deactivation object
    public bool IsIgnoreInactiveObj 
    {
        get { return isIgnoreInactiveObj; }
        set { isIgnoreInactiveObj = value; }
    }

    [SerializeField]
    private ListSortType groupSortType = ListSortType.Childindex;
    public ListSortType GroupSortType
    {
        get { return groupSortType; }
        set { groupSortType = value; }
    }

    private Transform child;
    protected void InitGroup()
    {

        var tempNodes = new List<GroupObj>();
        for (int i = 0; i < ObjList.Count; i++)
        {
            if (ObjList[i].Transform==null || (IsIgnoreInactiveObj && !ObjList[i].Transform.gameObject.activeSelf)|| !(ObjList[i].Transform.parent.gameObject==gameObject) || ObjList[i].Transform.parent==null)
            {
                tempNodes.Add(ObjList[i]);
            }
        }
        for (int i = 0; i < tempNodes.Count; i++)
        {
            ObjList.Remove(tempNodes[i]);
        }
        tempNodes.Clear();
    }

    protected void SortGroup()
    {

        switch (GroupSortType) 
        {
            case ListSortType.Childindex:
                ObjList.Sort((c1,c2)=>(c1.Transform.GetSiblingIndex().CompareTo(c2.Transform.GetSiblingIndex())));
                break;
            case ListSortType.ChildIndexReverse:
                ObjList.Sort((c1, c2) => (c1.Transform.GetSiblingIndex().CompareTo(c2.Transform.GetSiblingIndex())));
                ObjList.Reverse();
                break;
            case ListSortType.ChildAlphabet:
                ObjList.Sort((c1, c2) => (string.CompareOrdinal(c1.Name, c2.Name)));
                break;
            case ListSortType.ChildAlphabetReverse:
                ObjList.Sort((c1, c2) => (c1.Transform.GetSiblingIndex().CompareTo(c2.Transform.GetSiblingIndex())));
                ObjList.Reverse();
                break;

        }
    }

    protected bool IsContainsObj(Transform obj)
    {
        if (obj == null)
        {
            return false;
        }
        for (int i = 0; i < ObjList.Count; i++)
        {
            if (ObjList[i].Transform== obj)
            {
                return true;
            }
        }
        return false;
    }

    protected abstract void LayoutChildren();

    public virtual void RefreshInfo()
    {
        InitGroup();

        for (int i = 0; i < transform.childCount; i++)
        {
            child = transform.GetChild(i);
           
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(child, "ObjectCollection modify transform");
#endif 

            if (!IsContainsObj(child) && (child.gameObject.activeSelf || !IsIgnoreInactiveObj))
            {
                ObjList.Add(new GroupObj { Name = child.name, Transform = child });               
            }           
        }

        SortGroup();
        LayoutChildren();
        if (OnUpdatedCollection != null)
        {
            OnUpdatedCollection.Invoke(this);
        }
    }
}
