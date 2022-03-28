using System;

namespace PackageTypes.Packages
{
    [Serializable]
    public class HandCheckMessage : IPackage
    {
        public long Timestamp { get; set; }

        public HandCheckMessage(long timestamp)
        {
            Timestamp = timestamp;
        }

        public PackageTypes GetPackageType() => PackageTypes.HandCheckMessage;
    }

    [Serializable]
    public class HandCheckAnswer : IPackage
    {
        public long Timestamp { get; set; }

        public HandCheckAnswer(long timestamp)
        {
            Timestamp = timestamp;
        }

        public PackageTypes GetPackageType() => PackageTypes.HandCheckAnswer;
    }
}