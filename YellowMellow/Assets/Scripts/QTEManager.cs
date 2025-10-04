using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEManager : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform movingLine;
    public RectTransform hitZone;
    public RectTransform movementArea; // usually the background bar
    public TextMeshProUGUI resultText;

    [Header("Settings")]
    public float moveSpeed = 500f;
    public float hitZoneMinWidth = 20f;
    public float hitZoneMaxWidth = 50f;
    public float flashDuration = 0.3f;
    public int flashCount = 3;
    public KeyCode actionKey = KeyCode.Space;

    [Header("Feedback")]
    public ParticleSystem successFX;
    public ParticleSystem failFX;

    [Header("Colors")]
    public Color successColor = Color.green;
    public Color failColor = Color.red;
    private Color originalColor;

    private bool movingRight = true;
    private bool isActive = true;
    private GameObject currentWealthyHuman;

    public List<GameObject> itemOptions = new List<GameObject>();

    public Player player;
    private void Start()
    {
        originalColor = Color.green;
    }

    void Update()
    {
        if (!isActive) return;

        MoveLine();
        CheckInput();
    }

    void MoveLine()
    {
        float direction = movingRight ? 1f : -1f;
        movingLine.anchoredPosition += new Vector2(direction * moveSpeed * Time.deltaTime, 0f);

        // Bounds check (based on parent)
        float halfAreaWidth = movementArea.rect.width / 2f;
        float halfLineWidth = movingLine.rect.width / 2f;

        // Clamp and reverse direction
        if (movingLine.anchoredPosition.x >= halfAreaWidth - halfLineWidth)
        {
            movingRight = false;
        }
        else if (movingLine.anchoredPosition.x <= -halfAreaWidth + halfLineWidth)
        {
            movingRight = true;
        }
    }
    public GameObject GetRandomItem()
    {
        if (itemOptions == null || itemOptions.Count == 0)
        {
            Debug.LogWarning("Item list is empty!");
            return null;
        }

        int index = Random.Range(0, itemOptions.Count); // upper bound is exclusive
        return Instantiate(itemOptions[index], player.transform);
    }

    void CheckInput()
    {
        //if (Player.playerPaused) return;
        if (Input.GetKeyDown(actionKey))
        {
            if (IsLineOverHitZone())
            {
                Debug.Log("✅ HIT!");
                Success();
            }
            else
            {
                Debug.Log("❌ MISS!");
                Fail();
            }

        }
    }

    bool IsLineOverHitZone()
    {
        float lineX = movingLine.anchoredPosition.x;
        float zoneLeft = hitZone.anchoredPosition.x - hitZone.rect.width / 2f;
        float zoneRight = hitZone.anchoredPosition.x + hitZone.rect.width / 2f;

        return lineX >= zoneLeft && lineX <= zoneRight;
    }

    void Success()
    {
        isActive = false;
        // Add success logic here (animation, next stage, etc.)
        successFX?.Play();
        StartCoroutine(AnimateHitZoneResult(successColor));
        ShowResultText("Perfect!", Color.green);
        player.StolenItem = GetRandomItem();
        
    }

    void Fail()
    {
        isActive = false;
        // Add fail logic here (shake, retry, etc.)
        failFX?.Play();
        StartCoroutine(AnimateHitZoneResult(failColor));
        ShowResultText("Miss!", Color.red);
    }

    // Optional: call this to start a new attempt
    public void RestartQTE(GameObject wealthyHuman = null)
    {
        currentWealthyHuman = wealthyHuman;
        Player.playerPaused = true; // Ensure game is paused
        gameObject.SetActive(true); // Ensure UI is visible
        StopAllCoroutines(); // just in case
        isActive = false;

        movingRight = Random.Range(0, 2) == 1;
        movingLine.anchoredPosition = new Vector2(movingRight ? -movementArea.rect.width / 2f : movementArea.rect.width / 2f, movingLine.anchoredPosition.y);
        RandomizeHitZonePosition();

        StartCoroutine(StartAfterFlash());

    }

    void RandomizeHitZonePosition()
    {
        float areaWidth = movementArea.rect.width;
        float zoneWidth = hitZone.rect.width;

        float maxX = (areaWidth - zoneWidth) / 2f;
        float randomX = Random.Range(-maxX, maxX);

        hitZone.anchoredPosition = new Vector2(randomX, hitZone.anchoredPosition.y);

        // Reset scale and animate in
        hitZone.localScale = Vector3.zero;
        StartCoroutine(AnimateHitZoneAppear());

        // Optionally randomize hit zone width
        float newWidth = Random.Range(hitZoneMinWidth, hitZoneMaxWidth);
        hitZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }

    void ShowResultText(string text, Color color)
    {
        StopCoroutine(nameof(FadeResultText));
        resultText.text = text;
        resultText.color = new Color(color.r, color.g, color.b, 1f);
        StartCoroutine(FadeResultText());
    }

    IEnumerator FadeResultText()
    {
        float duration = 1f;
        float t = 0;
        Color startColor = resultText.color;

        while (t < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            resultText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }

        resultText.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        Player.SetGamePaused(false); // Ensure game is not paused
        if (currentWealthyHuman != null)
        {
            Destroy(currentWealthyHuman);
            currentWealthyHuman = null;
        }
        gameObject.SetActive(false); // Hide QTE UI 

    }
    IEnumerator AnimateHitZoneAppear()
    {
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            hitZone.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        hitZone.localScale = Vector3.one;
    }

    IEnumerator FlashHitZone()
    {
        Image zoneImage = hitZone.GetComponent<Image>();
        Color originalColor = zoneImage.color;
        Color flashColor = Color.white;

        for (int i = 0; i < flashCount; i++)
        {
            zoneImage.color = flashColor;
            yield return new WaitForSecondsRealtime(flashDuration / (flashCount * 2));
            zoneImage.color = originalColor;
            yield return new WaitForSecondsRealtime(flashDuration / (flashCount * 2));
        }

        zoneImage.color = originalColor;
    }

    IEnumerator StartAfterFlash()
    {
        yield return FlashHitZone();

        // Small delay after flash if needed
        yield return new WaitForSecondsRealtime(0.2f);
        isActive = true; // Now start the QTE movement



    }

    IEnumerator AnimateHitZoneResult(Color targetColor)
    {
        Image img = hitZone.GetComponent<Image>();
        float duration = 0.3f;
        float t = 0;

        while (t < duration)
        {
            img.color = Color.Lerp(img.color, targetColor, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        img.color = targetColor;
        yield return new WaitForSecondsRealtime(0.5f);

        // Reset back to original
        /*t = 0;
        while (t < duration)
        {
            img.color = Color.Lerp(img.color, originalColor, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        img.color = originalColor;*/
        isActive = true; // Now start the QTE movement

    }
}