using System.Collections.Generic;
using UnityEngine;

public class MapGenerate : MonoBehaviour {

    System.Random randomMap;

    public static string seed;
    public static string codeMap;

    public GameObject quad;
    public GameObject spawnPoint;
    public GameObject exitPoint;
    public GameObject blockPoint;
    public GameObject routePoint;
    public static GameObject map;
    GameObject ballsPool;
    GameObject[,] tiles;
    Color[] colorsAll = {
        Color.blue,
        Color.green,
        Color.red,
        new Color(0, 0, 0.5f, 1), //фиолетовый
        new Color(0.5f, 1, 0.5f, 1), //салатовый
        new Color(1, 0.5f, 0, 1), //оранжевый
        new Color(1, 0.5f, 1, 1), //коричневый
        new Color(0, 1, 1, 1), //голубой
        new Color(0.5f, 0f, 0, 1),  //темнокрасный
        new Color(1, 0.5f, 0.5f, 1), //розовый
    }; 
    Color[] colors;

    public static int sizeOfMap;
    public static int countOfColor;
    public static int countOfSpawn;

    float startTime;
    float spawnTime;
    float plusTime;
    int countOfBlockPoint;
    int countexitpoints;

    //метод со случайной картой
    public void MapOption(int _sizeOfMap, int _countOfColor, int _countOfSpawn, bool _randomSeed)
    {
        //задаю посадку
        if (_randomSeed) {
            seed = UnityEngine.Random.Range(1000000, 9999999).ToString();
        }

        Generate(_sizeOfMap, _countOfColor, _countOfSpawn);
    }

    //метод с заданной картой
    public void MapOption(string _seed)
    {
        string[] words = _seed.Split(new char[] {'-'});
        seed = words[0];

        Generate(int.Parse(words[1]), int.Parse(words[2]), int.Parse(words[3]));
    }

    //генерация карты - начало
    void Generate(int _sizeOfMap, int _countOfColor, int _countOfSpawn) {
        //получаем посадку
        randomMap = new System.Random(seed.GetHashCode());

        //удаляю старую карту при её наличии
        if (map != null) {
            DestroyMap();
        }

        //определение переменных
        sizeOfMap = _sizeOfMap;
        countOfColor = _countOfColor;
        countOfSpawn = _countOfSpawn;

        //определяю точки выхода для их ресета, для условий победы и их счёт
        GameUI.exitpointsobj = new GameObject[countOfColor];
        GameUI.exitpoints = new bool[countOfColor];
        countexitpoints = 0;

        //изменяю размер стартовой камеры
        Camera.main.orthographicSize = (float)sizeOfMap / 2;

        //создание вспомогательных объектов
        map = new GameObject("Map");
        ballsPool = new GameObject("BallsPool");
        ballsPool.transform.parent = map.transform;

        //создаю новый массив тайлос и произвожу их генерацию
        tiles = new GameObject[sizeOfMap, sizeOfMap];
        TilesGenerate();

        //создаю массив цветов карты
        colors = new Color[countOfColor];
        for (int i = 0; i < countOfColor; i++) {
            colors[i] = colorsAll[i];
        }

        //создание вход-выход
        spawnTime = Mathf.Clamp01((float)countOfSpawn / countOfColor);
        plusTime = 0;
        startTime = 1.0f / countOfSpawn;
        for (int i = 0; i < countOfSpawn; i++) {
            CreatePoint(spawnPoint);
        }
        for (int i = 0; i < countOfColor; i++) {
            CreatePoint(exitPoint);
        }
        
        //вызываем метод постановки блокпоинтов
        if (sizeOfMap >= 6) {
            CreateBlockPoint();
        }

        CreateRoutePoint();
        CreateBoundPoint();

        codeMap = seed + "-" + sizeOfMap.ToString() + "-" + countOfColor.ToString() + "-" + countOfSpawn.ToString();
    }

    //генерация сетки поля
    void TilesGenerate() {
        //задаю стартовую позицию
        Vector2 pos = new Vector2(0.5f * (1 - sizeOfMap), 0.5f * (1 - sizeOfMap));

        for (int x = 0; x < sizeOfMap; x++) {
            for (int y = 0; y < sizeOfMap; y++) {
                //создаём тайлс
                GameObject quadObj = Instantiate(quad, new Vector2(pos.x + x, pos.y + y), Quaternion.identity, map.transform) as GameObject;
                quadObj.name = "(" + x + "," + y + ")";

                // перекрашиваем в серый границы
                if (x == 0 || x == sizeOfMap - 1 || y == 0 || y == sizeOfMap - 1) {
                    quadObj.GetComponent<SpriteRenderer>().color = Color.grey;
                }

                //определяем тайлс как объект
                tiles[x, y] = quadObj;
            }
        }
    }

    //метод создания вход-выход
    void CreatePoint(GameObject objCreate) {
        int side, vec2, x, y;

        //выбираем случайную клетку на границе
        do
        {
            side = randomMap.Next(0,4);
            vec2 = randomMap.Next(1, sizeOfMap - 1);

            switch (side) {
                case 0:
                    x = sizeOfMap - 1;
                    y = vec2;
                    break;
                case 1:
                    x = vec2;
                    y = sizeOfMap - 1;
                    break;
                case 2:
                    x = 0;
                    y = vec2;
                    break;
                case 3:
                    x = vec2;
                    y = 0;
                    break;
                default:
                    x = 1;
                    y = 0;
                    break;
            }
        }
        while (tiles[x, y].GetComponent<SpriteRenderer>().color != Color.grey);

        //создаём там поинт (сначала спавн, потом выход)
        GameObject point = Instantiate(objCreate, tiles[x, y].transform.position, Quaternion.identity) as GameObject;
        point.transform.parent = tiles[x,y].transform;
        tiles[x, y].GetComponent<SpriteRenderer>().color = Color.red;

        //если это выход
        if (objCreate == exitPoint) {
            //передаём цвет
            point.GetComponent<SpriteRenderer>().color = colors[countexitpoints];
            //определяем его как часть массива
            point.GetComponent<ExitPoint>().numberexitpoint = countexitpoints;
            GameUI.exitpointsobj[countexitpoints] = point;
            countexitpoints++;
        }
        //если это спавн, то...
        else if (objCreate == spawnPoint) {
            //передаём ему набор цветов
            point.GetComponent<SpawnPoint>().colors = colors;

            //определяем направление спавна
            switch (side)
            {
                case 0:
                    point.GetComponent<SpawnPoint>().dirX = -1;
                    point.GetComponent<SpawnPoint>().arrow.transform.rotation = Quaternion.Euler(0,0,180);
                    break;
                case 1:
                    point.GetComponent<SpawnPoint>().dirY = -1;
                    point.GetComponent<SpawnPoint>().arrow.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case 2:
                    point.GetComponent<SpawnPoint>().dirX = 1;
                    point.GetComponent<SpawnPoint>().arrow.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case 3:
                    point.GetComponent<SpawnPoint>().dirY = 1;
                    point.GetComponent<SpawnPoint>().arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }

            //вносим параметры
            point.GetComponent<SpawnPoint>().spawnTime = spawnTime;
            point.GetComponent<SpawnPoint>().startTime = plusTime;
            point.GetComponent<SpawnPoint>().ballPool = ballsPool;
            plusTime += startTime;
        }
    }

    //метод создания блокпоинтов
    void CreateBlockPoint() {

        Vector2[] blockpoints = new Vector2[sizeOfMap-5];
        List<int> listX = new List<int>();
        List<int> listY = new List<int>();

        for (int a = 2; a < sizeOfMap - 2; a++) {
            listX.Add(a);
            listY.Add(a);
        }

        for (int i = 0; i < blockpoints.Length; i++) {
            int c = randomMap.Next(0, sizeOfMap - 4 - i);
            int d = randomMap.Next(0, sizeOfMap - 4 - i);

            int _c = listX[c];
            int _d = listY[d];

            tiles[_c, _d].GetComponent<SpriteRenderer>().color = Color.black;
            tiles[_c, _d].layer = 9;

            listX.RemoveAt(c);
            listY.RemoveAt(d);
        }
    }

    void CreateRoutePoint() {
        for (int x = 0; x < sizeOfMap; x++) {
            for (int y = 0; y < sizeOfMap; y++) {
                if(tiles[x,y].GetComponent<SpriteRenderer>().color == Color.white) {
                    tiles[x,y].GetComponent<Route>().enabled = true;
                    tiles[x, y].GetComponent<Route>().meshTIO1c = new bool[colors.Length];
                    tiles[x, y].GetComponent<Route>().meshTIO2c = new bool[colors.Length];
                    tiles[x, y].GetComponent<Route>().meshTIO3c = new bool[colors.Length];
                    tiles[x, y].GetComponent<Route>().colors = colors;
                    tiles[x, y].layer = 9;
                }
            }
        }
    }

    void CreateBoundPoint()
    {
        for (int x = 0; x < sizeOfMap; x++)
        {
            for (int y = 0; y < sizeOfMap; y++)
            {
                if (tiles[x, y].GetComponent<SpriteRenderer>().color == Color.grey)
                {
                    tiles[x, y].layer = 9;
                }
            }
        }
    }

    public static void DestroyMap()
    {
        Destroy(map);
    }
}
