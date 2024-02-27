using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Blender : MonoBehaviour, IAttachable
{
    const int Body = 40015;
    const int Carafe = 40016;
    const int Cap = 40017;
    SoundList _soundList;
    [SerializeField] AudioSource _audio;
    Transform _wheel;
    GameObjectManager _manager;
    bool _isOperating;
    public Transform AttachPoint { get; set; }
    bool _isAttached;
    public bool IsAttached
    {
        get
        {
            return _isAttached;
        }
        set
        {
            if (_manager != null && _manager.GetID == Body)
            {
                _wheel = value ? AttachPoint.GetChild(0).GetChild(1) : null;
            }
            _isAttached = value;
        }
    }

    private void Awake()
    {
        _manager = GetComponent<GameObjectManager>();
        if (_manager.GetID == Body)
        {
            transform.GetChild(2).GetComponent<HoverButton>().onButtonDown.AddListener(Operating);
        }
    }
    void Operating(Hand dummy)
    {
        StartCoroutine(RotateFirstChildForDuration());
    }
    IEnumerator RotateFirstChildForDuration()
    {
        if (_isOperating || _wheel == null) yield break;
        _isOperating = true;
        float rotationSpeed = 2280f; // �ʴ� ȸ�� ����
        var button = transform.GetChild(2).GetComponent<HoverButton>();
        if (button == null) yield break;
        button.enabled = false;
        float duration = 10f; // ȸ�� ���� �ð�

        float elapsedTime = 0f;
        GetComponent<SoundManager>().ButtonSound();
        while (elapsedTime < duration)
        {
            if (!IsAttached)
            {
                _isOperating = false;
                button.enabled = true;
                yield break;
            }
            // ȸ�� ���� ���
            float rotationAngle = rotationSpeed * Time.deltaTime;

            // ù ��° �ڽ��� ȸ��
            _wheel.Rotate(Vector3.up, rotationAngle);

            // ��� �ð� ����
            elapsedTime += Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }
        button.enabled = true;
        _isOperating = false;
    }
}





