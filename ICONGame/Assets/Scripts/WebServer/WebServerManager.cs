using System;
using System.Collections.Generic;
using UnityEngine;
using WebServer;
using WebServer.Event;

namespace FRAMEWORK.WebServer
{
    public static class WebServerManager
    {
        #region CORE_FUNCTIONS
        public static IServer server { get { return AppManager.webServer; } }

        private static void OnRequestEvent(NetworkRequestSignal requestSignal)
        {
            switch (requestSignal.RequestType)
            {
                case RequestType.Get:
                    {
                        server.Get(requestSignal.URL, requestSignal.OnSuccessHandler, requestSignal.OnExceptionHandler);
                    }
                    break;
                case RequestType.PostAndGet:
                    {
                        if (requestSignal.Form != null)
                            server.PostAndGet(requestSignal.URL, requestSignal.Form, requestSignal.OnSuccessHandler, requestSignal.OnExceptionHandler);
                        else
                            server.PostAndGet(requestSignal.URL, requestSignal.Data, requestSignal.OnSuccessHandler, requestSignal.OnExceptionHandler);
                    }
                    break;
                case RequestType.Post:
                    {
                        if (requestSignal.Form != null)
                            server.Post(requestSignal.URL, requestSignal.Form, () => requestSignal.OnSuccessHandler?.Invoke(""), requestSignal.OnExceptionHandler);
                        else
                            server.Post(requestSignal.URL, requestSignal.Data, () => requestSignal.OnSuccessHandler?.Invoke(""), requestSignal.OnExceptionHandler);
                    }
                    break;
                case RequestType.Put:
                    {
                        server.Put(requestSignal.URL, requestSignal.Data, () => requestSignal.OnSuccessHandler?.Invoke(""), requestSignal.OnExceptionHandler);
                    }
                    break;
                case RequestType.Delete:
                    {
                        server.Delete(requestSignal.URL, () => requestSignal.OnSuccessHandler?.Invoke(""), requestSignal.OnExceptionHandler);
                    }
                    break;
            }
        }

        public static void Get<T>(string url, Action<T> successHandler, Action<string, string> exceptionHandler)
        {
            NetworkRequestSignal requestSignal = new NetworkRequestSignal()
            {
                RequestType = RequestType.Get,
                URL = url,
                OnSuccessHandler = (value) =>
                {
                    if (value.TryDeserialize(out T response))
                    {
                        successHandler?.Invoke(response);
                    }
                    else
                    {
                        exceptionHandler?.Invoke("Failed deserialization", "");
                    }
                },
                OnExceptionHandler = exceptionHandler
            };
            OnRequestEvent(requestSignal);
        }

        public static void Get<T>(string url, WWWForm form, Action<T> successHandler, Action<string, string> exceptionHandler)
        {
            NetworkRequestSignal requestSignal = new NetworkRequestSignal()
            {
                RequestType = RequestType.Get,
                URL = url,
                OnSuccessHandler = (value) =>
                {
                    if (value.TryDeserialize(out T response))
                    {
                        successHandler?.Invoke(response);
                    }
                    else
                    {
                        exceptionHandler?.Invoke("Failed deserialization", "");
                    }
                },
                OnExceptionHandler = exceptionHandler,
                Form = form
            };
            OnRequestEvent(requestSignal);
        }

        //public static void PostAndGet<T>(string url, string data, Action<T> successHandler, Action<string,string> exceptionHandler)
        //{
        //    NetworkRequestSignal requestSignal = new NetworkRequestSignal()
        //    {
        //        RequestType = RequestType.PostAndGet,
        //        URL = url,
        //        OnSuccessHandler = (value) =>
        //        {
        //            if (value.TryDeserialize(out T response))
        //            {
        //                successHandler?.Invoke(response);
        //            }
        //            else
        //            {
        //                exceptionHandler?.Invoke("Failed deserialization","");
        //            }
        //        },
        //        OnExceptionHandler = exceptionHandler,
        //        Data = data
        //    };
        //    OnRequestEvent(requestSignal);
        //}

        public static void PostAndGet<T>(string url, WWWForm form, Action<T> successHandler = null, Action<string, string> exceptionHandler = null)
        {
            NetworkRequestSignal requestSignal = new NetworkRequestSignal()
            {
                RequestType = RequestType.PostAndGet,
                URL = url,
                OnSuccessHandler = (value) =>
                {
                    if (value.TryDeserialize(out T response))
                    {
                        successHandler?.Invoke(response);
                    }
                    else
                    {
                        exceptionHandler?.Invoke("Failed deserialization", "");
                    }
                },
                OnExceptionHandler = exceptionHandler,
                Form = form
            };
            OnRequestEvent(requestSignal);
        }

        public static void Post(string url, WWWForm form, Action successHandler, Action<string, string> exceptionHandler)
        {
            NetworkRequestSignal requestSignal = new NetworkRequestSignal()
            {
                RequestType = RequestType.Post,
                URL = url,
                OnSuccessHandler = (value) =>
                {
                    successHandler?.Invoke();
                },
                OnExceptionHandler = exceptionHandler,
                Form = form
            };
            OnRequestEvent(requestSignal);
        }

        public static void Post(string url, string data, Action successHandler, Action<string, string> exceptionHandler)
        {
            NetworkRequestSignal requestSignal = new NetworkRequestSignal()
            {
                RequestType = RequestType.Post,
                URL = url,
                OnSuccessHandler = (value) =>
                {
                    successHandler?.Invoke();
                },
                OnExceptionHandler = exceptionHandler,
                Data = data
            };
            OnRequestEvent(requestSignal);
        }

        public static void Put(string url, string data, Action successHandler, Action<string, string> exceptionHandler)
        {
            NetworkRequestSignal requestSignal = new NetworkRequestSignal()
            {
                RequestType = RequestType.Put,
                URL = url,
                OnSuccessHandler = (value) =>
                {
                    successHandler?.Invoke();
                },
                OnExceptionHandler = exceptionHandler,
                Data = data
            };
            OnRequestEvent(requestSignal);
        }

        public static void Delete(string url, Action successHandler, Action<string, string> exceptionHandler)
        {
            NetworkRequestSignal requestSignal = new NetworkRequestSignal()
            {
                RequestType = RequestType.Put,
                URL = url,
                OnSuccessHandler = (value) =>
                {
                    successHandler?.Invoke();
                },
                OnExceptionHandler = exceptionHandler
            };
            OnRequestEvent(requestSignal);
        }
        #endregion //CORE_FUNCTIONS


        // Add to Contacts Group
        public static void UpdateUserScore(string userName, string companyName,string score, Action successHandler, Action<string, string> exceptionHandler)
        {
            string apiStr = "saveuser&username={0}&companyname={1}&score={2}";
            WWWForm form = new WWWForm();
            Post(string.Format(apiStr, userName,companyName,score), form, successHandler, exceptionHandler);
        }

        public static void GetLeaderBoardData(Action<LeaderboardData> successHandler, Action<string, string> exceptionHandler)
        {
            string apiStr = "leaderboard";
            WWWForm form = new WWWForm();
            PostAndGet(apiStr, form, successHandler, exceptionHandler);
        }
    }
}
