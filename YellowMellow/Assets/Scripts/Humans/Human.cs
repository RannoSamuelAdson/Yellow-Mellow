
using UnityEngine;

public class Human : MonoBehaviour
{
    public Transform goalpoint;
    public float Speed = 3f;
    public QTEManager qteManager; // moved here

    public bool isWealthy = false; // track if upgraded

    void Update()
    {
        if (goalpoint == null || Player.playerPaused) return;

        // Move towards the goal
        transform.position = Vector3.MoveTowards(
            transform.position,
            goalpoint.position,
            Speed * Time.deltaTime
        );

        HandleRotation();

        // Destroy if reached
        if (Vector3.Distance(transform.position, goalpoint.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void HandleRotation()
    {
        if (goalpoint.position.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else if (goalpoint.position.x > transform.position.x)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isWealthy && other.gameObject.TryGetComponent<ValuableItem>(out ValuableItem item))
        {
            UpgradeToWealthy();
            Destroy(other);
        }
    }

    private void UpgradeToWealthy()
    {
        isWealthy = true;

        // Add the WealthyHuman component dynamically
        WealthyHuman wealthy = gameObject.AddComponent<WealthyHuman>();
        wealthy.qteManager = qteManager; // pass reference
        wealthy.goalpoint = goalpoint;

        // Optionally disable this Human component so only WealthyHuman logic runs
        this.enabled = false;

        Debug.Log("Human upgraded to WealthyHuman!");
    }
}