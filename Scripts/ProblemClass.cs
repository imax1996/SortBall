using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProblemClass : MonoBehaviour
{
    public static string nameProblem;
    public static bool problem;

    public GameObject problemObj;
    public Text textOfProblem;

    void Update()
    {
        if (problem) {
            problem = false;
            Error(nameProblem);
        }
    }

    public void Error(string text) {
        if (problemObj.activeInHierarchy != true) {
            StartCoroutine(CurError(text));
        }
    }

    IEnumerator CurError(string text)
    {
        textOfProblem.text = text;
        problemObj.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        problemObj.SetActive(false);
        yield return null;
    }
}
