using UnityEngine;

public class ValuableItem : MonoBehaviour
{
    public Sprite sprite;
    public float value = 1f;

    public 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Drop()
    {

    }
}
