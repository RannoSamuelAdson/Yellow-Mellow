using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button settingsButton;
    public Button backButton;
    public Button quitButton;
    public GameObject SettingsPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsButton.onClick.AddListener(OpenSettings);
        backButton.onClick.AddListener(CloseMenu);
        quitButton.onClick.AddListener(Quit);
    }
    private void OpenSettings()
    {
        SettingsPanel.SetActive(true);
    }
    private void Quit()
    {
        Application.Quit();
    }

    private void CloseMenu()
    {
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
