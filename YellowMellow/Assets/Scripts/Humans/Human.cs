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
            // Moving left → face left
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else if (goalpoint.position.x > transform.position.x)
        {
            // Moving right → face right
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }


        // Destroy if reached
        if (Vector3.Distance(transform.position, goalpoint.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
