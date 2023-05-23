using UnityEngine;

public class TileGenerator
{
    GameObject template;

    public TileGenerator(GameObject template)
    {
        this.template = template;
    }

    public GameObject[] GenerateTiles(int tileCount, float xSize, float tileOffsetY, Vector2 scale, Transform parent)
    {
        /* 
         * Time to adjust the scale and position of the tile sprites.
         * 
         * TileController Scaling
         * Q: How do we evenly scale objects from shortest to tallest?
         * A: I'm not really sure yet.
         * 
         * TileController Positioning
         * Q: How does one evenly distribute n points on a line of size L?
         * A: According to Ross Millikan's answer on Math Stack Exchange, you
         * place a point at one end of the line, and then place a new point
         * every L / (n - 1) units. The xPos below defines the leftmost point
         * on the line which is used to dislplay the tiles.
         * 
         * Link to Question: https://math.stackexchange.com/questions/36652/evenly-distribute-points-along-a-path
         */

        float yScaleOffset = scale.y / tileCount;
        float yScale = scale.y / tileCount;

        float xPosOffset = xSize / (tileCount - 1);
        float xPos = -xSize / 2f;

        float yPosOffset = (scale.y - yScale) / 2f + tileOffsetY;
        float yPos = 0f - yPosOffset;

        GameObject[] tiles = new GameObject[tileCount];
        for (int i = 0; i < tileCount; i++)
        {
            // Round the yScale value then use it to set the tiles scale.
            Vector3 tileScale = new Vector3(scale.x, yScale, 1f);

            // Determine the index postion then add it to the indexPosMap.
            Vector3 tilePos = new Vector3(xPos, yPos, 0f);

            // Create the tile object and add it to the array.
            GameObject tile = GameObject.Instantiate(template);
            SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
            tile.name = $"Tile {i}";
            tile.transform.SetPositionAndRotation(tilePos, parent.transform.rotation);
            tile.transform.parent = parent;
            tile.transform.localScale = tileScale;

            TileController controller = tile.GetComponent<TileController>();
            controller.Initialize(i, spriteRenderer);

            tiles[i] = tile;

            // Update the xPos and yScale values.
            xPos += xPosOffset;
            yScale += yScaleOffset;
            yPosOffset = (scale.y - yScale) / 2f + tileOffsetY;
            yPos = 0f - yPosOffset;
        }

        return tiles;
    }

    public TileController[] GetTileControllers(GameObject[] tiles)
    {
        TileController[] tileControllers = new TileController[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            tileControllers[i] = tiles[i].GetComponent<TileController>();
        }
        return tileControllers;
    }
}
