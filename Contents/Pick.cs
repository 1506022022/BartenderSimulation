using System.Collections;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;
using static ID;

public class Pick : MonoBehaviour
{
    float _actionDelay = 0.5f;
    bool _isActionDelay;
    GameObject _pickedGarnish;
    [SerializeField] Collider _col;
    [SerializeField] Vector3 pickPos;
    bool IsPicked => _pickedGarnish != null;

    private void OnTriggerEnter(Collider other)
    {
        var manager = other.GetComponent<GameObjectManager>();
        if (manager == null || _isActionDelay) return;
        ItemGroup targetGid = GetGroup(manager.GetID);

        if (!IsPicked && targetGid == ItemGroup.Garnish)
            PickTheGarnish(other);
        else if (IsPicked && targetGid == ItemGroup.Glass)
            DecorateGarnishOnGlass(other);
    }

    void PickTheGarnish(Collider other)
    {
        StartCoroutine(ActionDelay());
        _pickedGarnish = other.gameObject;
        _pickedGarnish.transform.SetParent(transform);
        _pickedGarnish.GetComponent<Rigidbody>().isKinematic = true;
        _pickedGarnish.GetComponents<Collider>().Where(x => !x.isTrigger).ForEach(x => x.enabled = false);
        _pickedGarnish.transform.SetLocalPositionAndRotation(pickPos, Quaternion.identity);

        Debug.Log(_col.bounds.center.z);
    }
    void DecorateGarnishOnGlass(Collider other)
    {
        StartCoroutine(ActionDelay());
        _pickedGarnish.transform.SetParent(other.transform);
        _pickedGarnish = null;
    }
    IEnumerator ActionDelay()
    {
        _isActionDelay = true;
        yield return new WaitForSeconds(_actionDelay);
        _isActionDelay = false;
    }
}
