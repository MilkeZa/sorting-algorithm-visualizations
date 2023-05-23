using UnityEngine;
using TMPro;

public class TileDisplay
{
    // Colors that the tile sprites will be using.
    [HideInInspector]
    public Color correctIndexColor;
    [HideInInspector]
    public Color incorrectIndexColor;

    TileController[] tileControllers;

    TMP_Text swapCountLabel;
    string swapCountBaseText = "Swaps";

    TMP_Text percentSortedLabel;
    string percentSortedBaseText = "% Sorted";

    public TileDisplay(TileController[] tileControllers, Color correctIndexColor, Color incorrectIndexColor, TMP_Text swapCountLabel, TMP_Text percentSortedLabel)
    {
        // Set the correct/incorrect index colors.
        this.correctIndexColor = correctIndexColor;
        this.incorrectIndexColor = incorrectIndexColor;

        // Set the tile controllers and update their colors.
        this.tileControllers = tileControllers;
        UpdateTiles(this.tileControllers);

        // Set the text labels and update their text.
        this.swapCountLabel = swapCountLabel;
        this.percentSortedLabel = percentSortedLabel;

        UpdateText();
    }

    public void UpdateTiles(TileController[] tileControllers)
    {
        // Iterate through each tile controller.
        this.tileControllers = tileControllers;
        foreach (TileController controller in tileControllers)
        {
            if (controller.atDesiredIndex)
            {
                // Sprite is at desired index set correct index color.
                controller.spriteRenderer.color = correctIndexColor;
            }
            else
            {
                // Sprite is not at desired index, set incorrect index color.
                controller.spriteRenderer.color = incorrectIndexColor;
            }
        }
    }

    public void UpdateText(int swapCount = 0, double percentSorted = 0, int sorted = 0, int total = 0)
    {
        // Format the text with the swap count and percent sorted values.
        string swapCountText = $"{swapCount} {swapCountBaseText}";
        string percentSortedText = $"{percentSorted}{percentSortedBaseText} ({sorted} / {total})";

        // Set the text within the labels with the formatted strings.
        swapCountLabel.SetText(swapCountText);
        percentSortedLabel.SetText(percentSortedText);
    }
}
