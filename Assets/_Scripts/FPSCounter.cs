using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Yoinked from user Stardog's answer to the FPS Counter question located at
 * https://forum.unity.com/threads/fps-counter.505495/
 * 
 * Output strings are modified to fit the format I wanted for this project.
 */

public class FPSCounter : MonoBehaviour
{
    TMP_Text fpsTextLabel;
    string fpsBaseText = "FPS =";
    
    Dictionary<int, string> cachedNumberStrings = new Dictionary<int, string>();
    int[] _frameRateSamples;
    int _cacheNumbersAmount = 300;
    int _averageFromAmount = 30;
    int _averageCounter = 0;
    int _currentAveraged;

    private void Awake()
    {
        fpsTextLabel = GetComponent<TMP_Text>();

        // Cache strings and create array.
        for (int i = 0; i < _cacheNumbersAmount; i++)
        {
            cachedNumberStrings[i] = i.ToString();
        }
        _frameRateSamples = new int[_averageFromAmount];
    }

    // Update is called once per frame
    void Update()
    {
        // Sample.
        var currentFrame = (int)System.Math.Round(1f / Time.smoothDeltaTime); // If your game modifies Time.timeScal, use unscaledDeltaTime and smooth manually (or not).
        _frameRateSamples[_averageCounter] = currentFrame;

        // Average.
        var average = 0f;
        foreach (var frameRate in _frameRateSamples)
        {
            average += frameRate;
        }

        _currentAveraged = (int)System.Math.Round(average / _averageFromAmount);
        _averageCounter = (_averageCounter + 1) % _averageFromAmount;

        // Assign to UI.
        fpsTextLabel.text = _currentAveraged < _cacheNumbersAmount && _currentAveraged > 0
            ? $"{fpsBaseText} {cachedNumberStrings[_currentAveraged]}"
            : _currentAveraged < 0
                ? $"{fpsBaseText} 0"
                : _currentAveraged > _cacheNumbersAmount
                    ? $"{fpsBaseText} {_cacheNumbersAmount}"
                    : $"{fpsBaseText} -1";
    }
}
