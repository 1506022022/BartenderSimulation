using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Checker;

public class GameObjectManager : MonoBehaviour
{
    #region static
    static List<GameObjectManager> managers;

    // 씬 내의 등록된 오브젝트들 삭제 후 재생성
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
    // 등록
        if (managers == null) managers = new List<GameObjectManager>();
        managers.Add(this);
    }
    private void OnDestroy()
    {
    // 제거
        managers.Remove(this);
    }
    void Start()
    {
    // 위치값 초기화, 필요없는 컴포넌트 제거, 아이템에 필요한 컴포넌트 부착
        _posIniter = new PositionIniter(transform, transform.position, transform.rotation);
        DestroyOtherComponent();
        Create();
    }

// 위치값 초기화
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
