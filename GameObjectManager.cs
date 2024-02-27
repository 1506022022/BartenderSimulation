using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Checker;

public class GameObjectManager : MonoBehaviour
{
    #region static
    static List<GameObjectManager> managers;
    public static void InitGameObject()
    {
        foreach (var manager in managers)
        {
            if (!manager._isDontInit)
                manager.Destroy();
        }
        foreach (var manager in managers)
        {
            if (!manager._isDontInit)
                manager.Create();
        }
    }
    #endregion

    public bool IsCreated { get; private set; }
    PositionIniter _posIniter;
    [SerializeField] bool _isInitPos;
    [SerializeField] bool _isDontDestoryChildrens;
    [SerializeField] bool _isDontDestoryComponents;
    [SerializeField] bool _isDontInit;
    [SerializeField] bool _isDontCreate;
    [SerializeField] Features _features;
    [SerializeField] List<Component> _dontDestroy;

    public int GetID => _features.ID;
    public string GetName => _features.Name;
    public string GetDescription => _features.Description;
    public bool isEmptyGameObject => _features != null;
    public ICustomEditorItemData GetGUI => _features.GetCustomEditor();

    private void Awake()
    {
        if (managers == null) managers = new List<GameObjectManager>();
        managers.Add(this);
    }
    private void OnDestroy()
    {
        managers.Remove(this);
    }
    void Start()
    {
        _posIniter = new PositionIniter(transform, transform.position, transform.rotation);
        DestroyOtherComponent();
        Create();
    }

    [ContextMenu("Create")]
    public void Create()
    {
        if (_isDontCreate) return;
        if (IsCreated) return;
        _features.GetCustomEditor().Create(gameObject);
        if (_features.Name != "Ice")
        {
            var infoWindow = RequireComponent<ItemDataWindow>(gameObject);
        }
        RequireComponent<AudioSource>(gameObject);
        RequireComponent<SoundManager>(gameObject);
        IsCreated = true;
        if (_isInitPos) _posIniter.SetPos();
        gameObject.SetActive(true);

    }
    [ContextMenu("Destroy")]
    public void Destroy()
    {
        if (!_isDontDestoryChildrens)
            GetGUI?.Destroy(gameObject);
        IsCreated = false;
        gameObject.SetActive(false);
    }
    void DestroyOtherComponent()
    {
        if (_dontDestroy == null) _dontDestroy = new List<Component>();
        _dontDestroy.Add(this);
        _dontDestroy.Add(GetComponent<Transform>());

        if (!_isDontDestoryChildrens)
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i));
            }

        var components = GetComponents<Component>();
        if (!_isDontDestoryComponents)
            for (int i = 0; i < components.Length; i++)
            {
                if (_dontDestroy.Any(x => x.Equals(components[i]))) continue;
                Destroy(components[i]);
            }
    }
    public void Init(Features features, List<Component> dontDestorys = null, bool isInitPos = false)
    {
        _features = features;
        _dontDestroy = dontDestorys;
        _isInitPos = isInitPos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying &&   // 게임 실행 중이 아닐 때만 그립니다.
            _features != null)
        {           // 그릴 대상이 있을 때만 그립니다.
            var modelToRender = _features.GetCustomEditor().GetGizmosMesh();
            if (modelToRender != null)
            {
                // 모델의 위치와 회전을 가져옵니다.
                Vector3 position = Vector3.zero;
                Quaternion rotation = Quaternion.identity;

                // 모델의 위치와 회전을 설정합니다.
                Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);

                // 모델을 그립니다.
                Gizmos.DrawMesh(modelToRender, transform.position, transform.rotation, transform.lossyScale);
                Gizmos.matrix = Matrix4x4.identity;
            }
        }
    }
#endif
}

class PositionIniter
{
    Transform _target;
    Vector3 _firstPos;
    Quaternion _firstRot;

    public PositionIniter(Transform target, Vector3 firstPos, Quaternion firstRot)
    {
        _target = target;
        _firstPos = firstPos;
        _firstRot = firstRot;
    }
    public void SetPos()
    {
        _target.SetPositionAndRotation(_firstPos, _firstRot);
    }
}