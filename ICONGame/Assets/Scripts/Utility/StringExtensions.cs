using Newtonsoft.Json;
using UnityEngine;

public static class StringExtensions
{

  public static bool TryDeserialize<T>(this string content, out T value)
  {
    if (string.IsNullOrEmpty(content) || content.Equals("null"))
    {
      value = default;
      return false;
    }


    try
    {
      value = JsonConvert.DeserializeObject<T>(content);
      return true;
    }
    catch (System.Exception ex)
    {
      Debug.LogException(ex);
      value = default;
      return false;
    }
  }
}
