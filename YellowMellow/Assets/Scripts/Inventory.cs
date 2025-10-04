using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ValuableItem[] stolenItems;
    public int carryLimit = 3;
    public List<GameObject> UIItems = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StealItem(ValuableItem item)
    {
        // First, check if there is an empty slot
        for (int i = 0; i < stolenItems.Length; i++)
        {
            if (stolenItems[i] == null) // vacant slot
            {
                stolenItems[i] = item;
                return; // we're done
            }
        }

        // If we reached here, there were no empty slots -> inventory full
        int indexOfLeastValuable = 0;
        float leastValue = float.MaxValue;

        for (int i = 0; i < stolenItems.Length; i++)
        {
            if (stolenItems[i].value < leastValue)
            {
                leastValue = stolenItems[i].value;
                indexOfLeastValuable = i;
            }
        }

        // Drop the least valuable one
        stolenItems[indexOfLeastValuable].Drop();

        // Replace it with the new item
        stolenItems[indexOfLeastValuable] = item;
    }
}
