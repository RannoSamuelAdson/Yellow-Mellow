using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle muteToggle;
    public Button backButton;
    private float lastVolume = 1f; // to remember volume before mute

    void Start()
    {
        // Initialize UI with current volume
        volumeSlider.value = AudioListener.volume;
        muteToggle.isOn = AudioListener.volume <= 0f;

        // Add listeners
        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteToggle.onValueChanged.AddListener(ToggleMute);
        backButton.onClick.AddListener(CloseSettings);
    }
    private void CloseSettings()
    {
        this.gameObject.SetActive(false);
    }
    private void SetVolume(float value)
    {
        AudioListener.volume = value;
        if (value > 0f) lastVolume = value;

        // Update mute toggle automatically
        muteToggle.isOn = (value <= 0f);
    }

    private void ToggleMute(bool isMuted)
    {
        if (isMuted)
        {
            lastVolume = AudioListener.volume > 0f ? AudioListener.volume : lastVolume;
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = lastVolume;
            volumeSlider.value = lastVolume;
        }
    }
}
