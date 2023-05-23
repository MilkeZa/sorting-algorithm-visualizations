using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlowController : MonoBehaviour
{
    [Header("Game Areas")]
    public GameObject mainMenuArea;
    public GameObject settingsMenuArea;
    public GameObject simulationArea;
    public GameObject tileContainer;

    [Header("Settings")]
    public TMP_InputField randomSeedInputField;

    public Toggle fpsCounterToggle;
    public TMP_Text fpsCounterLabel;

    private void Awake()
    {
        // Set the initial state of the FPS counter.
        fpsCounterLabel.gameObject.SetActive(fpsCounterToggle.isOn);
    }

    public void SetMainMenu()
    {
        mainMenuArea.SetActive(true);
        settingsMenuArea.SetActive(false);

        simulationArea.SetActive(false);
        tileContainer.SetActive(false);
    }

    public void SetSettingsMenu()
    {
        mainMenuArea.SetActive(false);
        settingsMenuArea.SetActive(true);

        simulationArea.SetActive(false);
        tileContainer.SetActive(false);
    }

    public void SetSimulation()
    {
        mainMenuArea.SetActive(false);
        settingsMenuArea.SetActive(false);

        simulationArea.SetActive(true);
        tileContainer.SetActive(true);
    }

    public void SetRandomSeedInput(bool isRandomSeed)
    {
        if (isRandomSeed)
        {
            randomSeedInputField.text = string.Empty;
            randomSeedInputField.interactable = false;
        }
        else
        {
            randomSeedInputField.interactable = true;
        }
    }

    public void SetFPSCounter(bool showFPSCounter)
    {
        if (showFPSCounter)
        {
            // Enable FPS counter to show text.
            fpsCounterLabel.gameObject.SetActive(true);
        }
        else
        {
            // Disable FPS counter to hide text.
            fpsCounterLabel.gameObject.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();

        // Remove for release.
        Debug.Log("Quit Application");
    }
}
