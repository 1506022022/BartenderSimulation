using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Collections.Generic;
using UnityEditor;
using static Checker;
#if UNITY_EDITOR
using static MyCustomEditor;
#endif
public class GlassEditor : CustomEditorItemData
{
    public List<GameObject> _colliders = new List<GameObject>();
    public Vector3 _center;
    public Vector3 _scale;
    public float _saltEffectHeight;
    public override void Destroy(GameObject go)
    {
        while(go.transform.childCount !=0)
        {
            DestroyImmediate(go.transform.GetChild(0).gameObject);
        }
    }
    void Tag(Transform transform, string tag)
    {
        // 태그 변경
        transform.tag = tag;
        for (int i = 0; i < transform.childCount; i++)
        {
            Tag(transform.GetChild(i), tag);
        }
    }
    public override void Create(GameObject go)
    {
        // 태그 변경
        Tag(go.transform, "Glass");

        // 컴포넌트 추가
        var meshFilter = RequireComponent<MeshFilter>(go);
        var meshRenderer = RequireComponent<MeshRenderer>(go);
        var meshCollider = RequireComponent<MeshCollider>(go);
        var rigidbody = RequireComponent<Rigidbody>(go);
        var boxCollider = RequireComponent<BoxCollider>(go);
        RequireComponent<Interactable>(go);
        RequireComponent<Throwable>(go);
        RequireComponent<VelocityEstimator>(go);

        // 변수 설정
        meshFilter.mesh = _mesh;
        meshRenderer.material = _material;
        meshCollider.enabled = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = false;
        boxCollider.isTrigger = true;
        boxCollider.size = _scale;
        boxCollider.center = _center;
        InstantiateGlassCollider(go);
        var fresnelMatarial = Instantiate(Resources.Load("Fresnel")) as Material;
        AddMatarial(meshRenderer,fresnelMatarial);

        RequireSaltMaterial(meshRenderer);

        #region localFuntion
        Material AddMatarial(MeshRenderer meshRenderer, Material m)
        {
            Material[] temp = new Material[meshRenderer.materials.Length + 1];
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                temp[i] = meshRenderer.materials[i];
            }
            temp[temp.Length - 1] = m;
            meshRenderer.materials = temp;
            return meshRenderer.materials[meshRenderer.materials.Length - 1];
        }
        void RequireSaltMaterial(MeshRenderer meshRenderer)
        {
            bool isExitsSaltShader = false;
            foreach (var material in meshRenderer.materials)
            {
                if (material.shader.name == "Shader Graphs/SaltEffect") isExitsSaltShader = true;
            }
            if (!isExitsSaltShader)
            {
                var saltMatarial = Instantiate(Resources.Load("Material_SaltEffect")) as Material;
                var mat = AddMatarial(meshRenderer,saltMatarial);
                mat.SetFloat("_Height", _saltEffectHeight);
            }
        }
        void InstantiateGlassCollider(GameObject go)
        {
            foreach (var collider in _colliders)
            {
                var instance = Instantiate(collider, go.transform);
                instance.GetComponent<MeshRenderer>().enabled = false;
                instance.AddComponent<BoxCollider>();
            }
        }
        #endregion
    }
#if UNITY_EDITOR
    public override void UpdateInspectorOverride(Features features)
    {
        var contentStyle = GetContentStyle();
        // 잔
        LabelField("잔", contentStyle);
        ObjectField("Glass Mesh", ref features.Glass._mesh);
        ObjectField("Glass Material", ref features.Glass._material);
        FloatField("Salt Effect Height", ref _saltEffectHeight);

        // 콜라이더
        LabelField("콜라이더", contentStyle);
        Vector3Field("Trigger Center", ref features.Glass._center);
        Vector3Field("Trigger Scale", ref features.Glass._scale);
        ObjectListField("Colliders", features.Glass._colliders);
        if (this != null) EditorUtility.SetDirty(this);
    }
#endif
}