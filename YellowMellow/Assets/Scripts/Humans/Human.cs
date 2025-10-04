using UnityEngine;

public class Human : MonoBehaviour
{
    public Transform goalpoint;
    public float Speed = 3f;

    void Update()
    {
        if (goalpoint == null) return;

        // Move towards the goal point
        transform.position = Vector3.MoveTowards(
            transform.position,
            goalpoint.position,
            Speed * Time.deltaTime
        );

        // Destroy if reached
        if (Vector3.Distance(transform.position, goalpoint.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
