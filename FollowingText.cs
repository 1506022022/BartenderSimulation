using System.Collections.Generic;
using UnityEngine;

public class FollowingText : MonoBehaviour
{
    public static List<FollowingText> Instances = new List<FollowingText>();
    [SerializeField] Transform ment;
    Vector3 distance;

    
    private void Awake()
    {
        Instances.Add(this);
        distance = ment.localPosition;
        Reposition();
    }
    public void Reposition()
    {
        ment.SetParent(transform);
        ment.transform.SetLocalPositionAndRotation(distance, Quaternion.identity);
        ment.SetParent(null);
    }
    public void StopMent()
    {
        ment.gameObject.SetActive(false);
    }
}
