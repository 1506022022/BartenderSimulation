using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Collections.Generic;
using static Checker;

#if UNITY_EDITOR
using UnityEditor;
using static MyCustomEditor;
#endif
public class ShakerEditor : CustomEditorItemData
{
    public Vector3 AttachPos;
    public Vector4 AttachRot;
    public Vector3 TriggerSize;
    public Vector3 TriggerCenter;
    Transform AttachPoint;
    public List<int> AttachAbleList = new List<int>();
    BoxCollider Trigger;

    public override void Create(GameObject go)
    {
        // 컴포넌트 추가
        var meshFilter = RequireComponent<MeshFilter>(go);
        var meshRenderer = RequireComponent<MeshRenderer>(go);
        var meshCollider = RequireComponent<MeshCollider>(go);
        var rigidbody = RequireComponent<Rigidbody>(go);
        var interactable = RequireComponent<Interactable>(go);
        var throwable = RequireComponent<Throwable>(go);
        var attachSoket = RequireComponent<AttachSoket>(go);
        var shaker = RequireComponent<Shaker>(go);
        RequireComponent<VelocityEstimator>(go);


        // 설정
        SetMeshFilter();
        SetMeshCollider();
        SetMeshRenderer();
        SetRigidbody();
        SetInteractable();
        if (AttachPoint == null) CreateAttachPoint();
        SetAttachSoket();

        #region local Funtion
        void SetMeshFilter()
        {
            meshFilter.mesh = _mesh;
        }
        void SetMeshCollider()
        {
            meshCollider.sharedMesh = _mesh;
            meshCollider.convex = true;
        }
        void SetMeshRenderer()
        {
            meshRenderer.material = _material;
        }
        void SetRigidbody()
        {
            rigidbody.velocity = Vector3.zero;
        }
        void SetInteractable()
        {
            interactable.highlightOnHover = false;
        }
        #endregion
        void CreateAttachPoint()
        {
            AttachPoint = new GameObject("attachPoint").transform;
            AttachPoint.SetParent(go.transform);
            AttachPoint.SetLocalPositionAndRotation(AttachPos, Quaternion.Euler(AttachRot));
        }
        void SetAttachSoket()
        {
            if (Trigger == null)
            {
                Trigger = go.AddComponent<BoxCollider>();
                Trigger.center = TriggerCenter;
                Trigger.size = TriggerSize;
                Trigger.isTrigger = true;
            }
            throwable.onPickUp.AddListener(attachSoket.OnPickup);
            throwable.onDetachFromHand.AddListener(attachSoket.ReleasePickup);
            throwable.restoreOriginalParent = true;
            attachSoket.AbleTargetID = AttachAbleList;
            shaker.IsAttached = false;
            shaker.AttachPoint = AttachPoint;
        }
    }
#if UNITY_EDITOR
    public override void UpdateInspectorOverride(Features features)
    {
        var contentStyle = GetContentStyle();
        var shaker = features.Shaker;
        LabelField("쉐이커", contentStyle);
        ObjectField("Mesh", ref shaker._mesh);
        ObjectField("Material", ref shaker._material);

        LabelField("어태치", contentStyle);
        Vector3Field("AttachPos", ref shaker.AttachPos);
        Vector4Field("AttachRot", ref shaker.AttachRot);
        LabelField("어태치 트리거", contentStyle);
        Vector3Field("Trigger Center", ref shaker.TriggerCenter);
        Vector3Field("Trigger Size", ref shaker.TriggerSize);
        // 리스트 표시
        IntListField("어태치 대상 ID", shaker.AttachAbleList);

        if (this != null) EditorUtility.SetDirty(this);
    }
#endif
}
