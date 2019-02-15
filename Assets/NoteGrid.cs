using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteGrid : MonoBehaviour
{
    [SerializeField] int beatsPerMinute;
    [SerializeField] GameObject vertexSprite, vertexSpriteZoom2, vertexSpriteZoom4, vertexSpriteZoom8;
    [SerializeField] Slider zoomSlider;
    Vector3[] vertices, vertices2, vertices4, vertices8;
    Dictionary<int, GameObject> storeInstantiation, storeInstantiation2, storeInstantiation4, storeInstantiation8;
    [SerializeField] GameObject leftPoint;
    [SerializeField] GameObject centerPoint;
    [SerializeField] ScrollRect scrollRect;

    void Awake()
    {
        zoomSlider.onValueChanged.AddListener(OnValueChanged);
    }

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();

        CreateGrid();
        PlaceSprite();
    }

    float prevValue;
    float currentValue = 1;

    void OnValueChanged(float value)
    {
        prevValue = currentValue;
        currentValue = zoomSlider.value;

        scrollRect.StopMovement();

        RepositionGrid();
        RepositionGrid2();
        RepositionGrid4();
    }

    void CreateGrid()
    {
        int xPosition = -650;
        int yPosition = -190;

        int yAxisSquares = 3;
        int xAxisSquares = beatsPerMinute * 9;

        vertices = new Vector3[(xAxisSquares + 1) * (yAxisSquares + 1)];

        vertices[0] = new Vector3(xPosition, yPosition, 0);

        float increaseXTil = vertices[0].x + (xAxisSquares * 120);
        float increaseYTil = vertices[0].y + (yAxisSquares * 120);

        for (int i = 0; xPosition <= increaseXTil; xPosition += 120)
        {
            for (yPosition = -190; yPosition <= increaseYTil; yPosition += 120)
            {
                vertices[i] = new Vector3(xPosition, yPosition, 0);
                i++;
            }
        }
    }

    void PlaceSprite()
    {
        storeInstantiation = new Dictionary<int, GameObject>();

        for (int i = 0; i < vertices.Length; i++)
        {
            GameObject instantiatedSprite = Instantiate(vertexSprite, vertices[i], Quaternion.identity);

            storeInstantiation.Add(i, instantiatedSprite);

            storeInstantiation[i].transform.parent = gameObject.transform;
        }
    }

    void RepositionGrid()
    {
        int subtractFromXAxis = 0;
        int shiftLeftValue = 0;

        int addToXAxis = 0;
        int shiftRightValue = 0;

        if (currentValue == 1)
        {
            shiftLeftValue = 0;
            shiftRightValue = 0;
        }
        else if (currentValue == 2)
        {
            shiftLeftValue = -120;
            shiftRightValue = 120;
        }
        else if (currentValue == 3)
        {
            shiftLeftValue = -360;
            shiftRightValue = 360;
        }
        else if (currentValue == 4)
        {
            shiftLeftValue = -840;
            shiftRightValue = 840;
        }

        for (int i = storeInstantiation.Count - 1; i >= 0; i--)
        {
            if (storeInstantiation[i].transform.position.x < centerPoint.transform.position.x)
            {
                storeInstantiation[i].transform.position = new Vector3(vertices[i].x + subtractFromXAxis, vertices[i].y, vertices[i].z);

                if (i % 4 == 0)
                {
                    subtractFromXAxis += shiftLeftValue;
                }
            }
        }

        for (int i = 0; i < storeInstantiation.Count; i++)
        {
            if (storeInstantiation[i].transform.position.x >= centerPoint.transform.position.x)
            {
                if (i % 4 == 0)
                {
                    addToXAxis += shiftRightValue;
                }

                storeInstantiation[i].transform.position = new Vector3(vertices[i].x + addToXAxis, vertices[i].y, vertices[i].z);
            }
        }
    }

    void CreateGridZoom2()
    {
        int xPosition = -650;
        int yPosition = -190;
        int xAxisSquares = beatsPerMinute * 9;
        int yAxisSquares = 3;

        int xPosition2 = xPosition + 120;

        vertices2 = new Vector3[(xAxisSquares + 1) * (yAxisSquares + 1)];
        vertices2[0] = new Vector3(xPosition2, yPosition, 0);

        float increaseXTil = vertices2[0].x + xAxisSquares * 120;
        float increaseYTil = vertices2[0].y + yAxisSquares * 120;

        for (int i = 0; xPosition2 <= increaseXTil; xPosition2 += 120)
        {
            for (yPosition = -190; yPosition <= increaseYTil; yPosition += 120)
            {
                vertices2[i] = new Vector3(xPosition2, yPosition, 0);
                i++;
            }
        }
    }

    void PlaceSprite2()
    {
        storeInstantiation2 = new Dictionary<int, GameObject>();

        for (int i = 0; i < vertices2.Length; i++)
        {
            GameObject instantiatedSprite2 = Instantiate(vertexSpriteZoom2, vertices2[i], Quaternion.identity);

            storeInstantiation2.Add(i, instantiatedSprite2);

            storeInstantiation2[i].transform.parent = gameObject.transform;
        }

        gridCounterZoom2++;
    }

    int gridCounterZoom2 = 0;

    void RepositionGrid2()
    {
        int shiftLeftValue = 0;
        int subtractFromXAxis = 0;

        int shiftRightValue = 0;
        int addToXAxis = 0;

        if (gridCounterZoom2 == 0 & currentValue >= 2)
        {
            CreateGridZoom2();
            PlaceSprite2();
        }

        if (gridCounterZoom2 >= 1 & currentValue == 1)
        {
            for (int i = 0; i < storeInstantiation2.Count; i++)
            {
                storeInstantiation2[i].SetActive(false);
            }
        }
        else if (currentValue >= 2)
        {
            for (int i = 0; i < storeInstantiation2.Count; i++)
            {
                storeInstantiation2[i].SetActive(true);
            }
        }

        if (currentValue == 1)
        {
            shiftLeftValue = 0;
            shiftRightValue = 0;
        }
        else if (currentValue == 2)
        {
            shiftLeftValue = -120;
            subtractFromXAxis = -120;

            shiftRightValue = 120;
            addToXAxis = -120;
        }
        else if (currentValue == 3)
        {
            shiftLeftValue = -360;
            subtractFromXAxis = -240;

            shiftRightValue = 360;
            addToXAxis = -240;
        }
        else if (currentValue == 4)
        {
            shiftLeftValue = -480;
            subtractFromXAxis = -240;

            shiftRightValue = 360;
            addToXAxis = 120;
        }

        for (int i = storeInstantiation2.Count - 1; i >= 0; i--)
        {
            if (storeInstantiation2[i].transform.position.x < centerPoint.transform.position.x)
            {
                storeInstantiation2[i].transform.position = new Vector3(vertices2[i].x + subtractFromXAxis, vertices2[i].y, vertices2[i].z);

                if (i % 4 == 0)
                {
                    subtractFromXAxis += shiftLeftValue;
                }
            }
        }

        for (int i = 0; i < storeInstantiation2.Count; i++)
        {
            if (storeInstantiation2[i].transform.position.x >= centerPoint.transform.position.x)
            {
                if (i % 4 == 0)
                {
                    addToXAxis += shiftRightValue;
                }

                storeInstantiation2[i].transform.position = new Vector3(vertices2[i].x + addToXAxis, vertices2[i].y, vertices[2].z);
            }
        }
    }

    void CreateGridZoom4()
    {
        int xPosition = -650;
        int yPosition = -190;
        int yAxisSquares = 3;
        int xAxisSquares = beatsPerMinute * 9;

        int xPosition4 = xPosition + 120;

        int vertices4ArraySize = ((xAxisSquares + 1) * (yAxisSquares + 1) * 2);

        vertices4 = new Vector3[vertices4ArraySize];

        vertices4[0] = new Vector3(xPosition4, yPosition, 0);

        float increaseXTil = Convert.ToInt32((vertices4ArraySize / 4 * 120) + xPosition4);
        float increaseYTil = vertices4[0].y + (yAxisSquares * 120);

        for (int i = 0; xPosition4 < increaseXTil; xPosition4 += 120)
        {
            for (yPosition = -190; yPosition <= increaseYTil; yPosition += 120)
            {
                vertices4[i] = new Vector3(xPosition4, yPosition, 0);
                i++;
            }
        }
    }

    void PlaceSprite4()
    {
        storeInstantiation4 = new Dictionary<int, GameObject>();

        for (int i = 0; i < vertices4.Length; i++)
        {
            GameObject instantiatedSprite4 = Instantiate(vertexSpriteZoom4, vertices4[i], Quaternion.identity);

            instantiatedSprite4.transform.parent = gameObject.transform;

            storeInstantiation4.Add(i, instantiatedSprite4);

            storeInstantiation4[i].transform.parent = gameObject.transform;
        }
        gridCounterZoom4++;
    }

    int gridCounterZoom4 = 0;

    void RepositionGrid4()
    {
        if (gridCounterZoom4 == 0)
        {
            CreateGridZoom4();
            PlaceSprite4();
        }

        int shiftLeftValue = 0;
        int subtractFromXAxis = 0;

        int shiftRightValue = 0;
        int addToXAxis = 0;

        if (currentValue < 3)
        {
            for (int i = 0; i < storeInstantiation4.Count; i++)
            {
                storeInstantiation4[i].SetActive(false);
            }
        }
        else if (currentValue >= 3)
        {
            for (int i = 0; i < storeInstantiation4.Count; i++)
            {
                storeInstantiation4[i].SetActive(true);
            }
        }

        if (currentValue == 3)
        {
            shiftLeftValue = -120;
            subtractFromXAxis = -120;

            shiftRightValue = 120;
            addToXAxis = 0;
        }

        int displacementCounter = 0;

        for (int i = storeInstantiation4.Count - 1; i >= 0; i--)
        {
            if (storeInstantiation4[i].transform.position.x < centerPoint.transform.position.x)
            {
                storeInstantiation4[i].transform.position = new Vector3(vertices4[i].x + subtractFromXAxis, vertices4[i].y, vertices4[i].z);

                if (i % 4 == 0)
                {
                    subtractFromXAxis += shiftLeftValue;
                }
            }
        }

        //void CreateGridZoom8()
        //{
        //    int xPosition = -650;
        //    int yPosition = -190;
        //    int yAxisSquares = 3;
        //    int xAxisSquares = beatsPerMinute * 9;

        //    int xPosition8 = xPosition + 0;

        //    int vertices8ArraySize = ((xAxisSquares + 1) * (yAxisSquares + 1)) * 4;

        //    vertices8 = new Vector3[vertices8ArraySize];

        //    vertices8[0] = new Vector3(xPosition8, yPosition, 0);

        //    float increaseXTil = (vertices8ArraySize / 4 * 120) + xPosition8;
        //    float increaseYTil = vertices8[0].y + (yAxisSquares * 120);

        //    for (int i = 0; xPosition8 < increaseXTil; xPosition8 += 120)
        //    {
        //        for (yPosition = -190; yPosition <= increaseYTil; yPosition += 120)
        //        {
        //            vertices8[i] = new Vector3(xPosition8, yPosition, 0);
        //            i++;
        //        }
        //    }
        //}

        //void PlaceSprite8()
        //{
        //    storeSprites8 = new Dictionary<int, GameObject>();

        //    for (int i = 0; i < vertices8.Length; i++)
        //    {
        //        GameObject instantiatedSprite8 = Instantiate(vertexSpriteZoom8, vertices8[i], Quaternion.identity);

        //        instantiatedSprite8.transform.parent = gameObject.transform;

        //        storeSprites8.Add(i, instantiatedSprite8);
        //    }

        //    gridCounterZoom8++;
        //}

        //int gridCounterZoom8 = 0;

        //void MoveGrid()
        //{
        //    int xPosition = -650;

        //    float moveOverValue = 0;
        //    float addToXAxis = 0;

        //    if (currentValue == 1)
        //    {
        //        moveOverValue = 120;
        //    }

        //    if (currentValue == 2)
        //    {
        //        addToXAxis = 120;
        //        moveOverValue = 240;
        //    }
        //    else if (currentValue == 3)
        //    {
        //        addToXAxis = 360;
        //        moveOverValue = 480;
        //    }
        //    else if (currentValue == 4)
        //    {
        //        addToXAxis = 840;
        //        moveOverValue = 960;
        //    }

        //    for (int i = 0; i < storeSprites.Count; i++)
        //    {
        //        if (i > 0 & i % 4 == 0)
        //        {
        //            addToXAxis += moveOverValue;
        //        }

        //        storeSprites[i].transform.position = new Vector3(xPosition + addToXAxis, storeSprites[i].transform.position.y, storeSprites[i].transform.position.z);
        //    }
        //}

        //void MoveGridZoom2()
        //{
        //    int xPosition = -650;

        //    if (GridCounterZoom2 == 0 & zoomSlider.value > 1)
        //    {
        //        CreateGridZoom2();
        //        PlaceSprite2();
        //    }

        //    float moveOverValue = 0;
        //    float addToXAxis = 0;

        //    if (GridCounterZoom2 >= 1 & currentValue == 1)
        //    {
        //        for (int i = 0; i < storeSprites2.Count; i++)
        //        {
        //            storeSprites2[i].SetActive(false);
        //        }
        //    }
        //    else if (currentValue > 1)
        //    {
        //        for (int i = 0; i < storeSprites2.Count; i++)
        //        {
        //            storeSprites2[i].SetActive(true);
        //        }
        //    }

        //    if (currentValue > 1)
        //    {
        //        if (currentValue == 2)
        //        {
        //            addToXAxis = 0;
        //            moveOverValue = 240;
        //        }
        //        else if (currentValue == 3)
        //        {
        //            addToXAxis = 120;
        //            moveOverValue = 480;
        //        }
        //        else if (currentValue == 4)
        //        {
        //            addToXAxis = 360;
        //            moveOverValue = 960;
        //        }

        //        for (int i = 0; i < storeSprites2.Count; i++)
        //        {
        //            if (i > 0 & i % 4 == 0)
        //            {
        //                addToXAxis += moveOverValue;
        //            }

        //            storeSprites2[i].transform.position = new Vector3(xPosition + addToXAxis, storeSprites2[i].transform.position.y, storeSprites2[i].transform.position.z);
        //        }
        //    }
        //}

        //void MoveGridZoom4()
        //{
        //    int xPosition = -650;

        //    int moveOverValue = 0;
        //    int addToXAxis = 0;

        //    if (gridCounterZoom4 == 0 & currentValue >= 3)
        //    {
        //        CreateGridZoom4();
        //        PlaceSprite4();
        //    }

        //    if (gridCounterZoom4 >= 1 & currentValue < 3)
        //    {
        //        for (int i = 0; i < storeSprites4.Count; i++)
        //        {
        //            storeSprites4[i].SetActive(false);
        //        }
        //    }
        //    else if (currentValue >= 3)
        //    {
        //        for (int i = 0; i < storeSprites4.Count; i++)
        //        {
        //            storeSprites4[i].SetActive(true);
        //        }
        //    }

        //    if ((currentValue == 3) || (currentValue == 4))
        //    {
        //        if (currentValue == 3)
        //        {
        //            moveOverValue = 240;
        //        }
        //        else if (currentValue == 4)
        //        {
        //            addToXAxis = 120;
        //            moveOverValue = 480;
        //        }

        //        for (int i = 0; i < storeSprites4.Count; i++)
        //        {
        //            if (i > 0 & i % 4 == 0)
        //            {
        //                addToXAxis += moveOverValue;
        //            }

        //            storeSprites4[i].transform.position = new Vector3(xPosition + addToXAxis, storeSprites4[i].transform.position.y, storeSprites4[i].transform.position.z);
        //        }
        //    }
        //}

        //void MoveGridZoom8()
        //{
        //    if (gridCounterZoom8 == 0 & currentValue == 4)
        //    {
        //        CreateGridZoom8();
        //        PlaceSprite8();
        //    }

        //    if (gridCounterZoom8 >= 1 & currentValue < 4)
        //    {
        //        for (int i = 0; i < storeSprites8.Count; i++)
        //        {
        //            storeSprites8[i].SetActive(false);
        //        }
        //    }
        //    else if (currentValue == 4)
        //    {
        //        for (int i = 0; i < storeSprites8.Count; i++)
        //        {
        //            storeSprites8[i].SetActive(true);
        //        }
        //    }

        //    int xPosition = -650;

        //    int moveOverValue = 0;
        //    int addToXAxis = 0;

        //    if (currentValue == 4)
        //    {
        //        moveOverValue = 240;

        //        for (int i = 0; i < storeSprites8.Count; i++)
        //        {
        //            if (i > 0 & i % 4 == 0)
        //            {
        //                addToXAxis += moveOverValue;
        //            }

        //            storeSprites8[i].transform.position = new Vector3(xPosition + addToXAxis, storeSprites8[i].transform.position.y, storeSprites8[i].transform.position.z);
        //        }
        //    }
        //}
    }
}