using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEvent;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject signInScreen;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject layerScreen_1;
    [SerializeField] private GameObject layerScreen_2;
    [SerializeField] private GameObject layerScreen_3;
    [SerializeField] private GameObject layerScreen_4;
    [SerializeField] private GameObject yourScoreScreen;
    [SerializeField] private GameObject leaderBoardScreen;
    [SerializeField] private GameObject prizesScreen;
    [SerializeField] private GameObject instructionsScreen;

    [SerializeField] private UIPopup popup;

    private Dictionary<UIScreenType,GameObject> allScreens = new Dictionary<UIScreenType, GameObject>();
    private UIScreenType currentScreen = UIScreenType.None;

    public UIScreenType StartingScreen = UIScreenType.SignIn;

    public Text txt_Debug1;
    public Text txt_Debug;

    [SerializeField] Texture2D CursorTexture, HandCursorTexture;

    public static UIManager uiManager;

    public static bool DisplayDebugInfo = true;

    public static void SetDebugText(int id, string str)
    {
        if (!DisplayDebugInfo)
        {
            if (uiManager != null && uiManager.txt_Debug1 != null)
                uiManager.txt_Debug1.gameObject.SetActive(false);
            if (uiManager != null && uiManager.txt_Debug != null)
                uiManager.txt_Debug.gameObject.SetActive(false);
            return;
        }
        if (id == 1)
        {
            if (uiManager != null && uiManager.txt_Debug1 != null)
                uiManager.txt_Debug1.text = str;
        }
        else if (uiManager != null && uiManager.txt_Debug != null)
            uiManager.txt_Debug.text = str;
    }

    private void Awake()
    {
        Cursor.SetCursor(CursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
    // Start is called before the first frame update
    void Start()
    {
        uiManager = this;

        allScreens.Clear();

        allScreens.Add(UIScreenType.SignIn, signInScreen);
        allScreens.Add(UIScreenType.MainMenu, mainMenuScreen);
        allScreens.Add(UIScreenType.Layer1, layerScreen_1);
        allScreens.Add(UIScreenType.Layer2, layerScreen_2);
        allScreens.Add(UIScreenType.Layer3, layerScreen_3);
        allScreens.Add(UIScreenType.Layer4, layerScreen_4);
        allScreens.Add(UIScreenType.YourScore, yourScoreScreen);
        allScreens.Add(UIScreenType.LeaderBoard, leaderBoardScreen);
        allScreens.Add(UIScreenType.Prizes, prizesScreen);
        allScreens.Add(UIScreenType.Instructions, instructionsScreen);

        foreach (KeyValuePair<UIScreenType, GameObject> screen in allScreens)
        {
            if (screen.Value != null)
                screen.Value.SetActive(screen.Key == StartingScreen);
        }

        ShowScreen(UIScreenType.SignIn);

        EventManager.Listen<ShowUIScreenEvent>(SetShowScreenEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowScreen(UIScreenType _screen)
    {
        if (currentScreen != _screen)
        {
            if (!allScreens.ContainsKey(_screen))
            {
                Debug.LogError("UI Screen missing  :: " + _screen.ToString());
                return;
            }
            if (allScreens.ContainsKey(currentScreen))
                allScreens[currentScreen].SetActive(false);

            currentScreen = _screen;

            allScreens[currentScreen].SetActive(true);
        }
    }

    public void SetShowScreenEvent(IEventBase obj)
    {
        if (obj is ShowUIScreenEvent)
            ShowScreen(((ShowUIScreenEvent)obj)._screen);
    }

    public void OnMouseHoverOnCircle(bool isHover)
    {
      if(isHover)
        Cursor.SetCursor(HandCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
      else
        Cursor.SetCursor(CursorTexture, Vector2.zero, CursorMode.ForceSoftware);
    }
}
