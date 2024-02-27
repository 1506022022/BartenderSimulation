using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valve.VR.InteractionSystem;
using static ID;

public class PickedGanish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var manager = other.GetComponent<GameObjectManager>();
        if (manager == null) return;
        ItemGroup targetGid = GetGroup(manager.GetID);

        if (targetGid == ItemGroup.Glass)
            DecorateGarnishOnGlass(other);
    }
    void DecorateGarnishOnGlass(Collider other)
    {
        GetComponent<Interactable>()?.attachedToHand.DetachObject(gameObject);
        transform.SetParent(other.transform);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}
