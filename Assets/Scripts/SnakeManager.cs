using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    public float moveInterval = 0.5f;   // Intervalo de tiempo entre movimientos
    private float moveTimer = 0f;       // Temporizador para controlar el intervalo de movimiento
    public int gridWidth = 15;          // Ancho del grid
    public int gridHeight = 15;         // Alto del grid
    private Vector2Int currentPosition; // Posici�n actual de la serpiente
    public GameObject snakeHead;


    private void Start()
    {
        // Inicializar la posici�n aleatoria de la serpiente

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

    // Funci�n para establecer una posici�n inicial aleatoria dentro de los l�mites del grid
    private void SetRandomPosition()
    {
        int x = Random.Range(0, gridWidth);
        int y = Random.Range(0, gridHeight);
        currentPosition = new Vector2Int(x, y);
        transform.position = new Vector3(x, y, 0); // Aseguramos que Z = 0
    }

    // Funci�n para mover la serpiente aleatoriamente dentro de los l�mites del grid
    private void MoveSnakeRandomly()
    {
        // Elegir una direcci�n aleatoria: 0 = izquierda, 1 = derecha, 2 = arriba, 3 = abajo
        int randomDirection = Random.Range(0, 4);

        // Mover la serpiente seg�n la direcci�n elegida
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

        // Actualizar la posici�n en el mundo
        transform.position = new Vector3(currentPosition.x, currentPosition.y, 0); // Aseguramos que Z = 0

    }
}


