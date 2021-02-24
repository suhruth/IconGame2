using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MEC;
using WebServer.Data;
using MiniJSON;

namespace WebServer
{
    public enum NetworkType
    {
        Online = 0,
        Offline,
    }

    public enum RequestType
    {
        Get,
        PostAndGet,
        Post,
        Put,
        Delete,
    }
    public interface IServer
    {
        void SetBaseURL(string baseURL);
        void SetHeaders(string key, string value);
        void Get(string url, Action<string> successHandler, Action<string,string> exceptionHandler);
        void PostAndGet(string url, string data, Action<string> successHandler, Action<string, string> exceptionHandler);
        void PostAndGet(string url, WWWForm form, Action<string> successHandler, Action<string, string> exceptionHandler);
        void Post(string url, WWWForm form, Action successHandler, Action<string, string> exceptionHandler);
        void Post(string url, string data, Action successHandler, Action<string, string> exceptionHandler);
        void Put(string url, string data, Action successHandler, Action<string, string> exceptionHandler);
        void Delete(string url, Action successHandler, Action<string, string> exceptionHandler);
    }
    public class RemoteServer : IServer
    {
        private string baseURL;
        private Dictionary<string, string> headers;
        public void SetBaseURL(string baseURL)
        {
            this.baseURL = baseURL;
          //  SetHeaders("token", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiNjlmYmQ0MDMwOTQ1NDBlYTIzZjc2MzY3OWNiODk0MTIyZjExNDQwY2E2N2NkZWE2OGQ3NDgxNzM3YWUwMmNlZmYzOWE5MTg2ZWE0YzU3YjgiLCJpYXQiOjE2MTI0ODcxNjgsIm5iZiI6MTYxMjQ4NzE2OCwiZXhwIjoxNjQ0MDIzMTY4LCJzdWIiOiI1Iiwic2NvcGVzIjpbXX0.Djui_LSnuhPCRPcknXv8lsJuhY29dRy0CGiheQ6kFbPt1WOteuvb5RXV8k27VNDD_mM8PHFoR3l4NgLz4WMn08enYZ0CaIb4-zVmAY9ijXYP5n-HBhXR4eTctYAiXAr6JFENHWOMSCcBij-Lv6nN0x6EjHD7qMOzxK89VOJfaWP6dFdMMRkr8hHMAC_uH3_3pwtGF901OJ1htkc1e2VG3uVWYk2DKPgx3wNPHnwnFeQRthlkh80VDcJ-icOiLZa2Cq1Ot52Hh0eG4dOGrXo32kBB_ilbfGOdRXbSGGyZxmDorRdF_FBRpZSGiD5UQl1Y7lLCfR1Shog3svTQbRAkUHKaCjZsQFy7c5lX6KH7W2ZzUyw3CmPjyZ5CM-FDXO-_xF_W8rOAFausM8YIyv-MYOFA1QlRPkBjbvRf-zg9-3h1LrQjXZpmoPmYK2lgms38MznLbIa5TqpSwRe6c8_-5oBBFh7oHFgM-gnuXAC-5n6M4zylu_U5XIwgEcqrH3ppP1n2bc2-gaYcnaEmi1AT9632qNFWg0yhec1B73CdEJdcqio3fzeknaHpaMy0xbc1cIwRwsAnG0HqRCNcyQ9DgZYLUsxnExPIfaQbb4_KB9prkQ1vkPDhupDqMitW3KqEA1001Eb0wbvyNnqMuoXcvwwTfGuGw8ENbhVROm4BV1c");
        }
        public void SetHeaders(string key, string value)
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>(1);
            }

            if (!headers.ContainsKey(key))
            {
                headers.Add(key, value);
            }
        }
        void IServer.Get(string url, Action<string> successHandler, Action<string, string> exceptionHandler)
        {
            Timing.RunCoroutine(GetEnumerator(baseURL + url, successHandler, exceptionHandler));
        }
        void IServer.Post(string url, WWWForm form, Action successHandler, Action<string, string> exceptionHandler)
        {
            Timing.RunCoroutine(PostEnumerator(baseURL + url, form, successHandler, exceptionHandler));
        }
        void IServer.Post(string url, string data, Action successHandler, Action<string, string> exceptionHandler)
        {
            Timing.RunCoroutine(PostEnumerator(baseURL + url, data, successHandler, exceptionHandler));
        }
        void IServer.PostAndGet(string url, string data, Action<string> successHandler, Action<string, string> exceptionHandler)
        {
            Timing.RunCoroutine(PostAndGetEnumerator(baseURL + url, data, successHandler, exceptionHandler));
        }
        void IServer.PostAndGet(string url, WWWForm form, Action<string> successHandler, Action<string, string> exceptionHandler)
        {
            Timing.RunCoroutine(PostAndGetEnumerator(baseURL + url, form, successHandler, exceptionHandler));
        }
        void IServer.Put(string url, string data, Action successHandler, Action<string, string> exceptionHandler)
        {
            Timing.RunCoroutine(PutEnumerator(baseURL + url, data, successHandler, exceptionHandler));
        }
        void IServer.Delete(string url, Action successHandler, Action<string, string> exceptionHandler)
        {
            Timing.RunCoroutine(DeleteEnumerator(baseURL + url, successHandler, exceptionHandler));
        }
        private IEnumerator<float> PostAndGetEnumerator(string url, string data, Action<string> successHandler, Action<string,string> exceptionHandler)
        {
            Debug.Log(string.Format("[REQUEST] URL : {0}, DATA : {1}", url, data));

            UnityWebRequest request = UnityWebRequest.Post(url, data);
            UIManager.SetDebugText(1, url);
        //     request.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            request.SendWebRequest();

            while (!request.isDone)
            {
                yield return 0;
            }

            if (request.isHttpError || request.isNetworkError)
            {
                exceptionHandler?.Invoke(request.error, "");
                Debug.LogError(request.error);
            }
            else
            {
                OnSuccessfullyFetch(successHandler, exceptionHandler, request);
            }
        }
        private IEnumerator<float> PostAndGetEnumerator(string url, WWWForm form, Action<string> successHandler, Action<string, string> exceptionHandler)
        {
            //Debug.Log(string.Format("[REQUEST] URL : {0}, DATA : {1}", url, form.));

            UnityWebRequest request = UnityWebRequest.Post(url, form);
            UIManager.SetDebugText(1, url);
         //   request.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            Debug.Log(url);

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    Debug.Log(header.Key + " " + header.Value);
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            request.SendWebRequest();

            while (!request.isDone)
            {
                yield return 0;
            }

            if (request.isHttpError || request.isNetworkError)
            {
                exceptionHandler?.Invoke(request.error, "");
                Debug.LogError(request.error);
            }
            else
            {
                OnSuccessfullyFetch(successHandler, exceptionHandler, request);
            }
        }
        private IEnumerator<float> PostEnumerator(string url, WWWForm form, Action successHandler, Action<string, string> exceptionHandler)
        {
            UnityWebRequest request = UnityWebRequest.Post(url, form);
            UIManager.SetDebugText(1, url);
           //request.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            request.SendWebRequest();

            while (!request.isDone)
            {
                yield return 0;
            }

            if (request.isHttpError || request.isNetworkError)
            {
                exceptionHandler?.Invoke(request.error, "");
                Debug.LogError(request.error);
            }
            else
            {
                successHandler?.Invoke();
            }
        }
        private IEnumerator<float> PostEnumerator(string url, string data, Action successHandler, Action<string, string> exceptionHandler)
        {
            UnityWebRequest request = UnityWebRequest.Post(url, data);
            UIManager.SetDebugText(1, url);
           // request.certificateHandler = new AcceptAllCertificatesSignedWithASpecificKeyPublicKey();
            request.SendWebRequest();

            while (!request.isDone)
            {
                yield return 0;
            }

            if (request.isHttpError || request.isNetworkError)
            {
                exceptionHandler?.Invoke(request.error,"");
                Debug.LogError(request.error);
            }
            else
            {
                successHandler?.Invoke();
            }
        }
        private IEnumerator<float> GetEnumerator(string url, Action<string> successHandler, Action<string, string> exceptionHandler)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            UIManager.SetDebugText(1, url);
            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    //Debug.Log(header.Key + " " + header.Value);
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            request.SendWebRequest();

            while (!request.isDone)
            {
                yield return 0;
            }

            if (request.isHttpError || request.isNetworkError)
            {
                exceptionHandler?.Invoke(request.error, "");
                Debug.LogError(request.error);
            }
            else
            {
                OnSuccessfullyFetch(successHandler, exceptionHandler, request);
            }
        }
        private IEnumerator<float> PutEnumerator(string url, string data, Action successHandler, Action<string, string> exceptionHandler)
        {
            UnityWebRequest request = UnityWebRequest.Put(url, data);
            UIManager.SetDebugText(1, url);
            Debug.Log(url);

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    Debug.Log(header.Key + " " + header.Value);
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            request.SendWebRequest();

            while (!request.isDone)
            {
                yield return 0;
            }

            if (request.isHttpError || request.isNetworkError)
            {
                exceptionHandler?.Invoke(request.error, "");
                Debug.LogError(request.error);
            }
            else
            {
                successHandler?.Invoke();
            }
        }
        private IEnumerator<float> DeleteEnumerator(string url, Action successHandler, Action<string, string> exceptionHandler)
        {
            UnityWebRequest request = UnityWebRequest.Delete(url);
            Debug.Log(url);

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    Debug.Log(header.Key + " " + header.Value);
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            request.SendWebRequest();

            while (!request.isDone)
            {
                yield return 0;
            }

            if (request.isHttpError || request.isNetworkError)
            {
                exceptionHandler?.Invoke(request.error, "");
                Debug.LogError(request.error);
            }
            else
            {
                successHandler?.Invoke();
            }
        }
        private void OnSuccessfullyFetch(Action<string> successHandler, Action<string, string> exceptionHandler, UnityWebRequest request)
        {
            //Debug.Log(request.downloadHandler.text);

            //if (request.downloadHandler.text.TryDeserialize<NetworkData>(out var networkData))
            //{
            //    if (networkData.Status)
            //    {
            //        successHandler?.Invoke(networkData.Data.ToJson());
            //    }
            //    else
            //    {
            //        exceptionHandler?.Invoke(networkData.Message, "");
            //    }
            //}
            //Debug.LogError("data ===== " + request.downloadHandler.text);
            //UIManager.SetDebugText(request.downloadHandler.text);

            string tempStr = request.downloadHandler.text;

            if(tempStr.Contains("status"))
            {
                UIManager.SetDebugText(0, request.downloadHandler.text);
                successHandler?.Invoke(request.downloadHandler.text);
            }
            else
            {
                int startVal = tempStr.IndexOf("[") + 2;
                int endVal = tempStr.IndexOf("]") - 1;
                if (startVal < endVal)
                {
                    UIManager.SetDebugText(0, request.downloadHandler.text);
                    successHandler?.Invoke(request.downloadHandler.text);
                }
                else
                {
                    exceptionHandler?.Invoke("Failed Fetching data", "");
                }
            }
           

            //Json.Deserialize(request.downloadHandler.text);
            //var dict = Json.Deserialize(request.downloadHandler.text) as Dictionary<String, System.Object>;
            //var arrayList = new List< System.Object > ();
            //arrayList = ((dict["list"]) as List< System.Object >);

            //UIManager.SetDebugText(arrayList[0].ToString());
            //Debug.Log("array[0]: " + arrayList[0]);
            //if (request.downloadHandler.text.TryDeserialize<NetworkData2>(out var networkData))
            //{
            //    if (networkData.type.Length > 0)
            //    {
            //        successHandler?.Invoke(request.downloadHandler.text);
            //    }
            //    else
            //    {
            //        exceptionHandler?.Invoke("Failed Fetching data", "");
            //    }
            //}
            //else
            //{
            //    exceptionHandler?.Invoke("Unable to deserialze the data", "Invalid");
            //}
        }
    }
}
