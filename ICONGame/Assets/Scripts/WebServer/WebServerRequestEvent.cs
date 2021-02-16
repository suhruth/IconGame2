using System;
using UnityEngine;

namespace WebServer.Event
{
    public struct NetworkRequestSignal
    {
        public string URL;
        public string Data;
        public WWWForm Form;
        public RequestType RequestType;
        public Action<string> OnSuccessHandler;
        public Action<string,string> OnExceptionHandler;
    }
}
