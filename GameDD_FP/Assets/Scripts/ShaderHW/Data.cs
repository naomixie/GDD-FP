using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data instance;

    public float tile_width;
    public float tile_length;
    public float tile_height;


    [Header("鼠标拾取有效距离")]
    public float mouseDistance = 1.0f;

    [Header("Map Properties")]
    public int MapSize = 50;
    private float MapHeight;
    public GameObject MapTile;

    [Header("Grass Properties")]
    public GameObject GrassPlane;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void GenerateMap()
    {
        MapHeight = -tile_height / 2;
        for (int i = -MapSize; i < MapSize; ++i)
        {
            for (int j = -MapSize; j < MapSize; ++j)
            {
                Instantiate(MapTile, new Vector3(i + 0.5f, MapHeight, j + 0.5f), Quaternion.identity);
            }
        }

        for (int i = -MapSize / 10; i < MapSize / 10; ++i)
        {
            for (int j = -MapSize / 10; j < MapSize / 10; ++j)
            {
                Instantiate(GrassPlane, new Vector3(i * 10 + 5, 0, j * 10 + 5), Quaternion.identity);
            }
        }
    }

}
