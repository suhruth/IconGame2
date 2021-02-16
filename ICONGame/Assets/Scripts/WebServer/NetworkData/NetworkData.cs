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

    public class NetworkData2
    {
        [JsonProperty("a")]
        public string type;

        [JsonProperty("list")]
        public object Data;

    }
}
