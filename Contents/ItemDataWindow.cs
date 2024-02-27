using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(ItemDataComponent))]
public class ItemDataWindow : MonoBehaviour
{
    GameObject UIWindow;                // ������ ����â
    static GameObject focusObject;      // ����â�� ����� ���
    GameObjectManager _manager;
    Text ui;                            // �ؽ�Ʈ UI

    // ������ �׽�Ʈ�� ��� ���
    private void OnDestroy()
    {
        View(false);
    }


    private void OnDisable()
    {
        View(false);
    }
    void Awake()
    {
        _manager = GetComponent<GameObjectManager>();
    }

    void Update()
    {
        // ��Ŀ�� �� �����۸� ���
        if (focusObject != gameObject)
        {
            View(false);
            return;
        }

        var interactable = GetComponent<Interactable>();
        if (interactable != null && interactable.attachedToHand != null)
        {
            View(false);
            return;
        }

        // ������ �о����
        ui.text = "";


        if (_manager.IsCreated)
        {
            ui.text = "[" + _manager.GetName + "]";
            ui.text += "\n\n" + _manager.GetDescription;
        }

    }


    protected virtual void OnHandHoverBegin(Hand hand)
    {
        View(true);
    }


    //-------------------------------------------------
    protected virtual void OnHandHoverEnd(Hand hand)
    {
        View(false);
    }

    // â ���
    public void View(bool isView)
    {
        if (isView)
        {
            RemoveBeforeWindow();
            OpenWindow();
            SetWindowPos();
            Operating(true);
        }
        else
        {
            Destroy(UIWindow);
            UIWindow = null;
            Operating(false);
        }

        void RemoveBeforeWindow()
        {
            var before = focusObject?.GetComponent<ItemDataWindow>()?.UIWindow;
            if (before != null) Destroy(before);
        }

        void SetWindowPos()
        {
            Vector3 pos = transform.position;
            pos.y = +1;
            UIWindow.transform.position = pos;
        }

        void OpenWindow()
        {
            UIWindow = Instantiate(Resources.Load<GameObject>("ItemDataWindow"));
            ui = UIWindow.GetComponentInChildren<Text>();
            focusObject = gameObject;
            UIWindow.SetActive(true);
        }

        void Operating(bool _is)
        {
            enabled = _is;
        }
    }
}
