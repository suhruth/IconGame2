﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Question : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private LineRenderer lr;
    private Button btn_Que;
    public GameObject dragObject;
    public Answer correctAnswer;
    private Answer selectedAnswer;
    public Answer Selected { get { return selectedAnswer; } set { selectedAnswer = value; } }
    private int score;
    public int Score { get { return score; } set { score = value; } }
    public Canvas myCanvas;

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
       // selectedAnswer = correctAnswer;
        if (isDragging)
        {
            if (lr != null && dragObject != null)
            {
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
                if (Vector3.Distance(gameObject.transform.position, selectedAnswer.gameObject.transform.position) > .1f)
                {
                    lr.SetPosition(0, gameObject.transform.position);
                    lr.SetPosition(1, selectedAnswer.gameObject.transform.position);
                }
            }
        }

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
        if (que != activeQue)
        {
            if (activeQue != null)
                activeQue.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            activeQue = que;

            if (activeQue != null)
                activeQue.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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

                if (activeAns != null)
                {
                   // activeAns.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    activeQue.Selected = activeAns;

                    activeQue.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
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
