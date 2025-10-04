using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button settingsButton;
    public Button backButton;
    public Button quitButton;
    public GameObject SettingsPanel;
    public GameObject Overlay;
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
        Overlay.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        // Check if ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }
}
