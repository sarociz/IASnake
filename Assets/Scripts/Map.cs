using UnityEngine;

public class Map : MonoBehaviour
{

    public GameObject cellPrefab;
    public int width = 20;
    public int height = 20;
    public float cellSize = 1f;

    private GameObject[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        // Inicializar la rejilla
        grid = new GameObject[width, height];

        // Iterar por cada posición en la rejilla
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Crear una nueva celda
                Vector3 startPosition = new Vector3(-(width / 2) * cellSize, -(height / 2) * cellSize, 0);
                Vector3 cellPosition = startPosition + new Vector3(x * cellSize, y * cellSize, 0);
                GameObject newCell = Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);

                // Nombrar y organizar la celda
                newCell.name = $"Cell ({x}, {y})";

                // Añadir la celda a la rejilla
                grid[x, y] = newCell;

            }
        }
    }

}
