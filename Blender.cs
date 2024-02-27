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
        float rotationSpeed = 2280f; // 초당 회전 각도
        var button = transform.GetChild(2).GetComponent<HoverButton>();
        if (button == null) yield break;
        button.enabled = false;
        float duration = 10f; // 회전 지속 시간

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
            // 회전 각도 계산
            float rotationAngle = rotationSpeed * Time.deltaTime;

            // 첫 번째 자식을 회전
            _wheel.Rotate(Vector3.up, rotationAngle);

            // 경과 시간 갱신
            elapsedTime += Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }
        button.enabled = true;
        _isOperating = false;
    }
}





