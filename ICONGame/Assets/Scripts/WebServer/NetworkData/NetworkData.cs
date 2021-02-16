using Newtonsoft.Json;

namespace WebServer.Data
{
  [System.Serializable]
  public class NetworkData
  {
    [JsonProperty("status")]
    public bool Status;

    [JsonProperty("message")]
    public string Message;

    [JsonProperty("code")]
    public string Code;

    [JsonProperty("data")]
    public object Data;
  }
}
