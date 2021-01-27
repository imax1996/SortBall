using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public GameObject logo;
    public Image fadeImage;
    public GameObject optionsMenu;
    public GameObject trainingObj;
    public GameObject startMenu;
    public GameObject winGame;
    public GameObject customMenu;
    public GameObject codeEnterMenu;
    public GameObject customLevelsMenu;
    public GameObject play;
    public GameObject editor;
    public GameObject gameMenu;
    public GameObject chooser;
    public GameObject winMenu;
    public GameObject buttonwinstory;
    public GameObject buttonwincustom;
    public GameObject skipLevelButton;
    public GameObject skipsLevels;
    public GameObject monetizationMenu;
    public GameObject button_noads_fade;
    public Button noads_button;

    public GameObject soundToggle;
    public GameObject[] levels;

    public Text[] codeMap;
    public Text leveltext;
    public Text nextleveltext;
    public Text wintext;
    public Text skipLevelText;

    Vector3 cameraPos;
    float cameraSize;
    bool storymode;
    int skipsLevel;

    public static bool english;
    public static GameObject[] exitpointsobj;
    public static bool[] exitpoints;
    public static int level;

    Training training;
    MapGenerate mapGenerate;
    MapGenerationTest generationTest;

    void Start()
    {
        //запуск игры и заставки(лого)
        StartCoroutine(CurLogo());
    }

    //лого-заставка
    IEnumerator CurLogo()
    {
        float speed = 1.5f;
        float percent = 0;
        while (percent <= 1) {
            percent += Time.deltaTime / speed;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.Lerp(1f, 0f, percent));
            yield return null;
        }

        //базовые параметры
        mapGenerate = gameObject.GetComponent<MapGenerate>();
        generationTest = gameObject.GetComponent<MapGenerationTest>();
        ExitPoint.WinnableGame += Winnable;
        level = 0;
        skipsLevel = 0;

        //загрузка уровня
        SaveData saveFile = new SaveData();
        saveFile = (SaveData)SaveLoadManager.LoadGame();
        if (saveFile != null)
        {
            level = saveFile.currentLevel;
            skipsLevel = saveFile.skiplevel;
        }

        if (!PlayerPrefs.HasKey("Language"))
        {
            if (Application.systemLanguage == SystemLanguage.Russian || Application.systemLanguage == SystemLanguage.Ukrainian || Application.systemLanguage == SystemLanguage.Belarusian)
            {
                PlayerPrefs.SetString("Language", "Russian");
                english = false;
            }
            else {
                PlayerPrefs.SetString("Language", "English");
                english = true;

            }
        }
        else if (PlayerPrefs.GetString("Language") == "English")
        {
            english = true;
        }
        else {
            english = false;
        }

        startMenu.SetActive(true);
        soundToggle.SetActive(true);
        skipsLevels.SetActive(true);
        UpdateText();
        logo.SetActive(false);
        yield return null;
    }

    //сохранение уровня
    void SaveGame() {
        SaveData saveFile = new SaveData
        {
            currentLevel = level,
            skiplevel = skipsLevel,
        };
        SaveLoadManager.SaveGame(saveFile);
    }

    public void BuyProductPasses(int passes) {
        skipsLevel += passes;
        UpdateText();
        SaveGame();
    }

    public void UpdateText() {
        skipLevelText.text = skipsLevel.ToString();

        if (level < Levels.levels.Length)
        {
            leveltext.text = Language("LEVEL ", "УРОВЕНЬ ") + (level + 1);
        }
        else
        {
            leveltext.text = Language("WAIT FOR NEW LEVELS", "ОЖИДАЙТЕ НОВЫЕ УРОВНИ");
        }
    }

    void Winnable() {
        if (english) {
            wintext.text = TextWin.ENwinText[Random.Range(0, TextWin.ENwinText.Length)];
        }
        else {
            wintext.text = TextWin.RUwinText[Random.Range(0, TextWin.RUwinText.Length)];
        }
        codeMap[1].text = Language("LEVEL CODE: ", "КОД УРОВНЯ: ") + MapGenerate.codeMap;
        ResetMenu();
        winMenu.SetActive(true);

        if (storymode == true) {
            storymode = false;
            skipsLevels.SetActive(true);
            if ((level - 4) % 10 == 0)
            {
                skipsLevel++;
            }

            level++;
            SaveGame();
        }
    }

    //обнуление кнопок
    void ResetMenu() {
        monetizationMenu.SetActive(false);
        skipsLevels.SetActive(false);
        skipLevelButton.SetActive(false);
        soundToggle.SetActive(false);
        logo.SetActive(false);
        startMenu.SetActive(false);
        customMenu.SetActive(false);
        codeEnterMenu.SetActive(false);
        customLevelsMenu.SetActive(false);
        play.SetActive(false);
        editor.SetActive(false);
        gameMenu.SetActive(false);
        chooser.SetActive(false);
        winMenu.SetActive(false);
    }

    //стоп в игре
    public void StopGame() {
        SpawnPoint.active = false;
        ResetMenu();
        for (int i = 0; i < exitpointsobj.Length; i++)
        {
            exitpointsobj[i].GetComponent<ExitPoint>().ResetPoint();
            exitpoints[i] = false;
        }
        Camera.main.orthographicSize = cameraSize;
        Camera.main.transform.position = cameraPos;
        editor.SetActive(true);
    }

    //старт в игре
    public void StartGame() {
        cameraPos = Camera.main.transform.position;
        cameraSize = Camera.main.orthographicSize;
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.orthographicSize = MapGenerate.sizeOfMap / 2f;

        SpawnPoint.active = true;
        ResetMenu();
        play.SetActive(true);

        Ball.createproblem = false;
    }

    //пауза в игре
    public void GameMenu() {
        ResetMenu();
        gameMenu.SetActive(true);
        soundToggle.SetActive(true);
        if (storymode) {
            skipLevelButton.SetActive(true);
            skipsLevels.SetActive(true);
        }
        codeMap[0].text = Language("LEVEL CODE: ", "КОД УРОВНЯ: ") + MapGenerate.codeMap;
        UpdateText();
    }

    //кнопки в меню в игре
    public void ContinueGame() {
        ResetMenu();
        editor.SetActive(true);
    }
    public void SkipLevel() {
        if (skipsLevel > 0)
        {
            skipsLevel--;
            UpdateText();
            if ((level - 4) % 10 == 0)
            {
                skipsLevel++;
            }
            level++;
            SaveGame();
            cameraPos = Camera.main.transform.position;
            cameraSize = Camera.main.orthographicSize;
            NextLevelStoryMode();
        }
        else {
            StoreOn();
        }
    }
    public void ResetGame() {
        ResetMenu();
        mapGenerate.MapOption(MapGenerate.codeMap);
        editor.SetActive(true);
    }
    public void ExitToStartMenu() {
        ResetMenu();
        MapGenerate.DestroyMap();
        Training(5);
        UpdateText();
        startMenu.SetActive(true);
        soundToggle.SetActive(true);
        skipsLevels.SetActive(true);
    }
    public void ExitToStartMenuFromGame() {
        ExitToStartMenu();
    }
    //стартовое меню
    public void StoryMode() {
        if (level < Levels.levels.Length)
        {
            ResetMenu();
            mapGenerate.MapOption(Levels.levels[level]);
            nextleveltext.text = Language("LEVEL ", "УРОВЕНЬ ") + (level + 2);
            editor.SetActive(true);
            buttonwincustom.SetActive(false);
            buttonwinstory.SetActive(true);
            storymode = true;
            Training(level);
        }
        else {
            winGame.SetActive(true);
        }
    }
    public void CustomGame() {
        ResetMenu();
        customMenu.SetActive(true);
    }
    public void ExitGame() {
        Application.Quit();
    }

    //кастомное меню
    public void EnterCode() {
        ResetMenu();
        codeEnterMenu.SetActive(true);
    }
    public void CustomLevelGame() {
        ResetMenu();
        customLevelsMenu.SetActive(true);
    }
    public void BackToMenu() {
        ResetMenu();
        UpdateText();
        startMenu.SetActive(true);
        soundToggle.SetActive(true);
        skipsLevels.SetActive(true);
    }

    //ввод кода меню
    public void GenerateCodeLevel() {
        generationTest.GenerateCodeLevel();
    }

    //кастомный уровень меню
    public void GenerateCustomLevel() {
        generationTest.GenerateLevel();
    }

    //возврат в кастомное меню
    public void BackToCustomMenu() {
        ResetMenu();
        customMenu.SetActive(true);
    }

    //меню победы (кастом)
    public void ExitToCustomMenu() {
        ResetMenu();
        MapGenerate.DestroyMap();
        customLevelsMenu.SetActive(true);
        Training(5);
    }
    public void NextLevelCustom() {
        NextLevel();
        mapGenerate.MapOption(MapGenerate.sizeOfMap, MapGenerate.countOfColor, MapGenerate.countOfSpawn, true);
        GameCreate(false);
    }

    //меню победы (сюжет)
    public void NextLevelStoryMode() {
        NextLevel();

        if (level < Levels.levels.Length)
        {
            GameCreate(true);
            mapGenerate.MapOption(Levels.levels[level]);
            nextleveltext.text = Language("LEVEL ", "УРОВЕНЬ ") + (level + 2);
            Training(level);
        }
        else
        {
            ExitToStartMenu();
            winGame.SetActive(true);
        }
    }

    //вспомогательные методы класса
    void NextLevel() {
        StopGame();
        Camera.main.transform.position = new Vector3(0,0,-10);
        MapGenerate.map.SetActive(false);
        MapGenerate.DestroyMap();
    }
    public void GameCreate(bool _storymode) {
        ResetMenu();
        editor.SetActive(true);
        buttonwincustom.SetActive(!_storymode);
        buttonwinstory.SetActive(_storymode);
        storymode = _storymode;
    }

    //управление камерой
    public void PlusCamera() {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 1f, 1.5f, MapGenerate.sizeOfMap / 2f);
    }
    public void MinusCamera() {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + 1f, 1.5f, MapGenerate.sizeOfMap / 2f);
    }
    public void HomeCamera() {
        Camera.main.orthographicSize = MapGenerate.sizeOfMap / 2f;
    }

    //проверка режима тренировки

    //отключение банера победы
    public void WinGameOff() {
        winGame.SetActive(false);
    }

    //включение магазина
    public void StoreOn() {
        monetizationMenu.SetActive(true);
    }

    //выход из магазина
    public void StoreOff() {
        monetizationMenu.SetActive(false);
    }

    // трейнинг
    void Training(int _level) {
        if (level < 6)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                if (i == _level)
                {
                    levels[i].SetActive(true);
                }
                else
                {
                    levels[i].SetActive(false);
                }
            }
        }
    }

    public void OptionsOn() {
        optionsMenu.SetActive(true);
    }

    public void OptionsOff() {
        optionsMenu.SetActive(false);
    }

    //язык
    string Language(string en, string ru) {
        if (english)
        {
            return en;
        }
        else {
            return ru;
        }
    }
}