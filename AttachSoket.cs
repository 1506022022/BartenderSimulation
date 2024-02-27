using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;
using static Checker;

public class AttachSoket : MonoBehaviour
{
    public delegate void AttachCallback();

    public List<int> AbleTargetID;
    IAttachable _attachedTarget;
    FollowTarget _follow;
    bool _isPickup;
    public AttachCallback OnAttach;
    public AttachCallback OnDetach;
    public IAttachable GetAttachTarget => _attachedTarget;

    void Awake()
    {
        _follow = new FollowTarget(transform, null, false);
    }
    private void OnTriggerEnter(Collider other)
    {
        // 어태치
        if (!_isPickup || !other.isTrigger) return;
        var attachable = GetAttachable(other.transform);
        if (attachable != null)
        {
            Attach(attachable);
            var id = other.GetComponent<GameObjectManager>().GetID;
            if (id == 40000)
            {
                var sound = Resources.Load("SoundList") as SoundList;
                var audio = GetComponent<AudioSource>();
                sound.PlaySound(audio, "StrainerAttach");
            }
            else if (id == 40001)
            {
                var sound = Resources.Load("SoundList") as SoundList;
                var audio = GetComponent<AudioSource>();
                sound.PlaySound(audio, "ShakercapAttach");
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        // 디태치

        if (!_isPickup || !other.isTrigger) return;
        var attachable = GetAttachable(other.transform);
        if (attachable != null && attachable == _attachedTarget)
        {
            Detach();
            var id = other.GetComponent<GameObjectManager>().GetID;
            if (id == 40002)
            {
                var sound = Resources.Load("SoundList") as SoundList;
                var audio = GetComponent<AudioSource>();
                sound.PlaySound(audio, "ShakercapDetach");
            }
        }
    }
    public void OnPickup()
    {
        _isPickup = true;
    }
    public void ReleasePickup()
    {
        _isPickup = false;
        if (_follow.IsFollow) _follow.Follow();
    }
    void Attach(IAttachable target)
    {
        if (target.IsAttached) return;
        _attachedTarget = target;
        var targetTransform = target.AttachPoint;
        _follow.SetFollowTarget(transform, targetTransform, true);
        target.IsAttached = true;
        OnAttach?.Invoke();
    }
    void Detach()
    {
        _attachedTarget.IsAttached = false;
        _attachedTarget = null;
        _follow.SetFollowTarget(transform, null, false);
        OnDetach?.Invoke();
    }
    IAttachable GetAttachable(Transform target)
    {
        var manager = target.transform.GetComponent<GameObjectManager>();
        // 임시 코드
        if (manager == null) manager = target.parent?.GetComponent<GameObjectManager>();
        //
        if (AnyParametersNull(manager) ||
            !manager.isEmptyGameObject ||
            AbleTargetID == null ||
            !(AbleTargetID.Any(x => x == manager.GetID))) return null;
        var attachable = target.GetComponent<IAttachable>();
        return attachable;
    }
}

public interface IAttachable
{
    public Transform AttachPoint { get; set; }
    public bool IsAttached { get; set; }
}
public class FollowTarget
{
    Transform _target;
    Transform _me;
    bool _isFollow;
    public bool IsFollow => _isFollow;
    public FollowTarget(Transform me, Transform target, bool isFollow = true)
    {
        SetFollowTarget(me, target, isFollow);
    }

    public void SetFollowTarget(Transform me, Transform target, bool isFollow)
    {
        _me = me;
        _target = target;
        OnFollow(isFollow);

    }
    public void Follow()
    {
        if (_isFollow && _me && _target)
        {
            _me.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
    public void OnFollow(bool isFollow)
    {
        var interactable = _me.GetComponent<Interactable>();
        var rigidbody = _me.GetComponent<Rigidbody>();
        var targetRigidbody = _target?.GetComponent<Rigidbody>();
        _isFollow = isFollow;
        if (interactable.attachedToHand != null)
        {
            var newAttachObjectSetting = interactable.attachedToHand.AttachedObjects[0];
            newAttachObjectSetting.AttachedRigidbodyWasKinematic = _isFollow;
            newAttachObjectSetting.originalParent = _isFollow ? _target.gameObject : null;
            interactable.attachedToHand.AttachedObjects[0] = newAttachObjectSetting;
            if (_isFollow)
            {
                interactable.attachedToHand.DetachObject(_me.gameObject, false);
                _me.SetParent(_target);
                _me.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                rigidbody.isKinematic = true;
                rigidbody.velocity = Vector3.zero;
                
            }
            
        }
    }


}