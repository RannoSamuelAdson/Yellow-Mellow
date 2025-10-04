using UnityEngine;

public class WealthyHuman : Human
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            Debug.Log("Wealthy human intercepted");
            return;
        }
    }
}
