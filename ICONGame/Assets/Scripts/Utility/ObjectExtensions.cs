using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static class ObjectExtensions
{
  public static byte[] Serialize(this object _object)
  {
    byte[] bytes;
    using (var _MemoryStream = new MemoryStream())
    {
      IFormatter _BinaryFormatter = new BinaryFormatter();
      _BinaryFormatter.Serialize(_MemoryStream, _object);

            //  bytes = new byte[_MemoryStream.Length];
            //  _MemoryStream.Read(bytes, 0, bytes.Length);
            //return bytes;
            return _MemoryStream.ToArray();
    }
  }

  public static async void SerializeAsync(this object _object, Action<byte[]> onSerialize)
  {
    Task<byte[]> task = Task.Run(() => _object.Serialize());
    await task;
    onSerialize?.Invoke(task.Result);
  }

  public static async void SerializeAsync(this object _object, ISurrogateSelector surrogateSelector, Action<byte[]> onSerialize)
  {
    Task<byte[]> task = Task.Run(() => _object.Serialize(surrogateSelector));
    await task;
    onSerialize?.Invoke(task.Result);
  }

  public static byte[] Serialize(this object _object, ISurrogateSelector surrogateSelector)
  {
    byte[] bytes;
    using (var _MemoryStream = new MemoryStream())
    {
      IFormatter _BinaryFormatter = new BinaryFormatter
      {
        SurrogateSelector = surrogateSelector
      };
      _BinaryFormatter.Serialize(_MemoryStream, _object);

      bytes = new byte[_MemoryStream.Length];
      _MemoryStream.Read(bytes, 0, bytes.Length);
    }
    return bytes;
  }

  public static T Deserialize<T>(this byte[] _byteArray)
  {
    T ReturnValue;
    using (var _MemoryStream = new MemoryStream(_byteArray))
    {
      IFormatter _BinaryFormatter = new BinaryFormatter();
      ReturnValue = (T)_BinaryFormatter.Deserialize(_MemoryStream);
    }
    return ReturnValue;
  }

  public static T Deserialize<T>(this byte[] _byteArray, ISurrogateSelector surrogateSelector)
  {
    T ReturnValue;
    using (var _MemoryStream = new MemoryStream(_byteArray))
    {
      IFormatter _BinaryFormatter = new BinaryFormatter
      {
        SurrogateSelector = surrogateSelector
      };
      ReturnValue = (T)_BinaryFormatter.Deserialize(_MemoryStream);
    }
    return ReturnValue;
  }

  public static async void DeserializeAsync<T>(this byte[] _byteArray, ISurrogateSelector surrogateSelector, Action<T> onDeserialize)
  {
    Task<T> task = Task.Run(() => _byteArray.Deserialize<T>(surrogateSelector));
    await task;
    onDeserialize?.Invoke(task.Result);
  }

  public static async void DeserializeAsync<T>(this byte[] _byteArray, Action<T> onDeserialize)
  {
    Task<T> task = Task.Run(() => _byteArray.Deserialize<T>());
    await task;
    onDeserialize?.Invoke(task.Result);
  }

  public static string ToJson(this object _object)
  {
    return JsonConvert.SerializeObject(_object);
  }
}