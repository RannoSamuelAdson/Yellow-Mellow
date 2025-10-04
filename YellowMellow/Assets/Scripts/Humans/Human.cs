using UnityEngine;

public class Human : MonoBehaviour
{
    public Transform goalpoint;
    public float Speed = 3f;

    void Update()
    {
        if (goalpoint == null) return;
        if (Player.playerPaused) return;

        // Move towards the goal point
        transform.position = Vector3.MoveTowards(
            transform.position,
            goalpoint.position,
            Speed * Time.deltaTime
        );

        if (goalpoint.position.x < transform.position.x)
        {
            // Moving left → face left (rotate 180° on Y)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (goalpoint.position.x > transform.position.x)
        {
            // Moving right → face right (no rotation)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }


        // Destroy if reached
        if (Vector3.Distance(transform.position, goalpoint.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
