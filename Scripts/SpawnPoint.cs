using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject ball;
    public GameObject arrow;
    public Color[] colors;
    public Vector2 target;
    public int dirX, dirY;

    public float startTime;
    float timer;
    public float spawnTime;
    public static bool active;

    public GameObject ballPool;

    void Start()
    {
        active = false;
        target = transform.position;
        timer = startTime;
    }

    void Update()
    {
        if (active) {
            timer -= Time.deltaTime;
        }

        if (timer <= 0) {
            Game();
            timer = spawnTime;
        }
    }

    void Game() {
        GameObject newBall = Instantiate(ball, transform.position, Quaternion.identity, ballPool.transform) as GameObject;
        int numberOfColor = Random.Range(0, colors.Length);
        newBall.GetComponent<SpriteRenderer>().color = colors[numberOfColor];
        newBall.GetComponent<Ball>().numberOfColor = numberOfColor;
        newBall.GetComponent<Ball>().targetPosition = new Vector2(target.x + 1 * dirX, target.y + 1 * dirY);
    }
}