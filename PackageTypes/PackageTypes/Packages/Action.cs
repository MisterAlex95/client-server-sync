using System;

namespace PackageTypes.Packages
{
    [Serializable]
    public class ActionMessage: IPackage
    {
        public long Timestamp { get; set; }

        public ActionMessage(long timestamp)
        {
            Timestamp = timestamp;
        }

        public PackageTypes GetPackageType() => PackageTypes.ActionMessage;
    }
    
    [Serializable]
    public class ActionAnswer: IPackage
    {
        public long Timestamp { get; set; }
        public ActionAnswer(long timestamp)
        {
            Timestamp = timestamp;
        }

        public PackageTypes GetPackageType() => PackageTypes.ActionAnswer;
    }
}