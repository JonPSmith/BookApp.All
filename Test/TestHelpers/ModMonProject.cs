// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Test.TestHelpers
{
    public enum ModMonTypes { Invalid, ThreePart, FourParts}

    public class ModMonProject
    {
        protected bool Equals(ModMonProject other)
        {
            return ProjectType == other.ProjectType 
                   && AppName == other.AppName 
                   && BContextName == other.BContextName 
                   && LayerName == other.LayerName 
                   && FeatureName == other.FeatureName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ModMonProject) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) ProjectType, AppName, BContextName, LayerName, FeatureName);
        }

        public ModMonProject(Assembly projectAssembly)
        {
            PropertyAssembly = projectAssembly;

            var split = projectAssembly.GetName().Name.Split('.');
            if (split.Length < 3 || split.Length > 4)
                return;

            ProjectType = ModMonTypes.ThreePart;
            AppName = split[0];
            BContextName = split[1];
            LayerName = split[2];
            if (split.Length != 4) 
                return;

            FeatureName = split[3];
            ProjectType = ModMonTypes.FourParts;
        }

        public ModMonTypes ProjectType { get; }

        public Assembly PropertyAssembly { get; }

        public string AppName { get; private set; }
        public string BContextName { get; private set; }
        public string LayerName { get; private set; }
        public string FeatureName { get; private set; }

        public override string ToString()
        {
            switch (ProjectType)
            {
                case ModMonTypes.Invalid:
                    return "- invalid project name";
                case ModMonTypes.ThreePart:
                    return $"{AppName}.{BContextName}.{LayerName}";
                case ModMonTypes.FourParts:
                    return $"{AppName}.{BContextName}.{LayerName}.{FeatureName}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}