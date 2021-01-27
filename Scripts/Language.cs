using UnityEngine;

public class Language : MonoBehaviour
{
    public void ru_RU() {
        PlayerPrefs.SetString("Language", "Russian");
        GameUI.english = false;
    }
    public void en_EN() {
        PlayerPrefs.SetString("Language", "English");
        GameUI.english = true;
    }
}
