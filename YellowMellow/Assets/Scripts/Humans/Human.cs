using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour
{
    public Transform goalpoint;
    public float Speed = 3f;
    public QTEManager qteManager; // moved here
    public GameObject tutorialText;
    public GameObject sprite;
    public GameObject wealthyIndicator;

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
    private void OnTriggerEnter(Collider other)
    {
        if(Player.playerPaused) return;

        if (!isWealthy) {
            if (other.gameObject.CompareTag("ValuableItem") && !other.GetComponent<Rigidbody>().isKinematic)
            {
                StartWealthyCountdown();
                Destroy(other.gameObject);
            }
            return; 
        }
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
            sprite.GetComponent<SpriteRenderer>().color = originalColor;
            yield return new WaitForSeconds(flashDuration / (flashCount * 2));
            sprite.GetComponent<SpriteRenderer>().color = flashColor;
            yield return new WaitForSeconds(flashDuration / (flashCount * 2));
        }
        sprite.GetComponent<SpriteRenderer>().color = Color.yellow;

        isWealthy = true; // Now start the QTE movement



    }


    private void HandleRotation()
    {
        if (goalpoint.position.x < transform.position.x)
            sprite.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else if (goalpoint.position.x > transform.position.x)
            sprite.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (!isWealthy && other.gameObject.TryGetComponent<ValuableItem>(out ValuableItem item))
        {
            UpgradeToWealthy();
            Destroy(other);
        }
    }*/
    public void setTutorialActiveState(bool state)
    {
        if (!isWealthy) return;
        tutorialText.SetActive(state);
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