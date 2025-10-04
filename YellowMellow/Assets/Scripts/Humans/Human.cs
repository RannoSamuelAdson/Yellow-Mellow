using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour
{
    public Transform goalpoint;
    public float Speed = 3f;
    public QTEManager qteManager;
    public bool isWealthy = false;

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
    private void OnTriggerEnter(Collider other)
    {
        if(!isWealthy) return;
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            Debug.Log("Wealthy human intercepted");
            qteManager.RestartQTE(this);
        }
    }
    public void StartWealthyCountdown()
    {
        StartCoroutine(CanInteractAfterDelay());
    }

    IEnumerator CanInteractAfterDelay()
    {
        Speed *= 1.5f; // Speed up for dramatic effect
        isWealthy = false; // Temporarily disable interaction
        Color originalColor = Color.white;
        Color flashColor = Color.yellowGreen;

        int flashCount = 5;
        float flashDuration = 2f;

        for (int i = 0; i < flashCount; i++)
        {
            GetComponent<SpriteRenderer>().color = originalColor;
            yield return new WaitForSecondsRealtime(flashDuration / (flashCount * 2));
            GetComponent<SpriteRenderer>().color = flashColor;
            yield return new WaitForSecondsRealtime(flashDuration / (flashCount * 2));
        }
        GetComponent<SpriteRenderer>().color = Color.yellow;

        isWealthy = true; // Now start the QTE movement



    }
}
