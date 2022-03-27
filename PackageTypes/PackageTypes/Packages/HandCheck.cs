using System;

namespace PackageTypes.Packages
{
    [Serializable]
    public class HandCheck
    {
        public string Data { get; set; }

        public HandCheck(string data)
        {
            Data = data;
        }
    }
}