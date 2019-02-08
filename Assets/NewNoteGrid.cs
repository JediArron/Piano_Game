using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewNoteGrid : MonoBehaviour
{
    [SerializeField] Slider zoomSlider;
    [SerializeField] GameObject centerPoint;
    [SerializeField] public int beatsPerMinute;
    [SerializeField] GameObject vertexSprite, vertexSprite2, vertexSprite4, vertexSprite8;
    Vector3[] zoom2Sprites, zoom2Sprites2;
    Vector3[] vertices;
    Dictionary<int, GameObject> storeSprites, storeSprites2, storeSprites4, storeSprites8;

    void Awake()
    {
        zoomSlider.onValueChanged.AddListener(OnValueChanged);
    }

    void Start()
    {
        CreateGrid();
        Zoom1();
    }

    void OnValueChanged(float nothing)
    {
        float currentValue = zoomSlider.value;

        if (currentValue == 1)
        {
            Zoom1();
        }
        else if (currentValue == 2)
        {
            Zoom2();
        }
    }

    void CreateGrid()
    {
        int xAxisSquares = (beatsPerMinute * 9) * 9;
        int yAxisSquares = 3;

        int increaseXTil = -650 + (xAxisSquares * 120);
        int increaseYTil = -190 + (yAxisSquares * 120);

        int xPosition = -650;
        int yPosition = -190;

        int vertexLoop = 1;
        int a = 0, b = 0, c = 0, d = 0;

        vertices = new Vector3[(xAxisSquares + 1) * (yAxisSquares + 1)];

        storeSprites = new Dictionary<int, GameObject>();
        storeSprites2 = new Dictionary<int, GameObject>();
        storeSprites4 = new Dictionary<int, GameObject>();
        storeSprites8 = new Dictionary<int, GameObject>();

        for (int i = 0; xPosition <= increaseXTil; xPosition += 120)
        {
            for (yPosition = -190; yPosition <= increaseYTil; yPosition += 120, i++)
            {
                vertices[i] = new Vector3(xPosition, yPosition, 0);

                if (i > 0 & i < 36 & i % 4 == 0)
                {
                    if (vertexLoop == 9)
                    {
                        vertexLoop = 0;
                    }

                    vertexLoop++;
                }
                else if (i >= 36 & i % 4 == 0)
                {
                    if (vertexLoop == 8 || vertexLoop == 9)
                    {
                        vertexLoop = 0;
                    }

                    vertexLoop++;
                }

                if (i < 36 & vertexLoop == 1 || i < 36 & vertexLoop == 9)
                {
                    GameObject instantiatedSprite = Instantiate(vertexSprite, vertices[i], Quaternion.identity);
                    //instantiatedSprite.transform.parent = gameObject.transform;
                    storeSprites.Add(a, instantiatedSprite);
                    a++;
                }
                else if (i >= 36 & vertexLoop == 8)
                {
                    GameObject instantiatedSprite = Instantiate(vertexSprite, vertices[i], Quaternion.identity);
                    //instantiatedSprite.transform.parent = gameObject.transform;
                    storeSprites.Add(a, instantiatedSprite);
                    a++;
                }
                else if (i < 36 & vertexLoop == 5)
                {
                    GameObject instantiatedSprite2 = Instantiate(vertexSprite2, vertices[i], Quaternion.identity);
                    //instantiatedSprite2.transform.parent = gameObject.transform;
                    storeSprites2.Add(b, instantiatedSprite2);
                    b++;
                }
                else if (i >= 36 & vertexLoop == 4)
                {
                    GameObject instantiatedSprite2 = Instantiate(vertexSprite2, vertices[i], Quaternion.identity);
                    //instantiatedSprite2.transform.parent = gameObject.transform;
                    storeSprites2.Add(b, instantiatedSprite2);
                    b++;
                }
                else if (i < 36 & vertexLoop == 3 || i < 36 & vertexLoop == 7)
                {
                    GameObject instantiatedSprite4 = Instantiate(vertexSprite4, vertices[i], Quaternion.identity);
                    //instantiatedSprite4.transform.parent = gameObject.transform;
                    storeSprites4.Add(c, instantiatedSprite4);
                    c++;
                }
                else if (i >= 36 & vertexLoop == 2 || i >= 36 & vertexLoop == 6)
                {
                    GameObject instantiatedSprite4 = Instantiate(vertexSprite4, vertices[i], Quaternion.identity);
                    //instantiatedSprite4.transform.parent = gameObject.transform;
                    storeSprites4.Add(c, instantiatedSprite4);
                    c++;
                }
                else if (i < 36 & vertexLoop == 2 || i < 36 & vertexLoop == 4 || i < 36 & vertexLoop == 6 || i < 36 & vertexLoop == 8)
                {
                    GameObject instantiatedSprite8 = Instantiate(vertexSprite8, vertices[i], Quaternion.identity);
                    //instantiatedSprite8.transform.parent = gameObject.transform;
                    storeSprites8.Add(d, instantiatedSprite8);
                    d++;
                }
                else if (i >= 36 & vertexLoop == 1 || i >= 36 & vertexLoop == 3 || i >= 36 & vertexLoop == 5 || i >= 36 & vertexLoop == 7)
                {
                    GameObject instantiatedSprite8 = Instantiate(vertexSprite8, vertices[i], Quaternion.identity);
                    //instantiatedSprite8.transform.parent = gameObject.transform;
                    storeSprites8.Add(d, instantiatedSprite8);
                    d++;
                }
            }
        }
    }

    void Zoom1()
    {
        for (int i = 0; i < storeSprites2.Count; i++) 
        {
            storeSprites2[i].SetActive(false);
        }

        for (int i = 0; i < storeSprites4.Count; i++)
        {
            storeSprites4[i].SetActive(false);
        }

        for (int i = 0; i < storeSprites8.Count; i++)
        {
            storeSprites8[i].SetActive(false);
        }

        for (int i = 0; i < storeSprites.Count; i++)
        {
            storeSprites[i].transform.position = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
            storeSprites[i].transform.parent = gameObject.transform;
        }
    }

    void Zoom2ReferenceGrid()
    {
        zoom2Sprites = new Vector3[storeSprites.Count];
        zoom2Sprites2 = new Vector3[storeSprites2.Count];

        int grid1ElementReference = 0;
        int grid2ElementReference = 4;

        for (int i = 0; i < storeSprites.Count; i++)
        {
            if (grid1ElementReference > 0 & grid1ElementReference % 4 == 0)
            {
                grid1ElementReference += 4;
            }

            zoom2Sprites[i] = new Vector3(vertices[grid1ElementReference].x, vertices[grid1ElementReference].y, vertices[grid1ElementReference].z);

            grid1ElementReference++;
        }

        for (int i = 0; i < storeSprites2.Count; i++)
        {
            if (grid2ElementReference > 4 & grid2ElementReference % 4 == 0)
            {
                grid2ElementReference += 4;
            }

            zoom2Sprites2[i] = new Vector3(vertices[grid2ElementReference].x, vertices[grid2ElementReference].y, vertices[grid2ElementReference].z);

            grid2ElementReference++;
        }
    }

    void Zoom2()
    {
        Zoom2ReferenceGrid();

        for (int i = 0; i < storeSprites2.Count; i++)
        {
            storeSprites2[i].SetActive(true);
        }

        for (int i = 0; i < storeSprites4.Count; i++)
        {
            storeSprites4[i].SetActive(false);
        }

        for (int i = 0; i < storeSprites8.Count; i++)
        {
            storeSprites8[i].SetActive(false);
        }

        for (int i = 0; i < storeSprites.Count; i++)
        {
            if (zoom2Sprites[i].x < centerPoint.transform.position.x)
            {
                storeSprites[i].transform.parent = null;
                storeSprites[i].transform.position = new Vector3(zoom2Sprites[i].x, zoom2Sprites[i].y, zoom2Sprites[i].z);
                storeSprites[i].transform.parent = gameObject.transform;
            }
        }

        for (int i = 0; i < storeSprites2.Count; i++)
        {
            if (zoom2Sprites2[i].x < centerPoint.transform.position.x)
            {
                storeSprites[i].transform.parent = null;
                storeSprites2[i].transform.position = new Vector3(zoom2Sprites2[i].x, zoom2Sprites2[i].y, zoom2Sprites2[i].z);
                storeSprites2[i].transform.parent = gameObject.transform;
            }
        }

        for (int i = 0; i < storeSprites.Count; i++)
        {
            if (zoom2Sprites[i].x >= centerPoint.transform.position.x)
            {
                storeSprites[i].transform.parent = null;
                storeSprites[i].transform.position = new Vector3(zoom2Sprites[i].x, zoom2Sprites[i].y, zoom2Sprites[i].z);
                storeSprites[i].transform.parent = gameObject.transform;
            }
        }

        for (int i = 0; i < storeSprites2.Count; i++)
        {
            if (zoom2Sprites2[i].x >= centerPoint.transform.position.x)
            {
                storeSprites2[i].transform.parent = null;
                storeSprites2[i].transform.position = new Vector3(zoom2Sprites2[i].x, zoom2Sprites2[i].y, zoom2Sprites2[i].z);
                storeSprites2[i].transform.parent = gameObject.transform;
            }
        }
    }

        
}
