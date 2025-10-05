using UnityEngine;
using UnityEngine.UI;
public enum TutorialState
{
    StartPanel,
    MoveTutorial,
    FindPeopleTutorial,
    MinigameTutorial,
    ReturnTutorial,
    TutorialOver
}
public class Tutorial : MonoBehaviour
{
    public Button startGameButton;
    public GameObject startPanel;
    public GameObject moveTutuorial;
    public GameObject findPeopleTutorial;
    public GameObject minigameTutorial;
    public GameObject returnTutorial;
    public QTEManager QTEmanager;
    public Nest nest;
    private TutorialState tutorialState = TutorialState.StartPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startGameButton.onClick.AddListener(StartTutorial);
    }
    private void StartTutorial()
    {
        startPanel.gameObject.SetActive(false);
        tutorialState = TutorialState.MoveTutorial;
        moveTutuorial.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        switch (tutorialState)
        {
            case TutorialState.MoveTutorial:
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                    moveTutuorial.SetActive(false);
                    findPeopleTutorial.SetActive(true);
                    tutorialState = TutorialState.FindPeopleTutorial;
                break;
            }
            case TutorialState.FindPeopleTutorial:
            {
                if (QTEmanager.isActive)
                    findPeopleTutorial.SetActive(false);
                    minigameTutorial.SetActive(true);
                    tutorialState = TutorialState.MinigameTutorial;
                break;
            }
            case TutorialState.MinigameTutorial:
            {
                if (QTEmanager.wasSuccessful)
                    minigameTutorial.SetActive(false);
                    returnTutorial.SetActive(true);
                    tutorialState = TutorialState.ReturnTutorial;
                break;
            }
            case TutorialState.ReturnTutorial:
            {
                if (nest.netWorth > 0)
                    returnTutorial.SetActive(false);
                    tutorialState = TutorialState.TutorialOver;
                break;
            }

            default: 
                break;
        }
    }
}
