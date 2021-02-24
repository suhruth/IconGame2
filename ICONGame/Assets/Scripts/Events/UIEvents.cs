using System.Collections.Generic;

namespace CustomEvent
{
    //public class SimpleEvent : IEventBase { }

    // initialize 
    // state change event
    // Siging screens and data
    #region APPLICATION_EVENTS
    #endregion // APPLICATION_EVENTS



    #region FROM_UI
    public class SubmitSignInCredentialsEvent : IEventBase { public string userName; public string email; }
    public class SubmitUserCredentialsEvent : IEventBase { public string userName; public string companyName; }
    public class MainMenuUIEvent : IEventBase { }
    public class PlayGameUIEvent : IEventBase { }
    public class InstructionsUIEvent : IEventBase { }
    public class LeaderBoardUIEvent : IEventBase { }
    public class PrizesUIEvent : IEventBase { }
    public class QuitUIEvent : IEventBase { }

    public class SubmitLayerScoreEvent : IEventBase { public int layerID; public int correctAns; public int timeTaken; public int BonusScore; public int score; }
    public class GetPlayerScoreEvent : IEventBase { }

    public class GetLeaderboardEvent : IEventBase { }

    #endregion // FROM_UI

    #region MAINMENU_EVENTS_TO_UI

    public class ShowUIScreenEvent : IEventBase { public UIScreenType _screen; }

    public class LayerScore
    {
        public int correctAns = 0;
        public int TimeTaken = 0;
        public int bonusPoints = 0;
        public int TotalPoints = 0;
    }

    public class SetPlayerScoreEvent : IEventBase { public List<LayerScore> layers = new List<LayerScore>(); public int totalScore; }

    public class SetLeaderboardEvent : IEventBase { public LeaderboardData leaderboard = new LeaderboardData(); }

    public class SignInStatusEvent : IEventBase { public bool status; public string Msg; }

    #endregion // MAINMENU_EVENTS_TO_UI


    #region MISC_EVENTS
    #endregion

}