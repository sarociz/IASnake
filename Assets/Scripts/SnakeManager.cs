using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    public float moveInterval = 0.5f;   // Intervalo de tiempo entre movimientos
    private float moveTimer = 0f;       // Temporizador para controlar el intervalo de movimiento
    public int gridWidth = 15;          // Ancho del grid
    public int gridHeight = 15;         // Alto del grid
    private Vector2Int currentPosition; // Posición actual de la serpiente
    public GameObject snakeHead;


    private void Start()
    {
        // Inicializar la posición aleatoria de la serpiente

        SetRandomPosition();
    }

    private void Update()
    {
        // Actualizamos el temporizador para mover la serpiente
        moveTimer += Time.deltaTime;

        // Si ha pasado el intervalo de movimiento, mover la serpiente
        if (moveTimer >= moveInterval)
        {
            moveTimer = 0f; // Reiniciamos el temporizador
            MoveSnakeRandomly(); // Mover la serpiente
        }
    }

    // Función para establecer una posición inicial aleatoria dentro de los límites del grid
    private void SetRandomPosition()
    {
        int x = Random.Range(0, gridWidth);
        int y = Random.Range(0, gridHeight);
        currentPosition = new Vector2Int(x, y);
        transform.position = new Vector3(x, y, 0); // Aseguramos que Z = 0
    }

    // Función para mover la serpiente aleatoriamente dentro de los límites del grid
    private void MoveSnakeRandomly()
    {
        // Elegir una dirección aleatoria: 0 = izquierda, 1 = derecha, 2 = arriba, 3 = abajo
        int randomDirection = Random.Range(0, 4);

        // Mover la serpiente según la dirección elegida
        switch (randomDirection)
        {
            case 0: // Mover hacia la izquierda
                currentPosition.x = Mathf.Max(currentPosition.x - 1, 0);
                break;
            case 1: // Mover hacia la derecha
                currentPosition.x = Mathf.Min(currentPosition.x + 1, gridWidth - 1);
                break;
            case 2: // Mover hacia arriba
                currentPosition.y = Mathf.Min(currentPosition.y + 1, gridHeight - 1);
                break;
            case 3: // Mover hacia abajo
                currentPosition.y = Mathf.Max(currentPosition.y - 1, 0);
                break;
        }

        // Actualizar la posición en el mundo
        transform.position = new Vector3(currentPosition.x, currentPosition.y, 0); // Aseguramos que Z = 0

    }
}


