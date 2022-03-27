using System;

namespace PackageTypes.Packages
{
    [Serializable]
    public class Ping
    {
        public string Data { get; set; }

        public Ping(string data)
        {
            Data = data;
        }
    }
}