using UnityEngine;

public class Food : MonoBehaviour
{
    public GameObject food;

    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject topWall;
    public GameObject bottomWall;

    private float minposx, maxposx, minposy, maxposy;
    // Start is called before the first frame update
    void Start()
    {
        float foodX = food.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float foodY = food.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        minposx = leftWall.transform.position.x + leftWall.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2 + foodX / 2;
        maxposx = rightWall.transform.position.x - rightWall.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2 - foodX / 2;
        minposy = bottomWall.transform.position.y + bottomWall.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2 + foodY / 2;
        maxposy = topWall.transform.position.y - topWall.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2 - foodY / 2;

        //InvokeRepeating("Spawn", 2f, 1f);
        food.SetActive(true);
        Invoke(nameof(Spawn), 0f);
    }

    void Spawn()
    {
        float x = Random.Range(minposx, maxposx);
        float y = Random.Range(minposy, maxposy);
        Vector2 pos = new Vector2(x, y);
        Instantiate(food, pos, Quaternion.identity, transform);
    }


    public void Eaten()
    {
        Invoke(nameof(Spawn), 0f);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
