using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum Directions { S = 1, A = 2, D = 4, W = 8 }


public class MainController : MonoBehaviour
{
    public static float RoadSize = 0.25f;

    [SerializeField] private GameObject tilesPrefabs, towerPrefabs;
    [SerializeField] private GameObject groundPlane;

    [SerializeField] private TextAsset heightMap;
    private List<int> allMap = new List<int>();

    [SerializeField] private TextAsset towerMap;
    private int towerMapX, towerMapY;
    private List<int> allTowerMap = new List<int>();

    [SerializeField] private TextAsset unitDataJson;
    private List<UnitDatas> allUnitDatas = new List<UnitDatas>();
    [SerializeField] private GameObject unitsPrefabs;
    [SerializeField] private GameObject unitsContainor;

    public List<Units> AllUnits = new List<Units>();
     
    public static float TimePhase
    {
        get
        {
            return totalTime % 1;
        }
        set
        {
            totalTime = value;
        }
    }

    private static float totalTime = 0;
    public int TurnsID = 0;


    // Start is called before the first frame update
    void Start()
    {

        ReadMap();
        ReadTowerMap();
        SetupFloors();

        ReadUnitData();
        SetUnits();
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnsID != Mathf.FloorToInt(totalTime))
        {
            UpdateTurns();
            TurnsID = Mathf.FloorToInt(totalTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            MovePhase();
        }
    }

    private void ReadUnitData()
    {
        allUnitDatas = JsonConvert.DeserializeObject<List<UnitDatas>>(unitDataJson.text);
    }

    private void SetUnits()
    {
        for (int i = 0; i < allUnitDatas.Count; i++)
        {
            GameObject go = Instantiate(unitsPrefabs, unitsContainor.transform);
            go.GetComponent<Units>().SetUnit(allUnitDatas[i]);
            AllUnits.Add(go.GetComponent<Units>());
        }
    }

    private void SetupFloors()
    {
        // Count one side and excluded center row
        int x = (int)((Tiles.TotalX - 1) / 2);
        int y = (int)((Tiles.TotalY - 1) / 2);

        // Total number of row is 2n + 1
        for (int j = -y; j <= y; j++)
        {
            for (int i = -x; i <= x; i++)
            {
                GameObject go = Instantiate(tilesPrefabs, groundPlane.transform);
                Vector3 offset = new Vector3(i * Tiles.TileSize.x, 0f, j * Tiles.TileSize.y);
                go.transform.position = groundPlane.transform.position + offset;
                Tiles.AllTiles.Add(go.GetComponent<Tiles>());

                GameObject tower = Instantiate(towerPrefabs, go.transform);
                tower.transform.position = go.transform.position;

                if (i + x >= Tiles.TotalX || j + y >= Tiles.TotalY)
                {
                    Debug.LogError("Map size not match");
                    continue;
                }
                else
                {
                    go.GetComponent<Tiles>().SetTile((allMap[(y - j) * Tiles.TotalX + (i + x)] == 0));
                    tower.GetComponent<Tower>().SetTower(allTowerMap[(y - j) * Tiles.TotalX + (i + x)]);
                }
            }
        }
    }

    private void ReadMap()
    {
        if (heightMap == null)
        {
            return;
        }
        string map = heightMap.text;
        string[] all = map.Split(',', '\n');

        for (int i = 0; i < all.Length; i++)
        {
            allMap.Add(Int32.Parse(all[i]));
        }

        if (allMap.Count != Tiles.TotalX * Tiles.TotalY)
        {
            Debug.LogError("Map Size Not Match");
        }
    }

    private void ReadTowerMap()
    {
        if (towerMap == null)
        {
            return;
        }
        string map = towerMap.text;
        string[] all = map.Split(',', '\n');

        for (int i = 0; i < all.Length; i++)
        {
            allTowerMap.Add(Int32.Parse(all[i]));
        }

        if (allTowerMap.Count != Tiles.TotalX * Tiles.TotalY)
        {
            Debug.LogError("Map Size Not Match");
        }
    }

    private void MovePhase()
    {
        foreach (var item in AllUnits)
        {
            item.Move();
        }
        totalTime += 0.02f;
    }

    private void UpdateTurns()
    {
        foreach (var item in AllUnits)
        {
            item.UpdateTurn();
        }
    }
}
