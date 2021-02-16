using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class LeaderboardItem
{
    [JsonProperty("id")]
    public string ID { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("companyname")]
    public string CompanyName { get; set; }

    [JsonProperty("score")]
    public string Score { get; set; }
}
public class LeaderboardData
{
        [JsonProperty("list")]
    public IList<LeaderBoardItem> Items { get; set; }
}
