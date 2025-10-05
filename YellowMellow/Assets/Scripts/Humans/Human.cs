using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Human : MonoBehaviour
{
    public Transform goalpoint;
    public float Speed = 3f;
    public QTEManager qteManager; // moved here
    public GameObject tutorialText;
    public GameObject sprite;
    public GameObject wealthyIndicator;
    public Sprite[] valuableItemSprites; // Array of item sprites
    public AnimatorController[] animatorControllers; // Reference to Animator Controllers

    public bool isWealthy = false; // track if upgraded
    public List<GameObject> itemOptions = new List<GameObject>();
    public GameObject chosenItem = null;


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
    public void GetRandomItem()
    {
        if (itemOptions == null || itemOptions.Count == 0)
        {
            Debug.LogWarning("Item list is empty!");
            return;
        }

        int index = Random.Range(0, itemOptions.Count); // upper bound is exclusive
        chosenItem = itemOptions[index];
    }
    private void OnTriggerEnter(Collider other)
    {
        if(Player.playerPaused) return;

        if (!isWealthy) {
            if (other.gameObject.CompareTag("ValuableItem") && !other.GetComponent<Rigidbody>().isKinematic)
            {
                var value = other.GetComponent<ValuableItem>().value;
                chosenItem = other.gameObject;
                SetIndicatorVisuals(value);
                StartWealthyCountdown();
                wealthyIndicator.SetActive(true);
                Destroy(other.gameObject);
            }
            return; 
        }
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            Debug.Log("Wealthy human intercepted");
            qteManager.RestartQTE(this, itemOptions.IndexOf(chosenItem));
        }
    }
    public void SetIndicatorVisuals(float value)
    {
        if (value < 5)
        {
            wealthyIndicator.GetComponent<Animator>().runtimeAnimatorController = animatorControllers[0];
            wealthyIndicator.GetComponent<SpriteRenderer>().sprite = valuableItemSprites[0];

        }
        else if (value < 10)
        {
            wealthyIndicator.GetComponent<Animator>().runtimeAnimatorController = animatorControllers[1];
            wealthyIndicator.GetComponent<SpriteRenderer>().sprite = valuableItemSprites[1];

        }
        else
        {
            wealthyIndicator.GetComponent<Animator>().runtimeAnimatorController = animatorControllers[2];
            wealthyIndicator.GetComponent<SpriteRenderer>().sprite = valuableItemSprites[2];

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
        SpriteRenderer spriterenderer = sprite.GetComponent<SpriteRenderer>();
        int flashCount = 5;
        float flashDuration = 2f;

        for (int i = 0; i < flashCount; i++)
        {
            float t = 0f;
            while (t < flashDuration / (flashCount * 2))
            {
                float alpha = Mathf.Lerp(1f, 0.3f, t / (flashDuration / (flashCount * 2)));
                spriterenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                t += Time.deltaTime;
                yield return null;
            }

            t = 0f;
            while (t < flashDuration / (flashCount * 2))
            {
                float alpha = Mathf.Lerp(0.3f, 1f, t / (flashDuration / (flashCount * 2)));
                spriterenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                t += Time.deltaTime;
                yield return null;
            }
        }
        //sprite.GetComponent<SpriteRenderer>().color = Color.yellow;

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