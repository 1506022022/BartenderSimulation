//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Basic throwable object
//
//=============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class BookInteractable : MonoBehaviour
    {
        [SerializeField] Book book;
        [EnumFlags]
        [Tooltip("The flags used to attach this object to the hand.")]
        public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.DetachFromOtherHand;

        [Tooltip("The local point which acts as a positional and rotational offset to use while held")]
        public Transform attachmentOffset;

        protected bool attached = false;
        protected float attachTime;
        protected Vector3 attachPosition;
        protected Quaternion attachRotation;
        protected Transform attachEaseInTransform;

        public UnityEvent onPickUp = new UnityEvent();
        public UnityEvent onDetachFromHand = new UnityEvent();
        public HandEvent onHeldUpdate;

        [HideInInspector]
        public Interactable interactable;


        //-------------------------------------------------
        protected virtual void Awake()
        {
            interactable = GetComponent<Interactable>();
        }


        //-------------------------------------------------
        protected virtual void OnHandHoverBegin(Hand hand)
        {
            bool showHint = false;

            // "Catch" the throwable by holding down the interaction button instead of pressing it.
            // Only do this if the throwable is moving faster than the prescribed threshold speed,
            // and if it isn't attached to another hand
            if (!attached)
            {

                GrabTypes bestGrabType = hand.GetBestGrabbingType();

                if (bestGrabType != GrabTypes.None)
                {
                    hand.AttachObject(gameObject, bestGrabType, attachmentFlags);
                    showHint = false;
                }
            }

            if (showHint)
            {
                hand.ShowGrabHint();
            }
        }


        //-------------------------------------------------
        protected virtual void OnHandHoverEnd(Hand hand)
        {
            hand.HideGrabHint();
        }


        //-------------------------------------------------
        protected virtual void HandHoverUpdate(Hand hand)
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();

            if (startingGrabType != GrabTypes.None)
            {
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags, attachmentOffset);
                hand.HideGrabHint();
            }
        }

        //-------------------------------------------------
        protected virtual void OnAttachedToHand(Hand hand)
        {
            //Debug.Log("<b>[SteamVR Interaction]</b> Pickup: " + hand.GetGrabStarting().ToString());


            attached = true;

            book.HoldHand = hand;
            onPickUp.Invoke();

            hand.HoverLock(null);

            attachTime = Time.time;
            attachPosition = transform.position;
            attachRotation = transform.rotation;

        }


        //-------------------------------------------------
        protected virtual void OnDetachedFromHand(Hand hand)
        {
            attached = false;

            book.HoldHand = null;
            onDetachFromHand.Invoke();

            hand.HoverUnlock(null);

        }



        //-------------------------------------------------
        protected virtual void HandAttachedUpdate(Hand hand)
        {


            if (hand.IsGrabEnding(this.gameObject))
            {
                hand.DetachObject(gameObject, false);

                // Uncomment to detach ourselves late in the frame.
                // This is so that any vehicles the player is attached to
                // have a chance to finish updating themselves.
                // If we detach now, our position could be behind what it
                // will be at the end of the frame, and the object may appear
                // to teleport behind the hand when the player releases it.
                //StartCoroutine( LateDetach( hand ) );
            }

            if (onHeldUpdate != null)
                onHeldUpdate.Invoke(hand);
        }


        //-------------------------------------------------
        protected virtual IEnumerator LateDetach(Hand hand)
        {
            yield return new WaitForEndOfFrame();

            hand.DetachObject(gameObject, false);
        }


        //-------------------------------------------------
        protected virtual void OnHandFocusAcquired(Hand hand)
        {
            gameObject.SetActive(true);
        }


        //-------------------------------------------------
        protected virtual void OnHandFocusLost(Hand hand)
        {
            gameObject.SetActive(false);
        }
    }

    public enum ReleaseStyle
    {
        NoChange,
        GetFromHand,
        ShortEstimation,
        AdvancedEstimation,
    }
}
