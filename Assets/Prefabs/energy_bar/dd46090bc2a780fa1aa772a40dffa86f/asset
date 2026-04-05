using UnityEngine;

public class CatSmack : MonoBehaviour
{
    public float smackRange = 1.5f;
    public LayerMask smackLayers;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward, smackRange, smackLayers);

            foreach (Collider hit in hits)
            {
                Debug.Log("Smacked: " + hit.name);

                Rigidbody rb = hit.attachedRigidbody;
                if (rb != null)
                {
                    rb.AddForce(transform.forward * 4f + Vector3.up * 2f, ForceMode.Impulse);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.forward, smackRange);
    }
}
