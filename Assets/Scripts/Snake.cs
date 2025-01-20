using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class Snake : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] public Food food;
    [SerializeField] private GameObject tailPrefab;
    [SerializeField] private Transform spriteTransform;
    [SerializeField] private List<Transform> bodySprites = new List<Transform>();

    private NeuralNetwork NeuralNetwork;
    private int points = 0;
    private Vector2 direction = Vector2.right; // Dirección inicial
    private List<Transform> tail = new List<Transform>();

    private List<Vector3> positions = new List<Vector3>(); // Historial de posiciones
    public float moveCooldown = 0.05f; // Reduce la velocidad al aumentar este valor
    private bool ate = false;
    private bool immuneToBodyCollision = false;
    public float segmentDistance = 0.7f; // Distancia entre segmentos

    void Start()
    {
        NeuralNetwork = new NeuralNetwork(inputSize: 8, hiddenSize: 12, outputSize: 4);
        ResetSnake();
        InvokeRepeating(nameof(Move), moveCooldown, moveCooldown);
    }

    void Update()
    {
        // Usa la red neuronal para decidir la dirección
        float[] state = GetState();
        int action = NeuralNetwork.Predict(state); // Predice la acción
        UpdateDirection(action); // Actualiza la dirección basada en la predicción
        //HandleInput();
    }

    public void ResetSnake()
    {
        // Elimina segmentos antiguos
        foreach (Transform segment in tail)
        {
            Destroy(segment.gameObject);
        }
        tail.Clear();

        // Reinicia dirección y posición
        direction = Vector2.right;
        transform.position = Vector3.zero;

        // Limpia el historial de posiciones
        positions.Clear();
        positions.Add(transform.position);

        // Reinicia puntajes y texto
        points = 0;
        scoreText.text = "Score: " + points.ToString();

        // Reinicia la inmunidad
        immuneToBodyCollision = false;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
        {
            direction = Vector2.right;
        }
    }

    void Move()
    {
        Vector2 currentPosition = transform.position;
        float[] currentState = GetState();

        // Mueve la cabeza
        transform.Translate(direction * segmentDistance); // Usa `segmentDistance` para mover más suavemente

        FlipSprite();
        // Si comió, añade un nuevo segmento
        if (ate)
        {
            GameObject newSegment = Instantiate(tailPrefab, currentPosition, Quaternion.identity);
            tail.Insert(0, newSegment.transform);

            // Agregar sprite hijo para el nuevo segmento
            Transform bodySprite = newSegment.transform.GetChild(0);
            bodySprites.Insert(0, bodySprite);

            ate = false;
            //NeuralNetwork.ApplyReward(10f, currentState, -1, GetState());

            // Activa inmunidad temporal para evitar colisiones
            immuneToBodyCollision = true;
            Invoke(nameof(DisableImmunity), moveCooldown);
        }
        else if (tail.Count > 0)
        {
            //penalización por cada paso sin comida
            //NeuralNetwork.ApplyReward(-1f, currentState, -1, GetState());

            // Actualiza las posiciones del cuerpo
            tail.Last().position = currentPosition;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
        UpdateBodySpritesRotation();
    }
    void FlipSprite()
    {
        // Calcula el ángulo solo para la rotación visual del sprite
        float angle = 0f;

        if (direction == Vector2.up)
        {
            angle = 90f; // Visual hacia arriba
            spriteTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (direction == Vector2.down)
        {
            angle = -90f; // Visual hacia abajo
            spriteTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (direction == Vector2.left)
        {
            angle = 180f; // Visual hacia la izquierda
            spriteTransform.localScale = new Vector3(1f, -1f, 1f);

        }
        else if (direction == Vector2.right)
        {
            angle = 0f; // Visual hacia la derecha
            spriteTransform.localScale = new Vector3(1f, 1f, 1f);

        }
        // Aplica la rotación solo al hijo (spriteTransform)
        spriteTransform.rotation = Quaternion.Euler(0f, 0f, angle);

    }

    void UpdateBodySpritesRotation()
    {
        // Recorre todos los segmentos del cuerpo (excepto la cabeza)
        for (int i = 0; i < bodySprites.Count; i++)
        {
            Transform currentSegment = bodySprites[i];
            Vector3 directionToNext;

            if (i == bodySprites.Count - 1)
            {
                // Último segmento: orientarse hacia la cabeza
                directionToNext = transform.position - currentSegment.position;
            }
            else
            {
                // Segmento intermedio: orientarse hacia el siguiente segmento
                directionToNext = bodySprites[i + 1].position - currentSegment.position;
            }

            // Determina el ángulo para orientar el sprite
            float angle = Mathf.Atan2(directionToNext.y, directionToNext.x) * Mathf.Rad2Deg;

            // Aplica la rotación solo al sprite (hijo)
            currentSegment.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }


    void UpdateDirection(int action)
    {
        switch (action)
        {
            case 0: if (direction != Vector2.down) direction = Vector2.up; break;
            case 1: if (direction != Vector2.up) direction = Vector2.down; break;
            case 2: if (direction != Vector2.right) direction = Vector2.left; break;
            case 3: if (direction != Vector2.left) direction = Vector2.right; break;
        }
    }

    float[] GetState()
    {
        Vector2 foodPosition = food.transform.position;
        Vector2 snakePosition = transform.position;

        float distanceToFoodX = foodPosition.x - snakePosition.x;
        float distanceToFoodY = foodPosition.y - snakePosition.y;

        float dangerLeft = Physics2D.Raycast(snakePosition, Vector2.left, 1f) ? 1f : 0f;
        float dangerRight = Physics2D.Raycast(snakePosition, Vector2.right, 1f) ? 1f : 0f;
        float dangerUp = Physics2D.Raycast(snakePosition, Vector2.up, 1f) ? 1f : 0f;
        float dangerDown = Physics2D.Raycast(snakePosition, Vector2.down, 1f) ? 1f : 0f;

        return new float[]
        {
            distanceToFoodX, distanceToFoodY,
            direction.x, direction.y,
            dangerLeft, dangerRight, dangerUp, dangerDown
        };
    }


    void DisableImmunity()
    {
        immuneToBodyCollision = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            AddPoint();
            food.Eaten();
            ate = true;
            Destroy(collision.gameObject);
        }
        else if ((!immuneToBodyCollision && collision.CompareTag("SnakeBody")) || collision.gameObject.CompareTag("Wall"))
        {
            GameOver();
        }
    }


    void GameOver()
    {
        Debug.Log("Game Over");
        //NeuralNetwork.ApplyReward(-100f, GetState(), -1, null); // Penalización alta por perder
        ResetSnake();
    }

    public void AddPoint()
    {
        points++;
        scoreText.text = "Score: " + points.ToString();
        //NeuralNetwork.ApplyReward(10f, GetState(), -1, null); // Recompensa al comer comida
    }
}
