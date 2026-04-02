using System;

namespace OgameGenSim.Utils;

public class ClientResult
{
private int statusCode;

    public int StatusCode
    {
        get { return statusCode; }
        set
        {
            statusCode = value;
            IsSuccessStatusCode = value - 299 <= 0;
        }
    }

    public string ErrorMessage { get; set; }
    public bool IsSuccessStatusCode { get; set; }

    public class ClientResultObject<O> : ClientResult
    {
        public O Result;
    }
    public class ClientResultObjectList<O> : ClientResult
    {
        public List<O> Result;
    }
}
