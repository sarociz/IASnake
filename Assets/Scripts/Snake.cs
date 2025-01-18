using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Food food;
    [SerializeField] private GameObject tailPrefab;

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
        ResetSnake();
        InvokeRepeating(nameof(Move), moveCooldown, moveCooldown);
    }
    void Update()
    {
        HandleInput();
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

        // Mueve la cabeza
        transform.Translate(direction * segmentDistance); // Usa `segmentDistance` para mover más suavemente


        // Si comió, añade un nuevo segmento
        if (ate)
        {
            GameObject newSegment = Instantiate(tailPrefab, currentPosition, Quaternion.identity);
            tail.Insert(0, newSegment.transform);
            ate = false;

            // Activa inmunidad temporal para evitar colisiones
            immuneToBodyCollision = true;
            Invoke(nameof(DisableImmunity), moveCooldown);
        }
        else if (tail.Count > 0)
        {
            // Actualiza las posiciones del cuerpo
            tail.Last().position = currentPosition;

            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
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
        else if (!immuneToBodyCollision && collision.CompareTag("SnakeBody"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("GameOver");
    }

    public void AddPoint()
    {
        points++;
        scoreText.text = "Score: " + points.ToString();
    }
}
