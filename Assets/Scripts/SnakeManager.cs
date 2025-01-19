using UnityEngine;

using System.Collections.Generic;

public class SnakeManager : MonoBehaviour
{
    public float moveInterval = 0.5f;   // Intervalo de tiempo entre movimientos
    private float moveTimer = 0f;       // Temporizador para controlar el intervalo de movimiento
    public int gridWidth = 15;          // Ancho del grid
    public int gridHeight = 15;         // Alto del grid
    private Vector2Int currentPosition; // Posición actual de la serpiente
    public GameObject snakeHead;

    [SerializeField] private Snake snake; // Referencia al script principal de la serpiente
    private NeuralNetwork network;

    private void Start()
    {

        // Inicializa la red neuronal
        network = new NeuralNetwork(inputSize: 6, hiddenSize: 12, outputSize: 4); // 6 entradas, 4 salidas
        SetRandomPosition();
    }

    private void Update()
    {
        // Obtener el estado del juego
        //float[] state = GetState();

        //// Obtener la acción predicha por la red neuronal
        //int action = network.Predict(state);

        //// Realizar la acción
        //PerformAction(action);
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


    //private float[] GetState()
    //{
    //    // Define el estado del juego
    //    Vector2 foodPosition = snake.food.transform.position;
    //    Vector2 snakePosition = snake.transform.position;
    //    Vector2 direction = snake.direction;

    //    // Calcula distancias relativas
    //    float distanceToFoodX = foodPosition.x - snakePosition.x;
    //    float distanceToFoodY = foodPosition.y - snakePosition.y;

    //    // Detectar colisiones (simplificado)
    //    float dangerLeft = Physics2D.Raycast(snakePosition, Vector2.left, 1f) ? 1f : 0f;
    //    float dangerRight = Physics2D.Raycast(snakePosition, Vector2.right, 1f) ? 1f : 0f;
    //    float dangerUp = Physics2D.Raycast(snakePosition, Vector2.up, 1f) ? 1f : 0f;
    //    float dangerDown = Physics2D.Raycast(snakePosition, Vector2.down, 1f) ? 1f : 0f;

    //    return new float[]
    //    {
    //        distanceToFoodX, distanceToFoodY,
    //        direction.x, direction.y,
    //        dangerLeft, dangerRight, dangerUp, dangerDown
    //    };
    //}

    //private void PerformAction(int action)
    //{
    //    // Traduce la acción a un movimiento
    //    switch (action)
    //    {
    //        case 0: snake.SetDirection(Vector2.up); break;
    //        case 1: snake.SetDirection(Vector2.down); break;
    //        case 2: snake.SetDirection(Vector2.left); break;
    //        case 3: snake.SetDirection(Vector2.right); break;
    //    }
    //}
}



