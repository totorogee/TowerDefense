using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] protected Transform levelOne;
    [SerializeField] protected Transform levelTwo;

    [SerializeField] protected MeshRenderer levelOneColorArea;
    [SerializeField] protected MeshRenderer levelTwoColorArea;

    [SerializeField] protected Color teamTColor;
    [SerializeField] protected Color teamBColor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetTower(int ID)
    {
        int level = Mathf.Abs(ID);
        bool isTeamT = (ID >= 0);
        SetTower(level, isTeamT);
    }


    public void SetTower(int level, bool isTeamT)
    {
        if (level < 0 || level > 2)
        {
            return;
        }

        levelOneColorArea.material.color = isTeamT ? teamTColor : teamBColor;
        levelTwoColorArea.material.color = isTeamT ? teamTColor : teamBColor;
        levelOne.gameObject.SetActive(level == 1);
        levelTwo.gameObject.SetActive(level == 2);
    }
}
