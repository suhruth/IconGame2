using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Question : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private LineRenderer lr;
    private Button btn_Que;
    public GameObject dragObject;
    public Answer correctAnswer;
    public Animator anim;

    private Answer selectedAnswer;
    public Answer Selected { get { return selectedAnswer; } set { selectedAnswer = value; } }
    private int score;
    public int Score { get { return score; } set { score = value; } }
    public Canvas myCanvas;


    float pulseTimer = 0.0f;
    public float PulseTime { set { pulseTimer = value; } }

    public void Clean()
    {
        selectedAnswer = null;
        if (lr != null)
            lr.positionCount = 0;
        if (anim != null)
        {
            anim.SetBool("PlayPulse", false);
            anim.Play("ScaleUPDown", -1, 0f);
            anim.speed = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        if (myCanvas == null)
            myCanvas = FindObjectOfType<Canvas>();

        btn_Que = GetComponent<Button>();
        if (btn_Que != null)
            btn_Que.onClick.AddListener(OnClick_Question);
    }

    bool isDragging = false;

    // Update is called once per frame
    void Update()
    {
        if (pulseTimer > 0.0f)
        {
            pulseTimer -= Time.deltaTime;
            if (pulseTimer < 0.0f)
            {
                anim.SetBool("PlayPulse", false);
                pulseTimer = 0.0f;
            }
        }
        // selectedAnswer = correctAnswer;
        if (isDragging)
        {
            if (lr != null && dragObject != null)
            {
                if (lr.positionCount == 0) lr.positionCount = 2;
                if (Vector3.Distance(gameObject.transform.position, dragObject.transform.position) > .1f)
                {
                    lr.SetPosition(0, gameObject.transform.position);
                    lr.SetPosition(1, dragObject.transform.position);
                }
            }
        }
        else
        {
            if (lr != null && selectedAnswer != null)
            {
                if (lr.positionCount == 0) lr.positionCount = 2;
                if (Vector3.Distance(gameObject.transform.position, selectedAnswer.gameObject.transform.position) > .1f)
                {
                    lr.SetPosition(0, gameObject.transform.position);
                    lr.SetPosition(1, selectedAnswer.gameObject.transform.position);
                }
            }
        }

    }

    public void DoPulseAnim()
    {
        anim.SetBool("PlayPulse", true);
        pulseTimer = 1.25f;
        anim.speed = 1;
    }

    Vector2 dragOrgPos = new Vector2();
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("On Drag Begin");
        //if (dragObject != null)
        //    dragOrgPos = dragObject.GetComponent<RectTransform>().anchoredPosition;
        ////    dragObject.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
    }
    Vector3 dragPosition = new Vector3();
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("On Dragging");
        //isDragging = true;
        //if (dragObject != null)
        //    dragObject.GetComponent<RectTransform>().anchoredPosition += eventData.delta * 7f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("On Drag End");
        //isDragging = false;
        //if (dragObject != null)
        //{
        //    dragObject.transform.position = transform.position;
        //    dragObject.GetComponent<RectTransform>().anchoredPosition = dragOrgPos;
        //}
    }

    public void OnClick_Question()
    {
        SetActiveQuestion(this);
    }

    public static Question activeQue = null;
    public static Answer activeAns = null;
    public static void SetActiveQuestion(Question que)
    {
        if (que != null && que.lr != null)
        {
            que.lr.positionCount = 0;
            que.Selected = null;
        }

        if (que != activeQue)
        {
            //if (activeQue != null)
            //    activeQue.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            que.DoPulseAnim();
            activeQue = que;
            activeQue.Selected = null;
            //if (activeQue != null)
            //    activeQue.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    public static void SetActiveAns(Answer ans)
    {
        if (ans != activeAns)
        {
            //if (activeAns != null)
            //    activeAns.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            if (activeQue != null)
            {
                activeAns = ans;
                activeQue.anim.SetBool("PlayPulse", false);

                if (activeAns != null)
                {
                   // activeAns.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    activeQue.Selected = activeAns;
                    activeAns.DoPulseAnim();

                    //activeQue.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    //activeAns.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    activeQue = null;
                    activeAns = null;
                }
            }
        }
    }

    public static void Clear()
    {
        if (activeQue != null)
        {
            activeQue.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            activeQue = null;
            activeAns = null;
        }
    }
}
