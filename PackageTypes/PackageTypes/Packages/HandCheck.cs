using System;

namespace PackageTypes.Packages
{
    [Serializable]
    public class HandCheck: IPackage
    {
        public long Timestamp { get; set; }

        public HandCheck(long timestamp)
        {
            Timestamp = timestamp;
        }

        public PackageTypes GetPackageType() => PackageTypes.HandCheckMessage;
    }
}