using UnityEngine;
using Valve.VR.InteractionSystem;

public class Attachable : MonoBehaviour, IAttachable
{
    [SerializeField] Transform _attachPoint;
    public Transform AttachPoint { get { return _attachPoint; } set { _attachPoint = value; } }
    public bool IsAttached { get; set; } = false;
    Throwable _throwable;
    AttachSoket _attachSoket;

    private void OnEnable()
    {
        _throwable = GetComponent<Throwable>();
        _attachSoket = GetComponent<AttachSoket>();
        _throwable.onPickUp.AddListener(_attachSoket.OnPickup);
        _throwable.onDetachFromHand.AddListener(_attachSoket.ReleasePickup);
        _throwable.restoreOriginalParent = true;
    }
    private void OnDisable()
    {
        _throwable.onPickUp.RemoveListener(_attachSoket.OnPickup);
        _throwable.onDetachFromHand.RemoveListener(_attachSoket.ReleasePickup);
    }
}
