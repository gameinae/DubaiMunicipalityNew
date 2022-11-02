using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    //cached
    public GameObject Member;

    private void Start()
    {
        StartCoroutine(Init());
    }
    IEnumerator Init() 
    {
        yield return new WaitForSeconds(1);
        CreateLeaderBoard();
    }
    void CreateLeaderBoard() 
    {
        var lm = GameManager.Instance.GridSystem.InvokeBoardMembers().OrderBy(m => m.Health).ToList();

        int lmCount = lm.Count - 1;
        for (int i =0; i <= lmCount; i++)
        {
            InitateLeaderBoard(lm[lmCount-i],i);
        }
    }
    public void InitateLeaderBoard(LeaderBoardMember leaderBoardMember,int index) 
    {
        GameObject m = Instantiate(Member,transform);
        m.transform.localPosition = new Vector3(0,0,0);
        LeaderBoardElement leaderBoardElement = m.GetComponent<LeaderBoardElement>();
        
        Sprite sp = null;
        if (leaderBoardMember.Health >= 80)
        {
            sp = GameManager.Instance.GridSystem.HealthLevel[0];
        }
        else if (leaderBoardMember.Health < 80 && leaderBoardMember.Health > 50)
        {
            sp = GameManager.Instance.GridSystem.HealthLevel[1];
        }
        else if (leaderBoardMember.Health <= 50)
        {
            sp = GameManager.Instance.GridSystem.HealthLevel[2];
        }
        leaderBoardElement.SetElementValues(leaderBoardMember.Name,leaderBoardMember.Health,index+1,sp);
    }
}
