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

        public static void PostAndGet(string url, WWWForm form, Action<LeaderboardData> successHandler = null, Action<string, string> exceptionHandler = null)
        {
            NetworkRequestSignal requestSignal = new NetworkRequestSignal()
            {
                RequestType = RequestType.PostAndGet,
                URL = url,
                OnSuccessHandler = (value) =>
                {
                    string tempStr = value;
                    
                    int startVal = tempStr.IndexOf("[") + 2;
                    int endVal = tempStr.IndexOf("]") - 1;
                    if (startVal < endVal)
                    {
                        string subStr = value.Substring(startVal, endVal - startVal);
                        string[] items = subStr.Split('}');
                        List<LeaderboardItem> lbs = new List<LeaderboardItem>();
                        if (items.Length > 0)
                        {
                            lbs.Clear();
                            for (int i = 0; i < items.Length; i++)
                            {
                                string[] fields = items[i].Split(',');
                                LeaderboardItem lbItem = new LeaderboardItem();
                                for (int j = 0; j < fields.Length; j++)
                                {
                                    string testStr2 = fields[j].Replace("\"", "");
                                    testStr2 = testStr2.Replace("{", "");
                                    string[] vals = testStr2.Split(':');
                                    if (vals.Length > 1)
                                    {
                                        if (vals[0].Equals("id"))
                                            lbItem.ID = vals[1];
                                        if (vals[0].Equals("username"))
                                            lbItem.Username = vals[1];
                                        if (vals[0].Equals("companyname"))
                                            lbItem.CompanyName = vals[1];
                                        if (vals[0].Equals("score"))
                                            lbItem.Score = vals[1];

                                    }
                                }
                                lbs.Add(lbItem);
                            }
                            LeaderboardData lbData = new LeaderboardData();
                            lbData.Items = lbs;
                            successHandler?.Invoke(lbData);
                        }
                        else
                        {
                            exceptionHandler?.Invoke("Failed deserialization", "");
                        }
                    }
                    else
                    {
                        exceptionHandler?.Invoke("Empty Leaderboard Data received", "");
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


        public static void UpdateSignInCredentials(string userName, string email, Action<SignInData> successHandler, Action<string, string> exceptionHandler)
        {
            string apiStr = "verifyuser&username={0}&email={1}";
            WWWForm form = new WWWForm();
            NetworkRequestSignal requestSignal = new NetworkRequestSignal()
            {
                RequestType = RequestType.PostAndGet,
                URL = string.Format(apiStr, userName, email),
                OnSuccessHandler = (value) =>
                {
                    string tempStr = value;
                    if (tempStr.Contains(""))
                    {
                        string[] fields = tempStr.Split(',');
                        SignInData sdData = new SignInData();
                        for (int j = 0; j < fields.Length; j++)
                        {
                            string testStr2 = fields[j].Replace("\"", "");
                            testStr2 = testStr2.Replace("{", "");
                            testStr2 = testStr2.Replace("}", "");
                            string[] vals = testStr2.Split(':');
                            if (vals.Length > 1)
                            {
                                if (vals[0].Equals("status"))
                                    sdData.Status = vals[1] == "true" ? true : false;
                                if (vals[0].Equals("message"))
                                    sdData.Message = vals[1];
                            }
                        }
                        successHandler?.Invoke(sdData);
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

        // Add to Contacts Group
        public static void UpdateUserScore(string userName, string companyName, string score, Action successHandler, Action<string, string> exceptionHandler)
        {
            string apiStr = "saveuser&username={0}&companyname={1}&score={2}";
            WWWForm form = new WWWForm();
            Post(string.Format(apiStr, userName, companyName, score), form, successHandler, exceptionHandler);
        }

        public static void GetLeaderBoardData(Action<LeaderboardData> successHandler, Action<string, string> exceptionHandler)
        {
            string apiStr = "leaderboard";
            WWWForm form = new WWWForm();
            PostAndGet(apiStr, form, successHandler, exceptionHandler);
        }
    }
}
