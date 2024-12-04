using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public GameObject snakeHead; // Cambiado a GameObject
    public GameObject bodySegmentPrefab; // Prefab del cuerpo
    public float moveSpeed = 0.5f; // Velocidad de movimiento

    private List<Transform> bodySegments = new List<Transform>();
    private Vector2Int direction = Vector2Int.right; // Dirección inicial
    private bool isAlive = true;
    public int gridWidth = 15; // Ancho del grid
    public int gridHeight = 15; // Altura del grid
    public float cellSize = 0.5f; // Tamaño de cada celda

    //void Start()
    //{
    //    ResetSnake();
    //}



    //public void ResetSnake()
    //{
    //    // Elimina segmentos antiguos
    //    foreach (Transform segment in bodySegments)
    //    {
    //        Destroy(segment.gameObject);
    //    }
    //    bodySegments.Clear();

    //    direction = Vector2Int.right;

    //    // Coloca la serpiente en una posición aleatoria dentro del grid
    //    Vector2Int randomPosition = GetRandomGridPosition();
    //    Vector3 newHeadPosition = GridToWorldPosition(randomPosition);
    //    snakeHead.transform.position = newHeadPosition;

    //    // Añade la cabeza a la lista de segmentos
    //    bodySegments.Add(snakeHead.transform);
    //}

    //public void Move()
    //{
    //    if (!isAlive) return; // Si no está vivo, no se mueve

    //    // Mover el cuerpo
    //    for (int i = bodySegments.Count - 1; i > 0; i--)
    //    {
    //        bodySegments[i].position = bodySegments[i - 1].position;
    //    }

    //    // Mover la cabeza con velocidad (multiplicado por Time.deltaTime)
    //    Vector3 headPosition = snakeHead.transform.position + new Vector3(direction.x * cellSize, direction.y * cellSize, 0);
    //    snakeHead.transform.position = Vector3.MoveTowards(snakeHead.transform.position, headPosition, moveSpeed * Time.deltaTime);
    //}

    //private Vector2Int GetRandomGridPosition()
    //{
    //    int x = Random.Range(0, gridWidth);
    //    int y = Random.Range(0, gridHeight);
    //    return new Vector2Int(x, y);
    //}

    //private Vector3 GridToWorldPosition(Vector2Int gridPosition)
    //{
    //    // Convertir la posición del grid (en términos de celdas) a coordenadas del mundo
    //    //float xPos = (gridPosition.x - gridWidth / 2) * cellSize;
    //    //float yPos = (gridPosition.y - gridHeight / 2) * cellSize;

    //    float xPos = (gridPosition.x - gridWidth / 2f) * cellSize + cellSize / 2f;
    //    float yPos = (gridPosition.y - gridHeight / 2f) * cellSize + cellSize / 2f;
    //    return new Vector3(xPos, yPos, 0);
    //}

    //public void ChangeDirection(Vector2Int newDirection)
    //{
    //    // Asegúrate de que la dirección no sea opuesta a la actual
    //    if (newDirection + direction != Vector2Int.zero)
    //    {
    //        direction = newDirection;
    //    }
    //}

    //public void Grow()
    //{
    //    // Crear un nuevo segmento del cuerpo
    //    Transform newSegment = Instantiate(bodySegmentPrefab, bodySegments[bodySegments.Count - 1].position, Quaternion.identity).transform;
    //    bodySegments.Add(newSegment);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Food"))
    //    {
    //        Grow();
    //    }
    //    else if (other.CompareTag("Wall") || other.CompareTag("Body"))
    //    {
    //        Die();
    //    }
    //}

    //public void Die()
    //{
    //    isAlive = false;
    //    Debug.Log("Game Over");
    //}
}
