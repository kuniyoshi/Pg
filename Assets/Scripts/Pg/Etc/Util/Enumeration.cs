#nullable enable
using System;

namespace Pg.Etc.Util
{
    public class Enumeration
        : IComparable, IEquatable<Enumeration>
    {
        public static bool operator ==(Enumeration? a, Enumeration? b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            return a?.Equals(b) ?? false;
        }

        public static bool operator !=(Enumeration? a, Enumeration? b)
        {
            return !(a == b);
        }

        public int Id { get; }
        public string Name { get; }

        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((Enumeration) obj).Id);
        }

        public override bool Equals(object? obj)
        {
            return obj is Enumeration other && Equals(other);
        }

        public bool Equals(Enumeration? other)
        {
            if (other is null)
            {
                return false;
            }

            var isTypeSame = other.GetType() == GetType();
            var isIdSame = other.Id == Id;

            return isTypeSame && isIdSame;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
