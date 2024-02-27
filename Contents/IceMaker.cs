using System.Collections.Generic;
using UnityEngine;

public class IceMaker : MonoBehaviour
{
    [SerializeField] Vector2 _makeRange;
    [SerializeField] GameObject _ice;
    [SerializeField] int MaxIceCount = 20;
    List<GameObject> Ices;
    const int makeCount = 5;
    private void Awake()
    {
        Ices = new List<GameObject>();
        for (int i = 0; i < makeCount; i++)
        {
            if (Ices.Count >= MaxIceCount) break;
            var instance = Instantiate(_ice, GetRandomPos(new Vector3(-3.06999993f, -3.82999992f, -21.5769997f)), Quaternion.identity);
            Ices.Add(instance);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag != "Scoop") return;
        for (int i = 0; i < makeCount; i++)
        {
            if (Ices.Count >= MaxIceCount) break;
            var instance = Instantiate(_ice, GetRandomPos(collision.transform.position), Quaternion.identity);
            Ices.Add(instance);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < makeCount; i++)
        {
            if (Ices.Count >= MaxIceCount) break;
            var instance = Instantiate(_ice, GetRandomPos(other.transform.position), Quaternion.identity);
            Ices.Add(instance);
        }
    }
    Vector3 GetRandomPos(Vector3 pos)
    {
        pos.x += Random.Range(-_makeRange.x, _makeRange.x);
        pos.y += 0.01f;
        pos.z += Random.Range(-_makeRange.y, _makeRange.y);
        return pos;
    }
}
