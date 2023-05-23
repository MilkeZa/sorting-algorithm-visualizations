using UnityEngine;

public class TileSorter
{
    System.Random prng;
    TileController[] tileControllers;

    public int swapCount;
    public double percentSorted;
    int sorted;
    int total;

    TileDisplay tileDisplay;

    public TileSorter(TileController[] tileControllers, TileDisplay tileDisplay, int prngSeed)
    {
        this.tileControllers = tileControllers;

        prng = new System.Random(prngSeed);

        this.tileDisplay = tileDisplay;
        ResetStatistics();
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        tileDisplay.UpdateText(swapCount, percentSorted, sorted, total);
        tileDisplay.UpdateTiles(tileControllers);
    }

    void UpdateStatistics()
    {
        swapCount++;

        int atDesiredIndexSum = 0;
        foreach (TileController controller in tileControllers)
        {
            if (controller.atDesiredIndex)
            {
                atDesiredIndexSum++;
            }
        }
        percentSorted = (atDesiredIndexSum / (double)tileControllers.Length) * 100;
        percentSorted = System.Math.Round(percentSorted, 2);
        sorted = atDesiredIndexSum;
        total = tileControllers.Length;
        tileDisplay.UpdateText(swapCount, percentSorted, sorted, total);

        // Don't bother uncommenting this debug statement with simulations that have more than 10 tiles.
        //Debug.Log($"Swap Count = {swapCount}, {percentSorted}% ({atDesiredIndexSum} / {tileControllers.Length})");
    }

    void ResetStatistics()
    {
        swapCount = -1;
        percentSorted = 0;
        sorted = 0;
        total = 0;
        UpdateStatistics();
    }

    public void ScrambleTiles(int swapIterations)
    {
        for (int iterations = 0; iterations < swapIterations; iterations++)
        {
            for (int i = 0; i < tileControllers.Length; i++)
            {
                // Choose two random indicies to swap elements at.
                int indexA = prng.Next(tileControllers.Length);
                int indexB = prng.Next(tileControllers.Length);

                // Verify that the same index is never swapped with itself.
                while (indexA == indexB)
                {
                    indexB = prng.Next(tileControllers.Length - 1);
                }

                // Swap the tiles.
                SwapTilesByIndex(indexA, indexB);
            }
        }

        ResetStatistics();
        UpdateDisplay();
    }

    void SwapTilesByIndex(int indexA, int indexB)
    {
        // Fetch the tile controllers and required information.
        TileController controllerA = tileControllers[indexA];
        TileController controllerB = tileControllers[indexB];

        tileControllers[indexA] = controllerB;
        tileControllers[indexB] = controllerA;

        Vector3 posA = controllerA.gameObject.transform.position;
        Vector3 posB = controllerB.gameObject.transform.position;

        controllerA.UpdateTileData(indexB, posB);
        controllerB.UpdateTileData(indexA, posA);

        UpdateStatistics();
    }

    public void BubbleSort(bool step)
    {
        if (step)
        {
            // Perform just the next swap of the simulationArea.
            for (int i = 0; i < tileControllers.Length; i++)
            {
                for (int j = 0; j < tileControllers.Length - i - 1; j++)
                {
                    int desiredIndexA = tileControllers[j].desiredIndex;
                    int desiredIndexB = tileControllers[j + 1].desiredIndex;

                    if (desiredIndexA > desiredIndexB)
                    {
                        SwapTilesByIndex(j, j + 1);

                        // Swap has occurred, update display and exit.
                        UpdateDisplay();
                        return;
                    }
                }
            }
        }
        else
        {
            // Set the swap count to 0 and perform the entire simulation from there.
            ResetStatistics();
            for (int i = 0; i < tileControllers.Length; i++)
            {
                bool swapFlag = false;

                for (int j = 0; j < tileControllers.Length - i - 1; j++)
                {
                    int desiredIndexA = tileControllers[j].desiredIndex;
                    int desiredIndexB = tileControllers[j + 1].desiredIndex;

                    if (desiredIndexA > desiredIndexB)
                    {
                        SwapTilesByIndex(j, j + 1);
                        swapFlag = true;
                    }
                }

                if (!swapFlag)
                {
                    break;
                }
            }
            UpdateDisplay();
        }
    }

    public void ShakerSort(bool step)
    {
        if (step)
        {
            // Perform just the next swap of the simulation.
        }
        else
        {
            // Set the swap count to 0 and perform the entire simulation from there.
            ResetStatistics();

            int numElements = tileControllers.Length;
            bool swapped = true;
            int start = 0;
            int end = numElements - 1;

            while (swapped)
            {
                swapped = false;

                for (int i = start; i < end; i++)
                {
                    if (tileControllers[i].desiredIndex > tileControllers[i + 1].desiredIndex)
                    {
                        SwapTilesByIndex(i, i + 1);
                        swapped = true;
                    }
                }

                if (!swapped)
                {
                    break;
                }

                swapped = false;

                for (int i = end - 1; i >= start; i--)
                {
                    if (tileControllers[i].desiredIndex > tileControllers[i + 1].desiredIndex)
                    {
                        SwapTilesByIndex(i, i + 1);
                        swapped = true;
                    }
                }

                start = start + 1;
            }
            UpdateDisplay();
        }
    }
}
