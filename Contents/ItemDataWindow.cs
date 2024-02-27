using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(ItemDataComponent))]
public class ItemDataWindow : MonoBehaviour
{
    GameObject UIWindow;                // 아이템 정보창
    static GameObject focusObject;      // 정보창에 출력할 대상
    GameObjectManager _manager;
    Text ui;                            // 텍스트 UI

    // 에디터 테스트용 출력 기능
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
        // 포커싱 된 아이템만 출력
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

        // 데이터 읽어오기
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

    // 창 출력
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
