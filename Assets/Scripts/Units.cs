using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitPathingMode { Straight, Right, Left }

public class Units : MonoBehaviour
{
    [SerializeField] protected Transform levelOne;
    [SerializeField] protected Transform levelTwo;

    [SerializeField] protected MeshRenderer levelOneColorArea;
    [SerializeField] protected MeshRenderer levelTwoColorArea;

    [SerializeField] protected Color teamTColor;
    [SerializeField] protected Color teamBColor;

    public UnitDatas MyData;
    public List<Tiles> MyTiles = new List<Tiles>();
    public UnitPathingMode MyPathingMode = UnitPathingMode.Right;
    public Directions MyDirections = Directions.A;

    public void SetUnit(UnitDatas unitDatas)
    {
        MyData = unitDatas;
        MyTiles.Add(Tiles.GetTile(new Vector2Int(MyData.XPos, MyData.YPos)));

        if (MyData.Type < 0 || MyData.Type > 2)
        {
            return;
        }

        if (MyData.IsTeamT < 0 || MyData.IsTeamT > 1)
        {
            return;
        }

        levelOneColorArea.material.color = MyData.IsTeamT == 1 ? teamTColor : teamBColor;
        levelTwoColorArea.material.color = MyData.IsTeamT == 1 ? teamTColor : teamBColor;
        levelOne.gameObject.SetActive(MyData.Type == 1);
        levelTwo.gameObject.SetActive(MyData.Type == 2);

      //  gameObject.transform.localEulerAngles = new Vector3(0, Extensions.DirectionToDegree(MyData.Direction), 0);
        gameObject.transform.localPosition = MyTiles[0].Offset(this);

    }

    public void Move()
    {
        gameObject.transform.localPosition = MyTiles[0].Offset(this);
    }

    public void UpdateTurn()
    {
        MyTiles[0] = MyTiles[0].NextTiles(this);
    }
}
