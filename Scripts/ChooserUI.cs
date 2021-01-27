using UnityEngine;
using UnityEngine.UI;

public class ChooserUI : MonoBehaviour
{
    public AudioSource click;

    public GameObject play;
    public GameObject editor;
    public GameObject chooser;

    public GameObject chooseNew;
    public GameObject chooseMeshT;
    public GameObject chooseMeshTIOc;

    public GameObject toggle;
    public GameObject meshO1;
    public GameObject meshO2;
    public GameObject meshBackToTIO;

    public GameObject editClear;
    public GameObject editColor;

    public GameObject[] meshTtoRotate;

    public Sprite[] meshTIO;
    public Sprite[] meshOut1;
    public Sprite[] meshOut2;
    public Sprite[] meshIn1;
    public Sprite[] meshIn2;

    Vector2 startPos;
    Vector2 target;
    Vector2 cameraPostion;
    GameObject[] toggles;
    
    Route selectTile;
    GameObject tempObj;
    Route temptile;

    bool[] meshTIO1c;
    bool[] meshTIO2c;

    float infelicity = 0.05f;

    void Start() {
        //создание вспомогательного тайла для резерва параметров выбранного тайла
        tempObj = new GameObject("TempTile");
        temptile = tempObj.AddComponent<Route>();
    }

    void Update()
    {
        //зажатая мышь
        if (Input.GetMouseButtonDown(0) && editor.activeInHierarchy == true)
        {
            cameraPostion = Camera.main.transform.position;
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target = startPos;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPos;
            target = startPos - pos;
        }

        // начинаем скрипт с щелчка по тайлу
        if (Input.GetMouseButtonUp(0))
        {
            if (cameraPostion == (Vector2)Camera.main.transform.position)
            {
                if (target.x >= startPos.x - infelicity && target.y >= startPos.y - infelicity && target.x <= startPos.x + infelicity && target.y <= startPos.y + infelicity)
                {
                    RaycastHit2D buttonInfo = Physics2D.Raycast(target, Vector3.forward, 100, LayerMask.GetMask("Buttons"));
                    if (buttonInfo.collider == null)
                    {
                        if (editor.activeInHierarchy == true && SpawnPoint.active == false && chooser.activeInHierarchy == false)
                        {
                            RaycastHit2D[] tileInfo = Physics2D.RaycastAll(target, Vector3.forward, 100);
                            for (int i = 0; i < tileInfo.Length; i++)
                            {
                                if (tileInfo[i].collider != null && tileInfo[i].collider.gameObject.GetComponent<Route>() != null && tileInfo[i].collider.gameObject.GetComponent<Route>().isActiveAndEnabled)
                                {
                                    DeactivateChooserButton();
                                    editor.SetActive(false);
                                    selectTile = tileInfo[i].collider.gameObject.GetComponent<Route>();
                                    BackupTile(ref temptile, ref selectTile);
                                    if (selectTile.currentElement != Route.Element.Empty) {
                                        if (selectTile.currentElement == Route.Element.MeshT) {
                                            editColor.SetActive(true);
                                            editClear.SetActive(false);
                                        }
                                        else {
                                            editClear.SetActive(true);
                                            editColor.SetActive(false);
                                        }
                                    }
                                    else {
                                        editColor.SetActive(false);
                                        editClear.SetActive(false);
                                    }
                                    chooser.SetActive(true);
                                    chooseNew.SetActive(true);
                                    meshBackToTIO.SetActive(true);
                                    click.Play();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    
    //пересохранение тайла
    void BackupTile(ref Route _temptile, ref Route _selecttile) {
        _temptile.currentElement = _selecttile.currentElement;
        _temptile.meshI = _selecttile.meshI;
        _temptile.meshL = _selecttile.meshL;
        _temptile.meshTF = _selecttile.meshTF;
        _temptile.meshTT = _selecttile.meshTT;
        _temptile.meshTIO = _selecttile.meshTIO;
    }

    //выключение кнопок
    void DeactivateChooserButton() {
        chooseNew.SetActive(false);
        chooseMeshT.SetActive(false);
        chooseMeshTIOc.SetActive(false);
    }

    //назад без изменений
    public void BackToEditorWithoutChange() {
        BackupTile(ref selectTile, ref temptile);
        BackToEditor();
    }

    //назад на карту
    public void BackToEditor() {
        DeactivateChooserButton();
        chooser.SetActive(false);
        editor.SetActive(true);
    }

    //принятие изменений
    void ApplyOption() {
        selectTile.choose = true;
        BackToEditor();
        selectTile.colorEdit = false;
    }

    //удаление элемента
    public void EditClear() {
        selectTile.currentElement = Route.Element.Empty;
        ApplyOption();
    }

    //изменение цвета - переделать
    public void EditColor() {
        bool meshTFbool = true;
        DeactivateChooserButton();

        switch (selectTile.meshTT)
        {
            case Route.MeshTT.MeshTTR:
                MeshTR();
                break;
            case Route.MeshTT.MeshTTU:
                MeshTU();
                break;
            case Route.MeshTT.MeshTTL:
                MeshTL();
                break;
            case Route.MeshTT.MeshTTD:
                MeshTD();
                break;
        }
        switch (selectTile.meshTF) {
            case Route.MeshTF.MeshTFP:
                meshTFbool = false;
                switch (selectTile.meshTIO)
                {
                    case Route.MeshTIO.MeshTIO1:
                        MeshTPIO1();
                        break;
                    case Route.MeshTIO.MeshTIO2:
                        MeshTPIO2();
                        break;
                    case Route.MeshTIO.MeshTIO3:
                        MeshTPIO3();
                        break;
                }
                break;
            case Route.MeshTF.MeshTFM:
                meshTFbool = true;
                switch (selectTile.meshTIO)
                {
                    case Route.MeshTIO.MeshTIO1:
                        MeshTMIO1();
                        break;
                    case Route.MeshTIO.MeshTIO2:
                        MeshTMIO2();
                        break;
                    case Route.MeshTIO.MeshTIO3:
                        MeshTMIO3();
                        break;
                }
                break;
        }

        TakeColor(meshTFbool);
        meshBackToTIO.SetActive(false);
    }

    //вспомогательные методы
    void SelectTileEditRotate(float euler) {
        meshO1.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, euler));
        meshO2.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, euler));
    }
    void SelectTileEditColor(int i) {
        meshO1.GetComponent<Image>().sprite = (selectTile.meshTF == Route.MeshTF.MeshTFP) ? meshIn1[i] : meshOut1[i];
        meshO2.GetComponent<Image>().sprite = (selectTile.meshTF == Route.MeshTF.MeshTFP) ? meshIn2[i] : meshOut2[i];
    }

    //выбор meshI
    public void MeshIH() {
        selectTile.currentElement = Route.Element.MeshI;
        selectTile.meshI = Route.MeshI.MeshIH;
        ApplyOption();
    }
    public void MeshIV() {
        selectTile.currentElement = Route.Element.MeshI;
        selectTile.meshI = Route.MeshI.MeshIV;
        ApplyOption();
    }

    //выбор meshL
    public void MeshL1() {
        selectTile.currentElement = Route.Element.MeshL;
        selectTile.meshL = Route.MeshL.MeshL1;
        ApplyOption();
    }
    public void MeshL2() {
        selectTile.currentElement = Route.Element.MeshL;
        selectTile.meshL = Route.MeshL.MeshL2;
        ApplyOption();
    }
    public void MeshL3() {
        selectTile.currentElement = Route.Element.MeshL;
        selectTile.meshL = Route.MeshL.MeshL3;
        ApplyOption();
    }
    public void MeshL4() {
        selectTile.currentElement = Route.Element.MeshL;
        selectTile.meshL = Route.MeshL.MeshL4;
        ApplyOption();
    }

    //выбор meshC
    public void MeshC() {
        selectTile.currentElement = Route.Element.MeshC;
        ApplyOption();
    }

    //выбор meshT
    public void MeshTR() {
        selectTile.currentElement = Route.Element.MeshT;
        SelectMeshTT(Route.MeshTT.MeshTTR, 0);
    }
    public void MeshTU() {
        selectTile.currentElement = Route.Element.MeshT;
        SelectMeshTT(Route.MeshTT.MeshTTU, 90);
    }
    public void MeshTL() {
        selectTile.currentElement = Route.Element.MeshT;
        SelectMeshTT(Route.MeshTT.MeshTTL, 180);
    }
    public void MeshTD() {
        selectTile.currentElement = Route.Element.MeshT;
        SelectMeshTT(Route.MeshTT.MeshTTD, 270);
    }
    void SelectMeshTT(Route.MeshTT meshTT, float rotateZ) {
        selectTile.meshTT = meshTT;
        for (int i = 0; i < meshTtoRotate.Length; i++)
        {
            meshTtoRotate[i].GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, rotateZ);
        }
        DeactivateChooserButton();
        chooseMeshT.SetActive(true);
    }

    //назад к выбору new
    public void BackToNew() {
        DeactivateChooserButton();
        chooseNew.SetActive(true);
    }
    
    //назад к выбору mesht
    public void BackToMeshT() {
        DeactivateChooserButton();
        DestroyToggles();
        chooseMeshT.SetActive(true);
    }

    //выбор meshTIO
    public void MeshTPIO1() {
        NameMethod(0,0, Route.MeshTF.MeshTFP, Route.MeshTIO.MeshTIO1, selectTile.meshTIO2c, selectTile.meshTIO3c);
    }
    public void MeshTPIO2() {
        NameMethod(1,1, Route.MeshTF.MeshTFP, Route.MeshTIO.MeshTIO2, selectTile.meshTIO1c, selectTile.meshTIO3c);
    }
    public void MeshTPIO3() {
        NameMethod(2,2, Route.MeshTF.MeshTFP, Route.MeshTIO.MeshTIO3, selectTile.meshTIO1c, selectTile.meshTIO2c);
    }
    public void MeshTMIO1() {
        NameMethod(0,3, Route.MeshTF.MeshTFM, Route.MeshTIO.MeshTIO1, selectTile.meshTIO2c, selectTile.meshTIO3c);
    }
    public void MeshTMIO2() {
        NameMethod(1,4, Route.MeshTF.MeshTFM, Route.MeshTIO.MeshTIO2, selectTile.meshTIO1c, selectTile.meshTIO3c);
    }
    public void MeshTMIO3() {
        NameMethod(2,5, Route.MeshTF.MeshTFM, Route.MeshTIO.MeshTIO3, selectTile.meshTIO1c, selectTile.meshTIO2c);
    }

    //метод для подготовки экрана к окну выбора цвета
    void NameMethod(int i, int j, Route.MeshTF _meshTF, Route.MeshTIO _meshTIO, bool[] _meshTIO1c, bool[] _meshTIO2c) {

        selectTile.meshTF = _meshTF;
        selectTile.meshTIO = _meshTIO;
        meshBackToTIO.GetComponent<Image>().sprite = meshTIO[j];
        meshTIO1c = _meshTIO1c;
        meshTIO2c = _meshTIO2c;
        if (selectTile.meshTF == Route.MeshTF.MeshTFM) {
            NameMethod2(meshOut1, meshOut2, i, true);
        }
        else if (selectTile.meshTF == Route.MeshTF.MeshTFP) {
            NameMethod2(meshIn1, meshIn2, i, false);
        }
        chooseMeshT.SetActive(false);
        chooseMeshTIOc.SetActive(true);
    }

    //вспомогательный метод выбора цвета
    void NameMethod2(Sprite[] meshInOrOut1, Sprite[] meshInOrOut2, int i, bool colors) {
        meshO1.GetComponent<Image>().sprite = meshInOrOut1[i];
        meshO2.GetComponent<Image>().sprite = meshInOrOut2[i];

        Colors(colors);
    }

    //метод отрисовки переключателей цвета
    void Colors(bool minus) {
        float anchorX = 0.32f;

        toggles = new GameObject[selectTile.colors.Length * 2];
        for (int i = 0; i < selectTile.colors.Length; i++)
        {

            GameObject toggle1 = Instantiate(toggle, chooseMeshTIOc.transform) as GameObject;
            GameObject toggle2 = Instantiate(toggle, chooseMeshTIOc.transform) as GameObject;

            Toggle toggleA = toggle1.GetComponent<Toggle>();
            Toggle toggleB = toggle2.GetComponent<Toggle>();

            toggle1.GetComponent<RectTransform>().anchorMin = new Vector2(anchorX, 0.67f);
            toggle1.GetComponent<RectTransform>().anchorMax = new Vector2(anchorX + 0.05f, 0.75f);

            toggle2.GetComponent<RectTransform>().anchorMin = new Vector2(anchorX, 0.44f);
            toggle2.GetComponent<RectTransform>().anchorMax = new Vector2(anchorX + 0.05f, 0.52f);

            anchorX += 0.06f;

            toggleA.targetGraphic.color = selectTile.GetComponent<Route>().colors[i];
            toggleB.targetGraphic.color = selectTile.GetComponent<Route>().colors[i];

            if (minus)
            {
                ToggleGroup group = toggle1.AddComponent<ToggleGroup>();

                toggleA.group = group;
                toggleB.group = group;
            }

            toggles[i * 2] = toggle1;
            toggles[i * 2 + 1] = toggle2;
        }

        if (!minus)
        {
            ToggleGroup group1 = toggles[0].AddComponent<ToggleGroup>();
            ToggleGroup group2 = toggles[1].AddComponent<ToggleGroup>();

            for (int i = 0; i < toggles.Length / 2; i++)
            {
                toggles[i * 2].GetComponent<Toggle>().group = group1;
                toggles[i * 2 + 1].GetComponent<Toggle>().group = group2;
            }
        }
    }

    //методы для переключения цветов
    public void ChangeToggleUp() {
        if (selectTile.meshTF == Route.MeshTF.MeshTFM) {
            click.Play();
            for (int i = 0; i < toggles.Length / 2; i++) {
                toggles[i * 2].GetComponent<Toggle>().isOn = true;
            }
        }
    }
    public void ChangeToggleDown() {
        if (selectTile.meshTF == Route.MeshTF.MeshTFM) {
            click.Play();
            for (int i = 0; i < toggles.Length / 2; i++) {
                toggles[i * 2 + 1].GetComponent<Toggle>().isOn = true;
            }
        }
    }

    //метод для считывания цветов у готового элемента
    void TakeColor(bool minus) {
        if (minus)
        {
            for (int i = 0; i < selectTile.colors.Length; i++)
            {
                toggles[i * 2].GetComponent<Toggle>().isOn = meshTIO1c[i];
                toggles[i * 2 + 1].GetComponent<Toggle>().isOn = meshTIO2c[i];
            }
        }
        else {
            for (int i = 0; i < selectTile.colors.Length; i++)
            {
                toggles[i * 2].GetComponent<Toggle>().isOn = !meshTIO1c[i];
                toggles[i * 2 + 1].GetComponent<Toggle>().isOn = !meshTIO2c[i];
            }
        }
    }

    //метод для удаления переключателей
    void DestroyToggles() {
        for (int i = 0; i < selectTile.colors.Length; i++)
        {
            Destroy(toggles[i * 2]);
            Destroy(toggles[i * 2 + 1]);
        }
    }

    //метод для подтверждения выбора цветов
    public void ApplyColor() {
        for (int i = 0; i < selectTile.colors.Length; i++)
        {
            meshTIO1c[i] = (selectTile.meshTF == Route.MeshTF.MeshTFM) ? toggles[i * 2].GetComponent<Toggle>().isOn : !toggles[i * 2].GetComponent<Toggle>().isOn;
            meshTIO2c[i] = (selectTile.meshTF == Route.MeshTF.MeshTFM) ? toggles[i * 2 + 1].GetComponent<Toggle>().isOn : !toggles[i * 2 + 1].GetComponent<Toggle>().isOn;
        }
        DestroyToggles();
        ApplyOption();
        selectTile.colorEdit = true;
    }
}
