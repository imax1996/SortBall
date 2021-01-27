using UnityEngine;
using UnityEngine.UI;

public class RecomendedNumber : MonoBehaviour
{
    int sizeOfMap;
    int countOfColor;

    private string recomended;

    public Text textcolor;
    public Text textpoint;

    string textcolornumber;
    string textpointnumber;

    public void ResetLanguage() {
        textcolor.text = "";
        textpoint.text = "";
    }

    public void Size(string size) {
        if (size != null && int.TryParse(size, out sizeOfMap))
        sizeOfMap = int.Parse(size);
        RecomendedColor();
    }

    public void Colors(string colors) {
        if (colors != null && int.TryParse(colors, out countOfColor))
        countOfColor = int.Parse(colors);
        RecomendedColor();
    }

    void Text(int a,  int b) {
        if (GameUI.english)
        {
            textcolor.text = "RECOMMENDED VALUE UP TO " + a;
            textpoint.text = "RECOMMENDED VALUE UP TO " + b;
        }
        else
        {
            textcolor.text = "РЕКОМЕНДОВАННОЕ ЗНАЧЕНИЕ ДО " + a;
            textpoint.text = "РЕКОМЕНДОВАННОЕ ЗНАЧЕНИЕ ДО " + b;
        }
    }

    void RecomendedColor() {
        switch (sizeOfMap) {
            case 3:
                switch (countOfColor) {
                    case 2:
                        Text(2, 1);
                        break;
                    default:
                        Text(2, 2);
                        break;
                }
                break;
            case 4:
                switch (countOfColor) {
                    case 2:
                        Text(2, 1);
                        break;
                    default:
                        Text(2, 4);
                        break;
                }
                break;
            case 5:
                switch (countOfColor)
                {
                    case 2:
                        Text(4, 2);
                        break;
                    case 3:
                        Text(4, 1);
                        break;
                    case 4:
                        Text(4, 1);
                        break;
                    default:
                        Text(4, 5);
                        break;
                }
                break;
            case 6:
                switch (countOfColor)
                {
                    case 2:
                        Text(5, 3);
                        break;
                    case 3:
                        Text(5, 2);
                        break;
                    case 4:
                        Text(5, 1);
                        break;
                    case 5:
                        Text(5, 1);
                        break;
                    default:
                        Text(5, 6);
                        break;
                }
                break;
            case 7:
                switch (countOfColor)
                {
                    case 2:
                        Text(5, 3);
                        break;
                    case 3:
                        Text(5, 3);
                        break;
                    case 4:
                        Text(5, 1);
                        break;
                    default:
                        Text(5, 7);
                        break;
                }
                break;
            case 8:
                switch (countOfColor)
                {
                    case 2:
                        Text(5, 4);
                        break;
                    case 3:
                        Text(5, 3);
                        break;
                    case 4:
                        Text(5, 2);
                        break;
                    case 5:
                        Text(5, 1);
                        break;
                    default:
                        Text(5, 8);
                        break;
                }
                break;
            case 9:
                switch (countOfColor)
                {
                    case 2:
                        Text(6, 6);
                        break;
                    case 3:
                        Text(6, 3);
                        break;
                    case 4:
                        Text(6, 3);
                        break;
                    case 5:
                        Text(6, 2);
                        break;
                    case 6:
                        Text(6, 2);
                        break;
                    case 7:
                        Text(6, 1);
                        break;
                    default:
                        Text(6, 9);
                        break;
                }
                break;
            case 10:
                switch (countOfColor)
                {
                    case 2:
                        Text(7, 6);
                        break;
                    case 3:
                        Text(7, 4);
                        break;
                    case 4:
                        Text(7, 3);
                        break;
                    case 5:
                        Text(7, 2);
                        break;
                    case 6:
                        Text(7, 2);
                        break;
                    case 7:
                        Text(7, 2);
                        break;
                    default:
                        Text(7, 10);
                        break;
                }
                break;
            case 11:
                switch (countOfColor)
                {
                    case 2:
                        Text(10, 10);
                        break;
                    case 3:
                        Text(10, 6);
                        break;
                    case 4:
                        Text(10, 4);
                        break;
                    case 5:
                        Text(10, 3);
                        break;
                    case 6:
                        Text(10, 3);
                        break;
                    case 7:
                        Text(10, 2);
                        break;
                    case 8:
                        Text(10, 2);
                        break;
                    case 9:
                        Text(10, 2);
                        break;
                    case 10:
                        Text(10, 2);
                        break;
                    default:
                        Text(10, 11);
                        break;
                }
                break;
            case 12:
                switch (countOfColor)
                {
                    case 2:
                        Text(10, 11);
                        break;
                    case 3:
                        Text(10, 7);
                        break;
                    case 4:
                        Text(10, 5);
                        break;
                    case 5:
                        Text(10, 4);
                        break;
                    case 6:
                        Text(10, 3);
                        break;
                    case 7:
                        Text(10, 3);
                        break;
                    case 8:
                        Text(10, 3);
                        break;
                    case 9:
                        Text(10, 2);
                        break;
                    case 10:
                        Text(10, 2);
                        break;
                    default:
                        Text(10, 12);
                        break;
                }
                break;
            default:
                textcolor.text = "";
                textpoint.text = "";
                break;
        }
    }
}
