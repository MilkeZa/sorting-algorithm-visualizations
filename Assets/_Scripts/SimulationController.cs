using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    GameController gameController;

    // Gameobjects required by this or other classes.
    public GameObject tileTemplate;
    public TMP_Text swapCountLabel;
    public TMP_Text percentSortedLabel;
    public GameObject tileContainer;

    // How wide in world space the simulationArea will display.
    float xSize = 105f;

    // Alter the tiles position on the Y axis.
    public float tileOffsetY = 5f;

    // Number of tiles in the simulationArea.
    public int tileCount = 50;
    public Vector2 tileScale = new Vector2(1f, 50f);

    // Colors that the tile sprites will be using.
    public Color correctIndexColor;
    public Color incorrectIndexColor;

    [HideInInspector]
    public GameObject[] tiles;
    TileController[] tileControllers;

    TileGenerator tileGenerator;
    
    TileDisplay tileDisplay;

    TileSorter tileSorter;
    public bool randomSeed = true;
    public string seed;

    public enum SortingAlgorithm { Bubble, Shaker };
    public SortingAlgorithm sortingAlgorithm;

    private void Awake()
    {
        gameController = GetComponent<GameController>();
        tileContainer = gameController.tileContainer;
        Initialize();
    }

    public List<string> GetSortingAlgorithms()
    {
        List<string> sortingAlgorithms = new List<string>();
        foreach (string sortingAlgorithmName in Enum.GetNames(typeof(SortingAlgorithm)))
        {
            sortingAlgorithms.Add(sortingAlgorithmName);
        }
        return sortingAlgorithms;
    }

    public void SetSortingAlgorithm(int algorithmIndex)
    {
        sortingAlgorithm = (SortingAlgorithm)algorithmIndex;
    }

    public void Initialize()
    {
        if (transform.childCount > 0)
        {
            DestroyChildren();
        }

        tileGenerator = new TileGenerator(tileTemplate);
        tiles = tileGenerator.GenerateTiles(tileCount, xSize, tileOffsetY, tileScale, tileContainer.transform);
        tileControllers = tileGenerator.GetTileControllers(tiles);

        tileDisplay = new TileDisplay(tileControllers, correctIndexColor, incorrectIndexColor, swapCountLabel, percentSortedLabel);
        
        if (randomSeed)
        {
            seed = Time.time.ToString();
        }
        tileSorter = new TileSorter(tileControllers, tileDisplay, seed.GetHashCode());
    }

    public void ScrambleTiles()
    {
        if (transform.childCount >= 1 && tileSorter != null)
        {
            /*
             * Determine how many iterations should be used based on how many child objects there are.
             * More children means more iterations.
             */

            int iterations = DetermineScrambleIterations();
            tileSorter.ScrambleTiles(iterations);
        }
    }

    int DetermineScrambleIterations()
    {
        int baseIterations = 1;
        if (tileCount < 50)
        {
            return baseIterations;
        }
        else if (50 <= tileCount && tileCount < 100) 
        {
            return baseIterations * 2;
        }
        else if (100 <= tileCount && tileCount < 1000)
        {
            return baseIterations * 3;
        }
        else
        {
            return baseIterations * 4;
        }
    }

    public void Sort(bool singleStep = false)
    {
        if (tileContainer.transform.childCount >= 1)
        {
            switch (sortingAlgorithm)
            {
                case SortingAlgorithm.Bubble:
                    tileSorter.BubbleSort(singleStep);
                    break;
                case SortingAlgorithm.Shaker:
                    tileSorter.ShakerSort(singleStep);
                    break;
            }
        }
    }

    public void UpdateTileColors(int resultIndex, Color updatedColor)
    {
        if (resultIndex.Equals(0))
        {
            correctIndexColor = updatedColor;
            tileDisplay.correctIndexColor = updatedColor;
        }
        else
        {
            incorrectIndexColor = updatedColor;
            tileDisplay.incorrectIndexColor = updatedColor;
        }
        tileDisplay.UpdateTiles(tileControllers);
    }

    public void DestroyChildren()
    {
        if (tileContainer.transform.childCount >= 1)
        {
            for (int i = tileContainer.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(tileContainer.transform.GetChild(i).gameObject);
            }
        }
    }
}
