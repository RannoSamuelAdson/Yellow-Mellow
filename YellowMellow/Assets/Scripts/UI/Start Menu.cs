using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button startGameButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startGameButton.onClick.AddListener(DisableStartMenu);
    }
    private void DisableStartMenu()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
