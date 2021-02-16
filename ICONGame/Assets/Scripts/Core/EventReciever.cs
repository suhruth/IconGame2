using CustomEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventReciever : MonoBehaviour
{
    private void Awake()
    {
        EventManager.Listen<TestMessageEvent>(OnTestMessageEvent);
    }

    private void OnTestMessageEvent(IEventBase obj)
    {
        TestMessageEvent e =(TestMessageEvent) obj;
        Debug.Log(e.msg);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
