using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEvent;
using WebServer;
using WebServer.Data;
using FRAMEWORK.WebServer;
using System;

public class AppManager : MonoBehaviour
{
    string userName;
    string companyName;

    private List<LayerScore> scores = new List<LayerScore>();

    public int NoOfLayers = 4;

    private static IServer server;
    public static IServer webServer { get { if (server == null) server = new RemoteServer(); return server; } }

    public string BaseURL = "http://localhost:9906?a=";

    // Start is called before the first frame update
    void Start()
    {
        scores.Clear();

        for (int i = 0; i < NoOfLayers; i++)
            scores.Add(new LayerScore());

        EventManager.Listen<SubmitUserCredentialsEvent>(OnSubmitUserCredentialsEvent);
        EventManager.Listen<MainMenuUIEvent>(OnMainMenuEvent);
        EventManager.Listen<PlayGameUIEvent>(OnPlayGameEvent);
        EventManager.Listen<InstructionsUIEvent>(OnInstructionsEvent);
        EventManager.Listen<LeaderBoardUIEvent>(OnLeaderboardEvent);
        EventManager.Listen<PrizesUIEvent>(OnPrizeEvent);
        EventManager.Listen<QuitUIEvent>(OnQuitEvent);
        EventManager.Listen<SubmitLayerScoreEvent>(OnSubmitLayerScoreEvent);
        EventManager.Listen<GetPlayerScoreEvent>(OnGetPlayerScoreEvent);
        EventManager.Listen<GetLeaderboardEvent>(OnGetLeaderboardEvent);

        webServer.SetBaseURL(BaseURL);
    }

    bool sendShowScreen = false;
    UIScreenType screen = UIScreenType.None;

    bool sendLeaderBoardData = false;
    bool leaderBoardDataReady = false;

    bool sendPlayerScoreData = false;
    // Update is called once per frame
    void Update()
    {
        if (sendShowScreen && screen != UIScreenType.None)
        {
            EventManager.Raise<ShowUIScreenEvent>(new ShowUIScreenEvent { _screen = screen });
            sendShowScreen = false;
        }

        if (sendPlayerScoreData)
        {
            OnSendPlayerScoreData();
            sendPlayerScoreData = false;
        }

        if (sendLeaderBoardData && leaderBoardDataReady)
        {
            OnSendLeaderBoardData();
            sendLeaderBoardData = false;
        }
    }

    private void OnSendPlayerScoreData()
    {
        SetPlayerScoreEvent playerScore = new SetPlayerScoreEvent();
        playerScore.layers = scores;
        playerScore.totalScore = 0;
        for (int i = 0; i < scores.Count; i++)
            playerScore.totalScore += scores[i].TotalPoints;
        EventManager.Raise<SetPlayerScoreEvent>(playerScore);
       // WebServerManager.UpdateUserScore(userName, companyName, playerScore.totalScore.ToString(), OnSendScoreToServerSuccessfully, OnSendScoreToServerFailed);
    }

    private void OnSendLeaderBoardData()
    {
        SetLeaderboardEvent leaderBoard = new SetLeaderboardEvent();
        // fill  the data once received from server.

        EventManager.Raise<SetLeaderboardEvent>(leaderBoard);
    }

    public void OnSubmitUserCredentialsEvent(IEventBase obj)
    {
        if (obj is SubmitUserCredentialsEvent)
        {
            SubmitUserCredentialsEvent userCred = (SubmitUserCredentialsEvent)obj;
            this.userName = userCred.userName;
            this.companyName = userCred.companyName;
            sendShowScreen = true;
            screen = UIScreenType.MainMenu;
        }
    }


    private void OnMainMenuEvent(IEventBase obj)
    {
        sendShowScreen = true;
        screen = UIScreenType.MainMenu;
    }

    public void OnPlayGameEvent(IEventBase obj)
    {
        sendShowScreen = true;
        screen = UIScreenType.Layer4;
    }
    public void OnInstructionsEvent(IEventBase obj)
    {
        sendShowScreen = true;
        screen = UIScreenType.Instructions;
    }
    public void OnLeaderboardEvent(IEventBase obj)
    {
        sendShowScreen = true;
        screen = UIScreenType.LeaderBoard;
    }
    public void OnPrizeEvent(IEventBase obj)
    {
        sendShowScreen = true;
        screen = UIScreenType.Prizes;
    }
    public void OnQuitEvent(IEventBase obj)
    {
        sendShowScreen = true;
        screen = UIScreenType.SignIn;
        Application.Quit();
    }
    public void OnSubmitLayerScoreEvent(IEventBase obj)
    {
        if (obj is SubmitLayerScoreEvent)
        {
            SubmitLayerScoreEvent ls = (SubmitLayerScoreEvent)obj;
            if (ls.layerID > 0 && ls.layerID <= NoOfLayers)
            {
                scores[ls.layerID - 1].correctAns = ls.correctAns;
                scores[ls.layerID - 1].bonusPoints = ls.BonusScore;
                scores[ls.layerID - 1].TotalPoints = ls.score;
                scores[ls.layerID - 1].TimeTaken = ls.timeTaken;

                screen = UIScreenType.Layer1;
                switch (ls.layerID)
                {
                    case 1: screen = UIScreenType.Layer2;
                        break;

                    case 2: screen = UIScreenType.Layer3;
                        break;

                    case 3: screen = UIScreenType.Layer4;
                        break;

                    case 4: screen = UIScreenType.YourScore;
                        break;
                }
                sendShowScreen = true;
            }
        }
    }

    public void OnGetPlayerScoreEvent(IEventBase obj)
    {
        sendPlayerScoreData = true;
    }

    public void OnGetLeaderboardEvent(IEventBase obj)
    {
        sendLeaderBoardData = true;
        leaderBoardDataReady = false;
       // WebServerManager.GetLeaderBoardData(OnGetLeaderboardDataSuccessfully, OnGetLeaderboardDataFailed);
    }

    #region NETWORKING

    private void OnSendScoreToServerFailed(string obj, string code)
    {
    }

    private void OnSendScoreToServerSuccessfully()
    {
        //EventManager.Raise<MyMeetupsDataEvent>(new MyMeetupsDataEvent { data = obj });
    }

    private void OnGetLeaderboardDataFailed(string obj, string code)
    {
        leaderBoardDataReady = true;
    }

    private void OnGetLeaderboardDataSuccessfully(LeaderboardData data)
    {
        leaderBoardDataReady = true;
        //EventManager.Raise<MyMeetupsDataEvent>(new MyMeetupsDataEvent { data = obj });
    }
    #endregion

}
