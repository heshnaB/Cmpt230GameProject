using UnityEngine;

// Class for Max reaching for anything (Usually Clementine)!
public class DogReach : MonoBehaviour
{
    [Header("Targets")]
    public Transform catTarget;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float turnSpeed = 5f;
    public float stopDistance = 1.5f;

    [Header("Reach Up Bones")]
    public Transform spine1;   // drag spine.006
    public Transform spine2;   // drag spine.007

    [Header("Reach Settings")]
    public float reachSpeed = 3f;
    public float heightThreshold = 0.8f;

    private Quaternion spine1Start;
    private Quaternion spine2Start;

    void Start()
    {
        if (spine1 != null) spine1Start = spine1.localRotation;
        if (spine2 != null) spine2Start = spine2.localRotation;
    }

    void Update()
    {
        if (catTarget == null) return;

        Vector3 flatTarget = new Vector3(catTarget.position.x, transform.position.y, catTarget.position.z);
        Vector3 toTarget = flatTarget - transform.position;
        float distance = toTarget.magnitude;

        bool catIsHigh = catTarget.position.y > transform.position.y + heightThreshold;
        bool closeEnoughToReach = distance < stopDistance + 0.5f;

        if (!catIsHigh || !closeEnoughToReach)
        {
            // Chase on ground
            if (distance > stopDistance)
            {
                Vector3 moveDir = toTarget.normalized;
                transform.position += moveDir * moveSpeed * Time.deltaTime;

                if (moveDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(moveDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
                }
            }
        }

        // Reach up if cat is high and nearby
        float reachAmount = (catIsHigh && closeEnoughToReach) ? 1f : 0f;

        if (spine1 != null)
        {
            Quaternion target = spine1Start * Quaternion.Euler(-20f * reachAmount, 0f, 0f);
            spine1.localRotation = Quaternion.Slerp(spine1.localRotation, target, reachSpeed * Time.deltaTime);
        }

        if (spine2 != null)
        {
            Quaternion target = spine2Start * Quaternion.Euler(-35f * reachAmount, 0f, 0f);
            spine2.localRotation = Quaternion.Slerp(spine2.localRotation, target, reachSpeed * Time.deltaTime);
        }
    }
}
