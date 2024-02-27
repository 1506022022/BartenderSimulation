using UnityEngine;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using static Checker;
#if UNITY_EDITOR
using UnityEditor;
using static MyCustomEditor;
#endif
public class BlenderEditor : CustomEditorItemData
{
    public Vector3 AttachPos;
    public Vector4 AttachRot;
    public Vector3 TriggerSize;
    public Vector3 TriggerCenter;
    Transform AttachPoint;
    public List<int> AttachAbleList = new List<int>();
    public List<GameObject> ChildObjects = new List<GameObject>();
    BoxCollider Trigger;
    public override void Create(GameObject go)
    {
        // ������Ʈ �߰�
        var meshCollider = RequireComponent<MeshCollider>(go);
        var rigidbody = RequireComponent<Rigidbody>(go);
        var interactable = RequireComponent<Interactable>(go);
        var throwable = RequireComponent<Throwable>(go);
        var attachSoket = RequireComponent<AttachSoket>(go);
        RequireComponent<VelocityEstimator>(go);


        // ����
        SetMeshCollider();
        SetRigidbody();
        SetInteractable();
        if (AttachPoint == null) CreateAttachPoint();
        CreateChild();
        var blender = RequireComponent<Blender>(go);
        SetAttachSoket();

        #region local Funtion
        void SetMeshCollider()
        {
            meshCollider.sharedMesh = _mesh;
            meshCollider.enabled = false;
        }
        void SetRigidbody()
        {
            rigidbody.velocity = Vector3.zero;
        }
        void SetInteractable()
        {
            interactable.highlightOnHover = true;
        }
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
            blender.AttachPoint = AttachPoint;
            blender.IsAttached = false;
        }
        void CreateChild()
        {
            foreach (var child in ChildObjects)
            {
                Instantiate(child, go.transform);
            }
        }
        #endregion
    }
#if UNITY_EDITOR
    public override void UpdateInspectorOverride(Features features)
    {
        var contentStyle = GetContentStyle();
        var blender = features.Blender;
        LabelField("����", contentStyle);
        ObjectField("Mesh", ref blender._mesh);

        LabelField("����ġ", contentStyle);
        Vector3Field("AttachPos", ref blender.AttachPos);
        Vector4Field("AttachRot", ref blender.AttachRot);
        LabelField("����ġ Ʈ����", contentStyle);
        Vector3Field("Trigger Center", ref blender.TriggerCenter);
        Vector3Field("Trigger Size", ref blender.TriggerSize);
        // ����Ʈ ǥ��
        IntListField("����ġ ��� ID", blender.AttachAbleList);
        ObjectListField("�ڽ� ������Ʈ", blender.ChildObjects);
        if (this != null) EditorUtility.SetDirty(this);
    }
#endif
}
