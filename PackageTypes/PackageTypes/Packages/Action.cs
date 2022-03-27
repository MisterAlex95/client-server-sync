using System;

namespace PackageTypes.Packages
{
    [Serializable]
    public class Action: IPackage
    {
        public long Timestamp { get; set; }

        public Action(long timestamp)
        {
            Timestamp = timestamp;
        }

        public PackageTypes GetPackageType() => PackageTypes.ActionMessage;
    }
}