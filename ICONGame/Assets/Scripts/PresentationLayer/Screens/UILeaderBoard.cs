using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomEvent;


namespace PRESENTATION
{
    public class UILeaderBoard : UIScreen
    {
        public GameObject content;
        public GameObject itemPrefab;
        // Start is called before the first frame update
        void Start()
        {
            if (itemPrefab == null)
                Debug.LogError("Invalid item prefab");

            if (content == null)
                Debug.LogError("Invalid scroll view content");

            EventManager.Listen<SetLeaderboardEvent>(OnLeaderboardEvent);
            EventManager.Raise<GetPlayerScoreEvent>(new GetPlayerScoreEvent());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnLeaderboardEvent(IEventBase obj)
        {
            if (content == null || itemPrefab == null) return;

            if (content != null)
            {
                LeaderBoardItem[] lbis= content.GetComponentsInChildren<LeaderBoardItem>();
                for (int i = 0; i < lbis.Length; i++)
                    GameObject.Destroy(lbis[i].gameObject);
            }

            if (obj is SetLeaderboardEvent)
            {
                SetLeaderboardEvent lb = (SetLeaderboardEvent)obj;
                for (int i = 0; i < lb.leaderboard.Count; i++)
                {
                    GameObject newItem = Instantiate(itemPrefab, content.transform) as GameObject;
                    LeaderBoardItem lbi = newItem.GetComponent<LeaderBoardItem>();
                    if (lbi != null)
                    {
                        lbi.txt_Rank.text = lb.leaderboard[i].rank.ToString();
                        lbi.txt_Name.text = lb.leaderboard[i].name;
                        lbi.txt_Points.text = lb.leaderboard[i].points.ToString();
                    }
                }
            }
        }

    }
}
