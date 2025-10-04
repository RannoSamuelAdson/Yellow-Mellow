using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    private List<ValuableItem> hoard = new List<ValuableItem>();
    public float netWorth = 0f;
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
            Destroy(other);
        }

        ;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
