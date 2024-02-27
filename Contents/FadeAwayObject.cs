using UnityEngine;

public class FadeAwayObject : MonoBehaviour
{
    private void Update()
    {
        var rigid = transform.GetComponent<Rigidbody>();
        var isKinematic = rigid.isKinematic;
        if (isKinematic) return;
        transform.localScale *= 0.97f;
        if (transform.localScale.magnitude < 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
