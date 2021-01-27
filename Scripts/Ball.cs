using UnityEngine;

public class Ball : MonoBehaviour
{
    CircleCollider2D cl;
    LayerMask routeMask;
    float velocity;

    public GameObject problem;

    public Vector2 lastPosition;
    public Vector2 currentPosition;
    public Vector2 targetPosition;

    [HideInInspector]
    public float moveSpeed = 1.5f;
    float curCpeed;

    public int numberOfColor;

    public static bool createproblem = false;

    void Start()
    {
        cl = GetComponent<CircleCollider2D>();
        lastPosition = transform.position;
        currentPosition = transform.position;
        curCpeed = moveSpeed;
    }

    void TextProblem(string text) {
        if (GameUI.english)
        {
            switch (text) {
                case "НЕТ ДОРОГИ!":
                    Problem("NO ROAD!");
                    break;
                case "ШАРЫ СТОЛКНУЛИСЬ!":
                    Problem("BALLS CRASHED!");
                    break;
                case "НЕВЕРНЫЙ ВХОД ИЛИ ВЫХОД!":
                    Problem("WRONG ENTRANCE OR EXIT!");
                    break;
            }
        }
        else {
            Problem(text);
        }
    }

    void Update()
    {
        if (SpawnPoint.active == false) {
            Destroy(gameObject);
            Time.timeScale = 1;
        }

        if (Time.timeScale == 1)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.zero, 0, LayerMask.GetMask("BlockPoint"));
            if (hitInfo.collider != null)
            {
                // game over
                TextProblem("НЕТ ДОРОГИ!");
            }

            RaycastHit2D ballHit = Physics2D.Raycast(transform.position, Vector2.zero, 0, LayerMask.GetMask("Ball"));
            if (ballHit.collider != null)
            {
                // game over
                if ((targetPosition - currentPosition) == -(ballHit.collider.GetComponent<Ball>().targetPosition - ballHit.collider.GetComponent<Ball>().currentPosition))
                {
                    if (targetPosition != ballHit.collider.GetComponent<Ball>().targetPosition)
                    {
                        if (currentPosition != ballHit.collider.GetComponent<Ball>().currentPosition)
                        {
                            TextProblem("ШАРЫ СТОЛКНУЛИСЬ!");
                        }
                    }
                }
            }
        }
        if ((Vector2)transform.position == targetPosition) {
            lastPosition = currentPosition;
            currentPosition = targetPosition;
            TakeTarget();
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, curCpeed * Time.deltaTime);
    }

    public void Problem(string nameOfProblem) {
        if (!createproblem)
        {
            createproblem = true;
            Time.timeScale = 0;
            GameObject newProblem = Instantiate(problem, transform.position, Quaternion.identity, gameObject.transform) as GameObject;
            newProblem.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.7f);
            
            ProblemClass.nameProblem = nameOfProblem;
            ProblemClass.problem = true;
        }
    }

    void TakeTarget() {
        RaycastHit2D target = Physics2D.Raycast(targetPosition, Vector2.zero, 0, LayerMask.GetMask("RoutePoint"));
        if (target.collider != null)
        {
            Route.Element curElement = target.collider.GetComponent<Route>().currentElement;
            //meshi+meshc
            if (curElement == Route.Element.MeshI || curElement == Route.Element.MeshC)
            {
                targetPosition = 2 * currentPosition - lastPosition;
            }
            //meshl
            else if (curElement == Route.Element.MeshL)
            {
                Route.MeshL curMeshL = target.collider.GetComponent<Route>().meshL;
                Vector2 temp;
                if (curMeshL == Route.MeshL.MeshL1 || curMeshL == Route.MeshL.MeshL3)
                {
                    temp = currentPosition - lastPosition;
                    targetPosition = currentPosition - new Vector2(temp.y, temp.x);
                }
                else if (curMeshL == Route.MeshL.MeshL2 || curMeshL == Route.MeshL.MeshL4)
                {
                    temp = currentPosition - lastPosition;
                    targetPosition = currentPosition + new Vector2(temp.y, temp.x);
                }
            }
            //mesht
            else if (curElement == Route.Element.MeshT) {
                Route.MeshTF curMeshTF = target.collider.GetComponent<Route>().meshTF;
                Route.MeshTT curMeshTT = target.collider.GetComponent<Route>().meshTT;
                Route.MeshTIO curMeshTIO = target.collider.GetComponent<Route>().meshTIO;

                Vector2 tempTIO = currentPosition - lastPosition;

                if ((curMeshTT == Route.MeshTT.MeshTTR && tempTIO.y == 1)||(curMeshTT == Route.MeshTT.MeshTTU && tempTIO.x == -1) ||(curMeshTT == Route.MeshTT.MeshTTL && tempTIO.y == -1) ||(curMeshTT == Route.MeshTT.MeshTTD && tempTIO.x == 1)) {
                    if (target.collider.GetComponent<Route>().meshTF == Route.MeshTF.MeshTFM)
                    {
                        if (target.collider.GetComponent<Route>().meshTIO != Route.MeshTIO.MeshTIO1)
                        {
                            //gameover
                            TextProblem("НЕВЕРНЫЙ ВХОД ИЛИ ВЫХОД!");
                        }
                    }
                    else if (target.collider.GetComponent<Route>().meshTF == Route.MeshTF.MeshTFP) {
                        if (target.collider.GetComponent<Route>().meshTIO1c[numberOfColor] != false)
                        {
                            //gameover
                            TextProblem("НЕВЕРНЫЙ ВХОД ИЛИ ВЫХОД!");
                        }
                    }
                }
                else if ((curMeshTT == Route.MeshTT.MeshTTR && tempTIO.x == -1) || (curMeshTT == Route.MeshTT.MeshTTU && tempTIO.y == -1) || (curMeshTT == Route.MeshTT.MeshTTL && tempTIO.x == 1) || (curMeshTT == Route.MeshTT.MeshTTD && tempTIO.y == 1)) {
                    if (target.collider.GetComponent<Route>().meshTF == Route.MeshTF.MeshTFM)
                    {
                        if (target.collider.GetComponent<Route>().meshTIO != Route.MeshTIO.MeshTIO2)
                        {
                            //gameover
                            TextProblem("НЕВЕРНЫЙ ВХОД ИЛИ ВЫХОД!");
                        }
                    }
                    else if (target.collider.GetComponent<Route>().meshTF == Route.MeshTF.MeshTFP)
                    {
                        if (target.collider.GetComponent<Route>().meshTIO2c[numberOfColor] != false)
                        {
                            //gameover
                            TextProblem("НЕВЕРНЫЙ ВХОД ИЛИ ВЫХОД!");
                        }
                    }
                }
                else if ((curMeshTT == Route.MeshTT.MeshTTR && tempTIO.y == -1) || (curMeshTT == Route.MeshTT.MeshTTU && tempTIO.x == 1) || (curMeshTT == Route.MeshTT.MeshTTL && tempTIO.y == 1) || (curMeshTT == Route.MeshTT.MeshTTD && tempTIO.x == -1))
                {
                    if (target.collider.GetComponent<Route>().meshTF == Route.MeshTF.MeshTFM)
                    {
                        if (target.collider.GetComponent<Route>().meshTIO != Route.MeshTIO.MeshTIO3)
                        {
                            //gameover
                            TextProblem("НЕВЕРНЫЙ ВХОД ИЛИ ВЫХОД!");
                        }
                    }
                    else if (target.collider.GetComponent<Route>().meshTF == Route.MeshTF.MeshTFP)
                    {
                        if (target.collider.GetComponent<Route>().meshTIO3c[numberOfColor] != false)
                        {
                            //gameover
                            TextProblem("НЕВЕРНЫЙ ВХОД ИЛИ ВЫХОД!");
                        }
                    }
                }

                if (curMeshTF == Route.MeshTF.MeshTFM) {
                    bool[][] meshTIO23 = { target.collider.GetComponent<Route>().meshTIO1c, target.collider.GetComponent<Route>().meshTIO2c, target.collider.GetComponent<Route>().meshTIO3c };
                    for (int i = 0; i < meshTIO23.Length; i++)
                    {
                        if (meshTIO23[i][numberOfColor] == true)
                        {
                            switch (i)
                            {
                                case 0:
                                    curMeshTIO = Route.MeshTIO.MeshTIO1;
                                    break;
                                case 1:
                                    curMeshTIO = Route.MeshTIO.MeshTIO2;
                                    break;
                                case 2:
                                    curMeshTIO = Route.MeshTIO.MeshTIO3;
                                    break;
                            }

                        }
                    }
                }

                if ((curMeshTT == Route.MeshTT.MeshTTR && curMeshTIO == Route.MeshTIO.MeshTIO2) || (curMeshTT == Route.MeshTT.MeshTTU && curMeshTIO == Route.MeshTIO.MeshTIO1) || (curMeshTT == Route.MeshTT.MeshTTD && curMeshTIO == Route.MeshTIO.MeshTIO3))
                {
                    targetPosition = currentPosition + Vector2.right;
                }
                else if ((curMeshTT == Route.MeshTT.MeshTTR && curMeshTIO == Route.MeshTIO.MeshTIO3) || (curMeshTT == Route.MeshTT.MeshTTU && curMeshTIO == Route.MeshTIO.MeshTIO2) || (curMeshTT == Route.MeshTT.MeshTTL && curMeshTIO == Route.MeshTIO.MeshTIO1))
                {
                    targetPosition = currentPosition + Vector2.up;
                }
                else if ((curMeshTT == Route.MeshTT.MeshTTD && curMeshTIO == Route.MeshTIO.MeshTIO1) || (curMeshTT == Route.MeshTT.MeshTTU && curMeshTIO == Route.MeshTIO.MeshTIO3) || (curMeshTT == Route.MeshTT.MeshTTL && curMeshTIO == Route.MeshTIO.MeshTIO2))
                {
                    targetPosition = currentPosition + Vector2.left;
                }
                else if ((curMeshTT == Route.MeshTT.MeshTTD && curMeshTIO == Route.MeshTIO.MeshTIO2) || (curMeshTT == Route.MeshTT.MeshTTR && curMeshTIO == Route.MeshTIO.MeshTIO1) || (curMeshTT == Route.MeshTT.MeshTTL && curMeshTIO == Route.MeshTIO.MeshTIO3))
                {
                    targetPosition = currentPosition + Vector2.down;
                }

            }
        }
    }
}
