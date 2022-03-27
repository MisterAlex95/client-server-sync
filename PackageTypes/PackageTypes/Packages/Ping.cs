using System;

namespace PackageTypes.Packages
{
    [Serializable]
    public class PingMessage: IPackage
    {
        public string Data { get; set; }

        public PingMessage(string data)
        {
            Data = data;
        }

        public PackageTypes GetPackageType() => PackageTypes.PingMessage;
    }
    
    [Serializable]
    public class PingAnswer: IPackage
    {
        public string Data { get; set; }

        public PingAnswer(string data)
        {
            Data = data;
        }

        public PackageTypes GetPackageType() => PackageTypes.PingAnswer;
    }
}