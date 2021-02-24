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
        public Button btn_MainMenu, btn_QuitBtn;

        // Start is called before the first frame update
        void Start()
        {
            btn_MainMenu.onClick.AddListener(MainMenu_OnClick);
            btn_QuitBtn.onClick.AddListener(OnQuitBtn);


            if (itemPrefab == null)
                Debug.LogError("Invalid item prefab");

            if (content == null)
                Debug.LogError("Invalid scroll view content");

            EventManager.Listen<SetLeaderboardEvent>(OnLeaderboardEvent);
            EventManager.Raise<GetLeaderboardEvent>(new GetLeaderboardEvent());

        }

        public void OnEnable()
        {
            EventManager.Raise<GetLeaderboardEvent>(new GetLeaderboardEvent());
        }


        public void MainMenu_OnClick()
        {
            EventManager.Raise<MainMenuUIEvent>(new MainMenuUIEvent { });
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

                if (lb.leaderboard == null)
                {
                    Debug.LogError("Empty leaderboard");
                    return;
                }

                if (lb.leaderboard.Items == null)
                {
                    Debug.LogError("Empty leaderboard Items");
                    return;
                }

                //if (lb.leaderboard.Items.Count > 1)
                //{
                //    lb.leaderboard.Items.Sort((x, y) =>
                //    {
                //        return string.Compare(y.Score, x.Score);
                //    });
                //}

                for (int i = 0; i < lb.leaderboard.Items.Count; i++)
                {
                    GameObject newItem = Instantiate(itemPrefab, content.transform) as GameObject;
                    LeaderBoardItem lbi = newItem.GetComponent<LeaderBoardItem>();
                    if (lbi != null)
                    {
                        lbi.txt_Rank.text = (i + 1).ToString(); //lb.leaderboard.Items[i].Rank.ToString();
                        lbi.txt_Name.text = lb.leaderboard.Items[i].Username;
                        lbi.txt_Points.text = lb.leaderboard.Items[i].Score.ToString();
                       // lbi.transform.parent = content.transform;
                    }
                }
            }
        }

        public void OnQuitBtn()
        {
            Application.OpenURL("https://ciscosbsummit2021.marcomarabiamea.com/thank-you/");
        }

    }
}
