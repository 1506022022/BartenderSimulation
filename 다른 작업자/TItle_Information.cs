using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TItle_Information : MonoBehaviour
{
    [SerializeField] private List<GameObject> InformationList;
    [SerializeField] private List<ObjectList> HighlightObjects;

    [SerializeField] private Material highlightMat;

    private int index = 0;
    public int Index
    {
        get
        {
            return index;
        }
        set
        {
            if (value >= InformationList.Count || value >= HighlightObjects.Count) {
                return; 
            }

            InformationList[index]?.SetActive(false);
            //MaterialDelete(HighlightObjects[index]);
            index = value;
            InformationList[index]?.SetActive(true);
            //MaterialAdd(HighlightObjects[index]);
        }
    }

    public void IndexAdd()
    {
        Index++;
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void MaterialDelete(ObjectList list)
    {
        foreach (var obj in list.List)
        {
            var mat = obj.GetComponent<MeshRenderer>()?.materials.ToList();
            mat.Remove(mat.Last());
            obj.GetComponent<MeshRenderer>().materials = mat.ToArray();
        }
    }

    private void MaterialAdd(ObjectList list)
    {
        foreach (var obj in list.List)
        {
            var mat = obj.GetComponent<MeshRenderer>()?.materials.ToList();
            mat.Add(highlightMat);
            obj.GetComponent<MeshRenderer>().materials = mat.ToArray();
        }
    }
}

[Serializable]
public class ObjectList
{
    public List<GameObject> List;
}
