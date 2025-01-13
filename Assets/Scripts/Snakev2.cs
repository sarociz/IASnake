using System.Collections.Generic; // Necesario para usar List
using TMPro;
using UnityEngine;

public class Snakev2 : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Food food;
    [SerializeField] private GameObject tailPrefab; // Prefab para los segmentos de la cola

    private int points = 0;
    private Vector2 direction = Vector2.right; // Dirección inicial
    private List<Transform> tail = new List<Transform>(); // Lista para los segmentos de la cola
    private float moveCooldown = 0.1f; // Tiempo entre movimientos
    private float moveTimer = 0f;

    void Start()
    {
        ResetSnake();
    }

    void Update()
    {
        HandleInput();

        // Controla el tiempo entre movimientos
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveCooldown)
        {
            MoveTail();
            Move();
            moveTimer = 0f;
        }
    }

    public void ResetSnake()
    {
        // Elimina segmentos antiguos
        foreach (Transform segment in tail)
        {
            Destroy(segment.gameObject);
        }
        tail.Clear();

        // Reinicia la dirección y posición de la cabeza
        direction = Vector2.right;
        transform.position = Vector3.zero;
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
        Vector3 newPosition = transform.position + (Vector3)direction;
        transform.position = newPosition;
    }

    void MoveTail()
    {
        // Mueve cada segmento al lugar del segmento anterior
        for (int i = tail.Count - 1; i > 0; i--)
        {
            tail[i].position = tail[i - 1].position;
        }

        // El primer segmento sigue a la cabeza
        if (tail.Count > 0)
        {
            tail[0].position = transform.position;
        }
    }

    void Grow()
    {
        // Instancia un nuevo segmento y posiciónalo en el lugar adecuado
        GameObject newSegment = Instantiate(tailPrefab);
        Vector3 newSegmentPosition = tail.Count > 0
            ? tail[tail.Count - 1].position
            : transform.position - (Vector3)direction;

        newSegment.transform.position = newSegmentPosition;
        tail.Add(newSegment.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            AddPoint();
            food.Eaten();
            Grow(); // Incrementa el tamaño de la serpiente
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("SnakeBody"))
        {
            Debug.Log("Game Over");
            Time.timeScale = 0; // Detén el juego
        }
    }

    public void AddPoint()
    {
        points++;
        scoreText.text = "Score: " + points.ToString();
    }
}
