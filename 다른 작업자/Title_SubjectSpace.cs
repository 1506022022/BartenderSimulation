using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Title_SubjectSpace : MonoBehaviour
{
    const string SubjectTargetTag = "Glass";
    public UnityEvent subjectEvent;

    void OnTriggerEnter(Collider other)
    {
        EnterSpace(other);
    }

    void EnterSpace(Collider other)
    {
        if (other.tag != SubjectTargetTag) return;

        var target = other.gameObject;
        var targetRigid = target.GetComponent<Rigidbody>();
        var targetInteractable = target.GetComponent<Interactable>();
        var attachedToHand = targetInteractable?.attachedToHand;

        if (Checker.Exist(targetInteractable, attachedToHand)) DetacchToHand();
        if (targetRigid) NonKinematicTarget();

        if(subjectEvent.GetPersistentEventCount() > 0) subjectEvent.Invoke();

        #region localFunc
        void DetacchToHand()
        {
            attachedToHand.DetachObject(target);
            targetInteractable.attachedToHand = null;
        }

        void NonKinematicTarget()
        {
            targetRigid.isKinematic = true;
        }
        #endregion
    }
}
