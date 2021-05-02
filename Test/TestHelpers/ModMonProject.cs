// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Test.TestHelpers
{
    public class ModMonProject
    {
        private readonly string _name;

        protected bool Equals(ModMonProject other)
        {
            return _name == other._name;
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
            return HashCode.Combine(_name);
        }

        public ModMonProject(Assembly projectAssembly)
        {
            PropertyAssembly = projectAssembly;
            _name = projectAssembly.GetName().Name;

            var split = projectAssembly.GetName().Name.Split('.');

            AppName = split[0];
            if (split.Length > 1)
                BContextName = split[1];
            if (split.Length > 2)
                LayerName = split[2];
        }

        public Assembly PropertyAssembly { get; }

        public string AppName { get; private set; }
        public string BContextName { get; private set; }
        public string LayerName { get; private set; }

        public override string ToString()
        {
            return _name;
        }
    }
}