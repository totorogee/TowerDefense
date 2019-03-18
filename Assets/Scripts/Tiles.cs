using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitTilePath
{
    public bool IsTeamT;
    public UnitPathingMode UnitPath;
    public Vector3 Starting;
}

public class Tiles : MonoBehaviour
{

    public static int TotalX = 13;
    public static int TotalY = 13;
    public static float TileSpacing = 0.05f;
    public static List<Tiles> AllTiles = new List<Tiles>();
    public static Vector2 TileSize = Vector2.one;

    [SerializeField] protected GameObject Body;
    [SerializeField] protected List<UnitTilePath> PresetPath = new List<UnitTilePath>();

    public int MyID = 0;
    public bool IsRoad = false;
    public Dictionary<Directions, Tiles> AdjTiles = new Dictionary<Directions, Tiles>();

    private void OnEnable()
    {
        Bounds tilesBounds = Body.GetComponent<Renderer>().bounds;
        TileSize = new Vector2(tilesBounds.size.x, tilesBounds.size.z);
    }

    public void SetTile(bool isRoad)
    {
        IsRoad = isRoad;
        transform.localScale = transform.localScale * (1f - TileSpacing);
        MyID = AllTiles.Count - 1;
        Body.SetActive(!IsRoad);

        StartCoroutine( SetAdjTiles());
    }

    public Vector3 Offset(Units unit)
    {
        UnitDatas unitDatas = unit.MyData;

        //TODO
        Vector3 result = transform.position;

        Vector3 forward = (unitDatas.IsTeamT == 1 ? -gameObject.transform.forward : gameObject.transform.forward);
        Vector3 roadOffset = (unitDatas.Direction == 1 || unitDatas.Direction == 8) ? gameObject.transform.right : -gameObject.transform.right;


        result += (roadOffset * MainController.RoadSize * TileSize.x);
        result += (forward * (unitDatas.Phase + MainController.TimePhase) * TileSize.x);

        return result;
    }

    public Tiles NextTiles(Units unit)
    {
        UnitDatas unitDatas = unit.MyData;

        return null;
    }

    public static Tiles GetTile(Vector2Int pos)
    {
        return AllTiles[pos.x + pos.y * TotalX];
    }

    private IEnumerator SetAdjTiles()
    {
        yield return new WaitForEndOfFrame();

        if (!IsRoad)
        {
            yield break;
        }

        if (MyID + 13 < AllTiles.Count && AllTiles[MyID + 13].IsRoad)
        {
            AdjTiles.Add(Directions.W, AllTiles[MyID + 13]);
        }

        if (MyID - 13 > 0 && AllTiles[MyID - 13].IsRoad)
        {
            AdjTiles.Add(Directions.S, AllTiles[MyID - 13]);
        }

        if (MyID - 1 > 0 && AllTiles[MyID - 1].IsRoad)
        {
            AdjTiles.Add(Directions.A, AllTiles[MyID - 1]);
        }

        if (MyID + 1 < AllTiles.Count && AllTiles[MyID + 1].IsRoad)
        {
            AdjTiles.Add(Directions.D, AllTiles[MyID + 1]);
        }
    }
}
