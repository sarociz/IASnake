using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    public Transform snakeHead; // Referencia a la cabeza
    public GameObject bodySegmentPrefab; // Prefab del cuerpo
    public float moveSpeed = 1.0f; // Velocidad de movimiento

    private List<Transform> bodySegments = new List<Transform>();
    private Vector2Int direction = Vector2Int.right; // Dirección inicial
    private bool isAlive = true;

    void Start()
    {
        bodySegments.Add(snakeHead); // Añadir la cabeza a la lista
        InvokeRepeating("Move", 0, 1 / moveSpeed); // Llamar a la función Move cada 1/moveSpeed segundos
    }

    public void Move()
    {
        if (!isAlive) return; // Si no está vivo, no se mueve

        // Mover el cuerpo
        for (int i = bodySegments.Count - 1; i > 0; i--)
        {
            bodySegments[i].position = bodySegments[i - 1].position;
        }

        // Mover la cabeza
        Vector2Int headPosition = Vector2Int.RoundToInt(snakeHead.position);
        //snakeHead.position = headPosition + direction;

        // Comprobar si ha chocado
        if (bodySegments.Exists(x => Vector2Int.RoundToInt(x.position) == headPosition))
        {
            isAlive = false;
            CancelInvoke("Move");
        }
    }

    public void addBody()
    {
        Vector3 newBodyPosition = bodySegments[bodySegments.Count - 1].position;
        GameObject newBodySegment = Instantiate(bodySegmentPrefab, newBodyPosition, Quaternion.identity);
        bodySegments.Add(newBodySegment.transform);

        // bodySegments.Add(Instantiate(bodySegmentPrefab, bodySegments[bodySegments.Count - 1].position, Quaternion.identity).transform);

    }
}
