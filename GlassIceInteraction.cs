using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassIceInteraction : MonoBehaviour
{
    [SerializeField] private Transform ices;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ice"))
        {
            Debug.Log("�����̶� �浹 �ߴ�");
            other.gameObject.transform.parent = ices;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ice"))
        {
            other.gameObject.transform.parent = null;
        }
    }
}
