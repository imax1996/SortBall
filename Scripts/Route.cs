using UnityEngine;

public class Route : MonoBehaviour
{
    public enum Element {Empty, MeshI, MeshL, MeshC, MeshT};
    public Element currentElement;
    public enum MeshI {MeshIH, MeshIV}
    public MeshI meshI;
    public enum MeshL {MeshL1, MeshL2, MeshL3, MeshL4};
    public MeshL meshL;

    public enum MeshTF {MeshTFP, MeshTFM};
    public MeshTF meshTF;
    public enum MeshTT {MeshTTR, MeshTTU, MeshTTL, MeshTTD};
    public MeshTT meshTT;
    public enum MeshTIO {MeshTIO1, MeshTIO2, MeshTIO3 };
    public MeshTIO meshTIO;

    public Color[] colors;

    public bool[] meshTIO1c;
    public bool[] meshTIO2c;
    public bool[] meshTIO3c;

    public bool colorEdit;
    bool colorcheck;

    public GameObject[] elements;
    GameObject element;

    public bool choose;

    void Start()
    {
        choose = false;
        colorEdit = false;
    }

    void Update()
    {
        if (choose)
        {
            switch (currentElement) {
                case Element.Empty:
                    gameObject.layer = 9;
                    break;
                default:
                    gameObject.layer = 8;
                    break;
            }

            switch (currentElement) {
                case Element.Empty:
                    DestroyElement();
                    break;
                case Element.MeshI:
                    switch (meshI)
                    {
                        case MeshI.MeshIH:
                            InstantiateElement(elements[0], Quaternion.Euler(new Vector3(0, 0, 0)));
                            break;
                        case MeshI.MeshIV:
                            InstantiateElement(elements[0], Quaternion.Euler(new Vector3(0, 0, 90)));
                            break;
                    }
                    break;

                case Element.MeshL:
                    switch (meshL)
                    {
                        case MeshL.MeshL1:
                            InstantiateElement(elements[1], Quaternion.Euler(new Vector3(0, 0, 0)));
                            break;
                        case MeshL.MeshL2:
                            InstantiateElement(elements[1], Quaternion.Euler(new Vector3(0, 0, 90)));
                            break;
                        case MeshL.MeshL3:
                            InstantiateElement(elements[1], Quaternion.Euler(new Vector3(0, 0, 180)));
                            break;
                        case MeshL.MeshL4:
                            InstantiateElement(elements[1], Quaternion.Euler(new Vector3(0, 0, 270)));
                            break;
                    }
                    break;

                case Element.MeshC:
                    InstantiateElement(elements[2], Quaternion.Euler(new Vector3(0, 0, 0)));
                    break;

                case Element.MeshT:
                    
                    switch (meshTT) {
                        case MeshTT.MeshTTR:
                            InstantiateElement(elements[3], Quaternion.Euler(new Vector3(0, 0, 0)));
                            break;
                        case MeshTT.MeshTTU:
                            InstantiateElement(elements[3], Quaternion.Euler(new Vector3(0, 0, 90)));
                            break;
                        case MeshTT.MeshTTL:
                            InstantiateElement(elements[3], Quaternion.Euler(new Vector3(0, 0, 180)));
                            break;
                        case MeshTT.MeshTTD:
                            InstantiateElement(elements[3], Quaternion.Euler(new Vector3(0, 0, 270)));
                            break;
                    }
                    switch (meshTF)
                    {
                        case MeshTF.MeshTFM:
                            colorcheck = false;
                            break;
                        case MeshTF.MeshTFP:
                            colorcheck = true;
                            break;
                    }
                    switch (meshTIO)
                    {
                        case MeshTIO.MeshTIO1:
                            ResetCheck(colorcheck);
                            break;
                        case MeshTIO.MeshTIO2:
                            ResetCheck(colorcheck);
                            break;
                        case MeshTIO.MeshTIO3:
                            ResetCheck(colorcheck);
                            break;
                    }

                    break;
            }

            choose = false;
        }
    }

    void ResetCheck(bool check) {
        switch (meshTIO)
        {
            case MeshTIO.MeshTIO1:
                for (int i = 0; i < meshTIO1c.Length; i++)
                {
                    meshTIO1c[i] = check;
                }
                break;
            case MeshTIO.MeshTIO2:
                for (int i = 0; i < meshTIO2c.Length; i++)
                {
                    meshTIO2c[i] = check;
                }
                break;
            case MeshTIO.MeshTIO3:
                for (int i = 0; i < meshTIO3c.Length; i++)
                {
                    meshTIO3c[i] = check;
                }
                break;
        }
    }

    void DestroyElement() {
        if (element != null) {
            Destroy(element);
        }
    }

    void InstantiateElement(GameObject _element, Quaternion _quaternion) {
        DestroyElement();
        element = Instantiate(_element, transform.position, _quaternion, gameObject.transform) as GameObject;
    }


}
