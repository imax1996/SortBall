using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject editor;
    Vector2 startPos, target;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && editor.activeInHierarchy == true)
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && editor.activeInHierarchy == true)
        {
            Vector2 pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPos;
            target = (Vector2)transform.position - pos;
        }
        if (editor.activeInHierarchy == true)
        {
            Camera.main.transform.position = new Vector3(Mathf.Clamp(target.x, -(MapGenerate.sizeOfMap / 2f - Camera.main.orthographicSize), (MapGenerate.sizeOfMap / 2f - Camera.main.orthographicSize)), Mathf.Clamp(target.y, -(MapGenerate.sizeOfMap / 2f - Camera.main.orthographicSize), (MapGenerate.sizeOfMap / 2f - Camera.main.orthographicSize)), -10);
        }
    }
}
