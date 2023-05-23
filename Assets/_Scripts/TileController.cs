using UnityEngine;

public class TileController : MonoBehaviour
{
    public int currentIndex; // Where the tile currently is.
    public int desiredIndex; // Where the tile should be.
    [HideInInspector]
    public bool atDesiredIndex;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    public void Initialize(int index, SpriteRenderer spriteRenderer)
    {
        currentIndex = index;
        desiredIndex = index;
        atDesiredIndex = currentIndex == desiredIndex;

        this.spriteRenderer = spriteRenderer;
    }

    public void UpdateTileData(int newIndex, Vector3 newPosition)
    {
        // Update the tiles position.
        Vector3 updatedPosition = new Vector3(newPosition.x, transform.position.y, transform.position.z);
        transform.SetPositionAndRotation(updatedPosition, transform.rotation);

        // Update currentIndex, then color based on the tiles current index.
        currentIndex = newIndex;
        atDesiredIndex = currentIndex == desiredIndex;
    }
}
