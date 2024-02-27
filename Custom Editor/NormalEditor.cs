using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using static MyCustomEditor;
#endif
public class NormalEditor : ScriptableObject, ICustomEditorItemData
{
    public GameObject _prefab;
    public Mesh _preview;
    public void Create(GameObject go)
    {
        GameObject.Instantiate(_prefab, go.transform);
    }

    public void Destroy(GameObject go)
    {

    }

#if UNITY_EDITOR
    public Mesh GetGizmosMesh()
    {
        return _preview;
    }

    public void UpdateInspectorOverride(Features features)
    {

        ObjectField("오리진 프리펩", ref _prefab);
        ObjectField("프리뷰 메쉬", ref _preview);

        if (this != null) EditorUtility.SetDirty(this);
    }
#endif
}