using UnityEngine;
using UnityEngine.UI;

public class UIButtonSoundPlayer : MonoBehaviour
{
    public SoundRandomizer clickSoundRandomizer;

    void Start()
    {
        

        // Find all buttons in the scene
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (Button btn in buttons)
        {
            // Remove old listeners just in case, then add our click listener
            btn.onClick.AddListener(() => clickSoundRandomizer.PlayRandomSound());
        }
    }

    
}
