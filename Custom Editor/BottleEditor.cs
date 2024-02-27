using UnityEngine;
using Valve.VR.InteractionSystem;
using static Checker;
using System.Collections.Generic;
#if UNITY_EDITOR
using static MyCustomEditor;
using UnityEditor;
#endif
public class BottleEditor : CustomEditorItemData
{
    public Color _fluidColor;
    public List<Mesh> _labelMeshs = new List<Mesh>();
    public List<Material> _labelMaterials = new List<Material>();
    public Vector3 _liquidEntranceScale;
    public Vector3 _liquidEntranceCenter;
    public override void Create(GameObject go)
    {
        // 컴포넌트 추가
        var meshFilter = RequireComponent<MeshFilter>(go);
        var meshRenderer = RequireComponent<MeshRenderer>(go);
        var rigidbody = RequireComponent<Rigidbody>(go);
        var meshCollider = RequireComponent<MeshCollider>(go);
        var liquidEntranceCollider = RequireComponent<BoxCollider>(go);
        var soundSource = RequireComponent<AudioSource>(go);
        RequireComponent<Interactable>(go);
        RequireComponent<Throwable>(go);
        RequireComponent<VelocityEstimator>(go);
        var soundManager = RequireComponent<SoundManager>(go);

        // 변수 설정
        meshFilter.mesh = _mesh;
        meshCollider.sharedMesh = _mesh;
        meshRenderer.material = _material;
        rigidbody.velocity = Vector3.zero;
        meshCollider.convex = true;
        // 추가
        liquidEntranceCollider.isTrigger = true;
        //liquidEntranceCollider.enabled = false;
        liquidEntranceCollider.size = _liquidEntranceScale;
        liquidEntranceCollider.center = _liquidEntranceCenter;
        soundSource.loop = false;
        for (int i = 0; i < _labelMeshs.Count; i++)
            Labeling(go, i);

        void Labeling(GameObject go, int i)
        {
            GameObject label = go.transform.Find("Label" + i.ToString())?.gameObject;
            if (label == null)
            {
                label = new GameObject("Label" + i.ToString());
                label.transform.SetParent(go.transform);
                label.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                label.transform.localScale = Vector3.zero;
                label.AddComponent<MeshFilter>();
                label.AddComponent<MeshRenderer>();
            }
            label.GetComponent<MeshFilter>().mesh = _labelMeshs[i];
            label.GetComponent<MeshRenderer>().material = _labelMaterials[i];
        }
    }

#if UNITY_EDITOR
    public override void UpdateInspectorOverride(Features features)
    {
        var contentStyle = GetContentStyle();
        // 병
        LabelField("병", contentStyle);
        ObjectField("Bottle Mesh", ref features.Bottle._mesh);
        ObjectField("Bottle Material", ref features.Bottle._material);
        ObjectListField("Labels Mesh", features.Bottle._labelMeshs);
        ObjectListField("Labels Material", features.Bottle._labelMaterials);
        Vector3Field("Entrance Center", ref features.Bottle._liquidEntranceCenter);
        Vector3Field("Entrance Scale", ref features.Bottle._liquidEntranceScale);
        if (this != null) EditorUtility.SetDirty(this);
    }
#endif
}
