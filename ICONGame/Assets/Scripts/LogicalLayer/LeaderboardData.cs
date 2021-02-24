using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class SignInData
{
    [JsonProperty("status")]
    public bool Status { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

}
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

    [JsonProperty("rank")]
    public int Rank { get; set; }
}
public class LeaderboardData
{
        [JsonProperty("list")]
    public List<LeaderboardItem> Items { get; set; }
}
