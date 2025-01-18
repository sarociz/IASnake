using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snakev2 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Food food;
    [SerializeField] private GameObject tailPrefab;

    private int points = 0;
    private Vector2 direction = Vector2.right; // Dirección inicial
    private List<Transform> tail = new List<Transform>();
    private List<Vector3> positions = new List<Vector3>(); // Historial de posiciones
    private float moveCooldown = 0.1f;
    private float moveTimer = 0f;
    private float distance = 0.5f;
    private bool ate = false;

 
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

        // Reinicia dirección y posición
        direction = Vector2.right * distance;
        transform.position = Vector3.zero;

        // Limpia el historial de posiciones
        positions.Clear();
        positions.Add(transform.position);

        // Reinicia puntajes y texto
        points = 0;
        scoreText.text = "Score: " + points.ToString();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
        {
            direction = Vector2.up * distance;
        }
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
        {
            direction = Vector2.left * distance;
        }
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
        {
            direction = Vector2.down * distance;
        }
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
        {
            direction = Vector2.right * distance;
        }
    }

    void Move()
    {
        Vector2 v = transform.position;

        transform.Translate(direction);

        if (ate)
        {
            GameObject g = Instantiate(tailPrefab, v, Quaternion.identity);
            tail.Insert(0, g.transform);
            ate = false;
        }
        else if (tail.Count > 0)
        {
            Vector3 previousPosition = tail[0].position;
            tail[0].position = v;

            for (int i = 1; i < tail.Count; i++)
            {
                Vector3 tempPosition = tail[i].position;
                tail[i].position = previousPosition;
                previousPosition = tempPosition;
            }
        }

        // Actualiza historial de posiciones
        positions.Add(transform.position);
    }


    void Grow()
    {
        // Instancia un nuevo segmento y posiciónalo
        GameObject newSegment = Instantiate(tailPrefab);
        Vector3 newSegmentPosition = tail.Count > 0
            ? tail[tail.Count - 1].position
            : transform.position - (Vector3)direction;

        newSegment.transform.position = newSegmentPosition;
        tail.Add(newSegment.transform);

        // Activa la inmunidad al cuerpo
       // immuneToBodyCollision = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            AddPoint();
            food.Eaten();
            //Grow();
            ate = true;
            Destroy(collision.gameObject);
        }
        else {
            Debug.Log("Game Over");
            SceneManager.LoadScene("GameOver");

        }
    }


    public void AddPoint()
    {
        points++;
        scoreText.text = "Score: " + points.ToString();
    }
}
