/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;

namespace ECS
{
    public class Family : IEquatable<Family>
    {
        readonly Type[] _allComponents;
        readonly Type[] _oneComponents;
        readonly Type[] _noneComponents;

        internal Family(Type[] all, Type[] one, Type[] none)
        {
            _allComponents = all;
            _oneComponents = one;
            _noneComponents = none;
        }

        public bool Matches(Entity entity)
        {
            foreach (Type component in _allComponents)
            {
                if (!entity.HasComponent(component))
                {
                    return false;
                }
            }

            if (_oneComponents.Length > 0)
            {
                bool hasOne = false;
                foreach (Type component in _oneComponents)
                {
                    if (entity.HasComponent(component))
                    {
                        hasOne = true;
                        break;
                    }
                }

                if (!hasOne)
                {
                    return false;
                }
            }

            foreach (Type component in _noneComponents)
            {
                if (entity.HasComponent(component))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Family other = obj as Family;
            if (other == null)
            {
                return false;
            }

            return Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                foreach (Type compType in _allComponents)
                {
                    hash = hash * 31 + compType.GetHashCode();
                }

                foreach (Type compType in _oneComponents)
                {
                    hash = hash * 31 + compType.GetHashCode();
                }

                foreach (Type compType in _noneComponents)
                {
                    hash = hash * 31 + compType.GetHashCode();
                }

                return hash;
            }
        }

        public bool Equals(Family other)
        {
            return (_allComponents.IsEquivalent(other._allComponents)
                    && _oneComponents.IsEquivalent(other._oneComponents)
                    && _noneComponents.IsEquivalent(other._noneComponents));
        }

        public override string ToString()
        {
            string[] allComps = new string[_allComponents.Length];
            for (int i = 0; i < _allComponents.Length; i++)
            {
                allComps[i] = _allComponents[i].ToString();
            }

            string[] oneComps = new string[_oneComponents.Length];
            for (int i = 0; i < _oneComponents.Length; i++)
            {
                oneComps[i] = _oneComponents[i].ToString();
            }

            string[] noneComps = new string[_noneComponents.Length];
            for (int i = 0; i < _noneComponents.Length; i++)
            {
                noneComps[i] = _noneComponents[i].ToString();
            }

            return string.Format("[Family]\nAll: {0}\nOne: {1}\nNone: {2}",
                                 string.Join(", ", allComps),
                                 string.Join(", ", oneComps),
                                 string.Join(", ", noneComps));
        }

        public static FamilyBuilder All(params Type[] components)
        {
            return new FamilyBuilder().All(components);
        }

        public static FamilyBuilder One(params Type[] components)
        {
            return new FamilyBuilder().One(components);
        }

        public static FamilyBuilder Exclude(params Type[] components)
        {
            return new FamilyBuilder().Exclude(components);
        }
    }

    public class FamilyBuilder
    {

        internal FamilyBuilder()
        {
        }

        List<Type> _allComponents = new List<Type>();
        List<Type> _oneComponents = new List<Type>();
        List<Type> _noneComponents = new List<Type>();

        public FamilyBuilder All(params Type[] components)
        {
            _allComponents.AddRange(components);
            return this;
        }

        public FamilyBuilder One(params Type[] components)
        {
            _oneComponents.AddRange(components);
            return this;
        }

        public FamilyBuilder Exclude(params Type[] components)
        {
            _noneComponents.AddRange(components);
            return this;
        }

        public Family Get()
        {
            return new Family(_allComponents.ToArray(),
                              _oneComponents.ToArray(),
                              _noneComponents.ToArray());
        }
    }
}
