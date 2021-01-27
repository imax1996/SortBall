using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    public AudioSource collect;

    BoxCollider2D bc;
    Color colorexit;
    public int countBall;
    public int needBallToWin;
    public int numberexitpoint;
    public bool win = false;

    public GameObject star;
    public float starscale;

    public static event System.Action WinnableGame;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        needBallToWin = 5 * MapGenerate.countOfSpawn;
        colorexit = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public void ResetPoint() {
        countBall = 0;
        win = false;
        StarScale();
    }

    void Update() {

        RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, bc.size * transform.localScale.x, 0, Vector2.zero, 0, LayerMask.GetMask("Ball"));
        if (hitInfo.collider != null) {
            if (colorexit == hitInfo.collider.GetComponent<SpriteRenderer>().color)
            {
                Destroy(hitInfo.collider.gameObject);
                CountBall();
            }
            else {
                //game over
                if (GameUI.english)
                {
                    hitInfo.collider.gameObject.GetComponent<Ball>().Problem("WRANG BALL COLOR!");
                }
                else {
                    hitInfo.collider.gameObject.GetComponent<Ball>().Problem("НЕВЕРНЫЙ ЦВЕТ ШАРА!");
                }
            }
        }
    }

    void CountBall() {
        countBall++;
        StarScale();
        if (countBall >= needBallToWin && win == false) {
            win = true;
            GameUI.exitpoints[numberexitpoint] = true;
            Check();
        }
    }

    void StarScale() {
        starscale = (float)countBall / needBallToWin;

        if (starscale <= 0)
        {
            starscale = 0;
        }
        else if (starscale >= 1)
        {
            starscale = 1;
        }

        star.transform.localScale = Vector3.one * starscale;
    }

    void Check() {
        for (int i = 0; i < GameUI.exitpoints.Length; i++) {
            if (GameUI.exitpoints[i] == false) {
                collect.Play();
                return;
            }
        }
        WinnableGame?.Invoke();
    }
}
