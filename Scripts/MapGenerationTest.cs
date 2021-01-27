using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerationTest : MonoBehaviour
{
    public GameObject[] inputFieldsCustom;
    bool randomSeed;
    bool problemactive = false;
    string seed;
    int seedcode;
    int sizeOfMap;
    int countOfColor;
    int countOfSpawn;

    public void Code(string code) {
        if (code != null && !code.Contains(" ")) seed = code;
    }

    public void Size(string size) {
        if (size != null && int.TryParse(size, out sizeOfMap))
        sizeOfMap = int.Parse(size);
    }

    public void Colors(string colors) {
        if (colors != null && int.TryParse(colors, out countOfColor))
        countOfColor = int.Parse(colors);
    }

    public void Spawn(string spawns) {
        if(spawns != null && int.TryParse(spawns, out countOfSpawn))
        countOfSpawn = int.Parse(spawns);
    }

    public void GenerateLevel() {
        if (sizeOfMap >= 3 && sizeOfMap <= 15 && countOfColor >= 1 && countOfColor <= 10 && countOfSpawn >= 1 && (countOfColor + countOfSpawn) <= ((sizeOfMap - 2) * 4))
        {
            gameObject.GetComponent<MapGenerate>().MapOption(sizeOfMap, countOfColor, countOfSpawn, true);
            gameObject.GetComponent<GameUI>().GameCreate(false);
        }
        else {
            if (!(sizeOfMap >= 3 && sizeOfMap <= 15)) {
                StartCoroutine(CurWrongText(0));
                gameObject.GetComponent<ProblemClass>().Error(Language("WRONG MAP SIZE!", "НЕВЕРНЫЙ РАЗМЕР КАРТЫ!"));
            }
            else if (!(countOfColor >= 1 && countOfColor <= 10)) {
                StartCoroutine(CurWrongText(1));
                gameObject.GetComponent<ProblemClass>().Error(Language("WRONG NUMBER OF COLORS!", "НЕВЕРНОЕ ЧИСЛО ЦВЕТОВ!"));
            }
            else if (!(countOfSpawn >= 1)) {
                StartCoroutine(CurWrongText(2));
                gameObject.GetComponent<ProblemClass>().Error(Language("NO SPAWNPOINTS!", "НЕТ ТОЧЕК ВХОДА!"));
            }
            else if (!((countOfColor + countOfSpawn) <= ((sizeOfMap - 2) * 4))) {
                int count = 0;
                string text = "";
                if ((((sizeOfMap - 2) * 4) - countOfColor) > 0) {
                    StartCoroutine(CurWrongText(2));
                    count = (((sizeOfMap - 2) * 4) - countOfColor);
                    text = Language("NUMBER OF SPAWNPOINT IN THIS CASE SHOULD BE NO MORE THAN ", "КОЛИЧЕСТВО ВХОДОВ В ДАННОМ СЛУЧАЕ ДОЛЖНО БЫТЬ НЕ БОЛЕЕ ") + count.ToString() + "!";
                }
                else {
                    StartCoroutine(CurWrongText(1));
                    text = Language("TOO MUCH COLORS AT LEVEL!", "СЛИШКОМ МНОГО ЦВЕТОВ НА УРОВНЕ!");
                }
                gameObject.GetComponent<ProblemClass>().Error(text);
            }
        }
    }

    public void GenerateCodeLevel() {
        if (seed != null && !seed.Contains(" "))
        {
            string[] words;
            words = seed.Split(new char[] { '-' });

            if (words.Length == 4 && int.TryParse(words[0], out seedcode) && int.TryParse(words[1], out sizeOfMap) && int.TryParse(words[2], out countOfColor) && int.TryParse(words[3], out countOfSpawn))
            {
                if (int.Parse(words[0]) >= 1000000 && int.Parse(words[0]) <= 9999999
                    && int.Parse(words[1]) >= 3 && int.Parse(words[1]) <= 15
                    && int.Parse(words[2]) >= 1 && int.Parse(words[2]) <= 10
                    && int.Parse(words[3]) >= 1
                    && (int.Parse(words[2]) + int.Parse(words[3])) <= ((int.Parse(words[1]) - 2) * 4)
                    )
                {
                    gameObject.GetComponent<MapGenerate>().MapOption(seed);
                    gameObject.GetComponent<GameUI>().GameCreate(false);
                }
                else
                {
                    gameObject.GetComponent<ProblemClass>().Error(Language("WRONG LEVEL CODE!", "НЕВЕРНЫЙ КОД!"));
                }
            }
            else
            {
                gameObject.GetComponent<ProblemClass>().Error(Language("WRONG LEVEL CODE!", "НЕВЕРНЫЙ КОД!"));
            }
        }
        else
        {
            gameObject.GetComponent<ProblemClass>().Error(Language("WRONG LEVEL CODE!", "НЕВЕРНЫЙ КОД!"));
        }
    }

    IEnumerator CurWrongText(int number) {
        if (problemactive != true) {
            problemactive = true;
            Color defaultColor = inputFieldsCustom[number].GetComponent<Image>().color;
            for (int i = 0; i < 2; i++)
            {
                inputFieldsCustom[number].GetComponent<Image>().color = Color.red;
                yield return new WaitForSeconds(0.2f);
                inputFieldsCustom[number].GetComponent<Image>().color = defaultColor;
                yield return new WaitForSeconds(0.2f);
            }
            problemactive = false;
        }
    }

    string Language(string en, string ru) {
        if (GameUI.english)
        {
            return en;
        }
        else {
            return ru;
        }
    }
}
