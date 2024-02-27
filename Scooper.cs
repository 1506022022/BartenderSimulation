using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Scooper : MonoBehaviour
{
    GameObject _originFoodIngredients;
    LocalTransformData firstTransform;
    Transform _activeFoodIngredients;
    public bool IsScoopedUp { get; private set; }
    [SerializeField] List<int> ScoopTargets;
    [SerializeField] List<int> DropTargets;
    void Awake()
    {
        _originFoodIngredients = transform.GetChild(0).gameObject;
        firstTransform = new LocalTransformData(transform.GetChild(0));
    }
    void OnDisable()
    {
        if (IsScoopedUp)
        {
            Destroy(_activeFoodIngredients.gameObject);
            Drop();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        var manager = other.GetComponent<GameObjectManager>();
        if (manager == null) return;

        int otherID = manager.GetID;
        bool isScoopTarget = ScoopTargets.Any(x => x == otherID);
        bool isDropTarget = DropTargets.Any(x=>x == otherID);

        if (!IsScoopedUp && isScoopTarget)
            ScoopUp();

        if (IsScoopedUp && isDropTarget)
            Drop();

    }
    void ScoopUp()
    {
        ActiveFoodIngredients();
        IsScoopedUp = true;
    }
    void ActiveFoodIngredients()
    {
        bool isExits = false;
        for (int i = 1; i < transform.childCount; i++)
        {
            var suger = transform.GetChild(i);
            if (!suger.gameObject.activeSelf)
            {
                InitFoodIngredients(suger);
                break;
            }
        }
        if (!isExits)
        {
            var suger = Instantiate(_originFoodIngredients, transform).transform;
            InitFoodIngredients(suger);
        }

        void InitFoodIngredients(Transform foodIngredients)
        {
            foodIngredients.gameObject.SetActive(true);
            LocalTransformData.SetTransformAtoB(foodIngredients, firstTransform);
            foodIngredients.GetComponentsInChildren<Rigidbody>()?.ForEach(x => x.isKinematic = true);
            foodIngredients.GetComponentsInChildren<Rigidbody>().ForEach(x => x.velocity = Vector3.zero);
            foodIngredients.GetComponentsInChildren<Rigidbody>().ForEach(x=>x.detectCollisions = false);
            isExits = true;
            _activeFoodIngredients = foodIngredients;
        }

    }
    void Drop()
    {
        _activeFoodIngredients.GetComponentsInChildren<Rigidbody>()?.ForEach(x=>x.isKinematic = false);
        _activeFoodIngredients.GetComponentsInChildren<Rigidbody>().ForEach(x => x.detectCollisions = true);
        _activeFoodIngredients.SetParent(null);
        _activeFoodIngredients = null;
        IsScoopedUp = false;
    }
}
public class LocalTransformData
{
    Vector3 _position;
    Vector3 _eulerAngles;
    Vector3 _scale;
    public LocalTransformData(Transform transform)
    {
        _position = transform.localPosition;
        _eulerAngles = transform.localEulerAngles;
        _scale = transform.localScale;
    }
    public static void  SetTransformAtoB(Transform a, LocalTransformData b)
    {
        a.localPosition = b._position;
        a.localEulerAngles = b._eulerAngles;
        a.localScale = b._scale;
    }
}


