using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nest : MonoBehaviour
{
    private List<ValuableItem> hoard = new List<ValuableItem>();
    public float netWorth = 0f;
    public TMP_Text netWorthText;
    public Timer timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ValuableItem>(out ValuableItem item))
        {
            hoard.Add(item);
            netWorth += item.value;
            Destroy(item.gameObject);
            netWorthText.text = "Net worth: " + netWorth.ToString("F2") + "$";
            timer.timeRemaining += item.value * 2f; // Add item's value to the timer
        }

        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
