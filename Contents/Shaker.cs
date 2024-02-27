using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour, IAttachable
{
    public const int ShakerID = 1;
    List<int> _onColliderIndices = new List<int>();
    AttachSoket _attachSoket;
    GameObjectManager _goManager;
    public Transform AttachPoint { get; set; }
    public bool IsAttached { get; set; }

    private void Awake()
    {
        _attachSoket = GetComponent<AttachSoket>();
        _goManager = GetComponent<GameObjectManager>();
    }
}
