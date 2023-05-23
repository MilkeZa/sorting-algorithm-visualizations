using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // System Settings --------------------------------------------------------
    [Header("System Settings")]
    public AudioMixer masterAudioMixer;

    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    // Simulation Settings ----------------------------------------------------
    [Header("Simulation Settings")]
    public GameObject tileContainer;
    SimulationController simulationController;

    public TMP_InputField tileCountInputField;

    public TMP_InputField tileScaleXInputField;
    public TMP_InputField tileScaleYInputField;

    public Toggle randomSeedToggle;
    public TMP_InputField randomSeedInputField;

    public TMP_Dropdown sortingAlgoDropdown;
    List<string> sortingAlgorithmOptions;

    public TMP_InputField correctIndexR;
    public TMP_InputField correctIndexG;
    public TMP_InputField correctIndexB;
    public Image correctIndexResult;
    public TMP_InputField incorrectIndexR;
    public TMP_InputField incorrectIndexG;
    public TMP_InputField incorrectIndexB;
    public Image incorrectIndexResult;

    private void Awake()
    {
        simulationController = GetComponent<SimulationController>();

        // Set the resolution.
        ParseResolutions();

        // Parse the sorting algorithm options and set the currently selected.
        ParseSortingAlgorithms();

        // Set the index colors within the settings menu.
        SetColor("0");
        SetColor("1");
    }

    public void ParseSortingAlgorithms()
    {
        sortingAlgorithmOptions = simulationController.GetSortingAlgorithms();
        sortingAlgoDropdown.AddOptions(sortingAlgorithmOptions);
        sortingAlgoDropdown.value = 0;
        sortingAlgoDropdown.RefreshShownValue();

        SetSortingAlgorithm();
    }

    public void SetSortingAlgorithm()
    {
        simulationController.SetSortingAlgorithm(sortingAlgoDropdown.value);

        // Remove for release
        Debug.Log($"Set Sorting Algorithm to {sortingAlgorithmOptions[sortingAlgoDropdown.value]}");
    }

    void ParseResolutions()
    {
        // Grab the available resolutions for the screen in use.
        resolutions = Screen.resolutions;

        // Remove the default options.
        resolutionDropdown.ClearOptions();

        /*
         * Dropdown AddOptions method takes an array of strings. In order to add the
         * resolution options to the dropdown, we must first convert each resolution
         * into a string. Second, we will add each string to a list of strings and
         * finally, we will pass the list of resolution strings as a parameter to the
         * AddOptions method of the dropdown.
         */

        List<string> resolutionStrings = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            resolutionStrings.Add(option);

            if (resolutions[i].Equals(Screen.currentResolution))
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(resolutionStrings);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Remove for release
        Debug.Log($"Set Resolution to {resolution.width} x {resolution.height}");
    }

    public void SetVolume(float volume)
    {
        masterAudioMixer.SetFloat("masterVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    /* 
     * Is this method necessary? Unity allows you to restrict input to only integer values, so 
     * I can't really say if I even need this. 
    */
    int ValidateTileCount(string tileCountInput)
    {
        // Set the default tile count should input be invalid.
        int tileCount = 50;

        // Trim the input of any whitespace.
        tileCountInput = tileCountInput.Trim();

        // Verify that the string is not empty and is made of only digits.
        if (!tileCountInput.Equals("") && tileCountInput.All(char.IsDigit))
        {
            // Convert the validated string to an integer.
            tileCount = int.Parse(tileCountInput);
        }

        return tileCount;
    }

    public void SetTileCount(string tileCountInput = "50")
    {
        int tileCount = ValidateTileCount(tileCountInput);
        simulationController.tileCount = tileCount;
    }

    float ValidateScaleValue(string valueString)
    {
        // Trim the value, then remove any non-digit characters (excluding decimal sign).
        valueString = Regex.Replace(valueString, "[^.0-9]", "");

        // Convert string to float.
        return float.Parse(valueString);
    }

    public void SetTileScale()
    {
        float xScale = ValidateScaleValue(tileScaleXInputField.text);
        float yScale = ValidateScaleValue(tileScaleYInputField.text);
        Vector2 tileScale = new Vector2(xScale, yScale);

        simulationController.tileScale = tileScale;
    }

    public void SetRandomSeed()
    {
        simulationController.seed = randomSeedInputField.text;
    }

    int ValidateRGBValue(string rgbValueString)
    {
        rgbValueString = Regex.Replace(rgbValueString, "[^0-9]", "");
        int rgbValue = int.Parse(rgbValueString);
        if (rgbValue < 0)
        {
            rgbValue = 0;
        }
        else if (rgbValue > 255)
        {
            rgbValue = 255;
        }
        return rgbValue;
    }

    public void SetColor(string colorIndexString)
    {
        int resultIndex;
        int[] rgbValues = new int[3];
        TMP_InputField inputR, inputG, inputB;

        // Determine whether input is destined for Correct or Incorrect index result.
        if (colorIndexString.Equals("0"))
        {
            resultIndex = 0;
            inputR = correctIndexR;
            inputG = correctIndexG;
            inputB = correctIndexB;
        }
        else
        {
            resultIndex = 1;
            inputR = incorrectIndexR;
            inputG = incorrectIndexG;
            inputB = incorrectIndexB;
        }

        // Parse the RGB values assocaited with the input fields, create the updated color, and apply it.
        rgbValues[0] = ValidateRGBValue(inputR.text);
        rgbValues[1] = ValidateRGBValue(inputG.text);
        rgbValues[2] = ValidateRGBValue(inputB.text);
        Color updatedColor = CreateColor(rgbValues);
        SetResultColor(resultIndex, updatedColor);
        SetSimulationColor(resultIndex, updatedColor);

        // Update the text displayed within the input fields.
        inputR.text = rgbValues[0].ToString();
        inputG.text = rgbValues[1].ToString();
        inputB.text = rgbValues[2].ToString();
    }

    void SetResultColor(int resultIndex, Color updatedColor)
    {
        Image indexResultImage = resultIndex.Equals(0) ? correctIndexResult : incorrectIndexResult;
        indexResultImage.color = updatedColor;

        // Remove for release
        string resultType = resultIndex.Equals(0) ? "Correct" : "Incorrect";
        Debug.Log($"{resultType} Index Result set to {updatedColor}");
    }

    Color CreateColor(int[] rgbValues)
    {
        float r = rgbValues[0] / 255f;
        float g = rgbValues[1] / 255f;
        float b = rgbValues[2] / 255f;

        return new Color(r, g, b, 1);
    }

    void SetSimulationColor(int indexResult, Color updatedColor)
    {
        simulationController.UpdateTileColors(indexResult, updatedColor);
    }

    public void ResetSimulationSettings()
    {
        // Reset the tile count to the default value defined in the method definition.
        tileCountInputField.text = "50";
        SetTileCount();

        // Reset the tile scale to the default values defined in the method definition.
        tileScaleXInputField.text = "1";
        tileScaleYInputField.text = "50";
        SetTileScale();

        // Reset text visible in simulation settings area and apply default colors.
        correctIndexR.text = "0";
        correctIndexG.text = "255";
        correctIndexB.text = "0";
        incorrectIndexR.text = "255";
        incorrectIndexG.text = "0";
        incorrectIndexB.text = "0";
        SetColor("0");
        SetColor("1");
    }
}
