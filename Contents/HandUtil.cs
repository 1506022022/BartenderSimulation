using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class HandUtil : MonoBehaviour
{
    [SerializeField]
    Book book;
    [SerializeField]
    Pages page;
    public GameObject prefab;
    Hand hand;
    Interactable interactable;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
    }
    public void CreateAndAttchToHand()
    {
        CreateGameObject();
        AttachToHand();
        interactable.attachedToHand = null;
    }
    void CreateGameObject()
    {
        page.PrintPages(book);
        book.UpdateSprites();
        book.transform.parent.parent.gameObject.SetActive(true);
        book.currentPage = 0;
    }
    private void OnAttachedToHand(Hand hand)
    {
        this.hand = hand;
        CreateAndAttchToHand();
    }

    void AttachToHand()
    {
        var obj = book.transform.parent.parent.gameObject;
        var throwable = obj.GetComponent<Throwable>();
        obj.transform.SetPositionAndRotation(hand.objectAttachmentPoint.transform.position, Quaternion.identity);
        hand.AttachObject(obj, hand.GetBestGrabbingType(), throwable.attachmentFlags);
    }
}
