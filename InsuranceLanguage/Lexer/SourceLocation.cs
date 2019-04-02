using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage
{
    public struct SourceLocation : IEquatable<SourceLocation>
    {
        readonly int column;
        readonly int line;
        readonly int index;

        public int Column => column;
        public int Line => line;
        public int Index => index;

        public SourceLocation(int index, int line, int column)
        {
            this.index = index;
            this.column = column;
            this.line = line;
        }

        public static bool operator !=(SourceLocation l, SourceLocation r)
        {
            return !l.Equals(r);
        }

        public static bool operator ==(SourceLocation l, SourceLocation r)
        {
            return l.Equals(r);
        }

        public override bool Equals(object obj)
        {
            if(obj is SourceLocation)
            {
                return Equals((SourceLocation)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(SourceLocation other)
        {
            return other.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return 0xB1679EE ^ Index ^ Line ^ Column;
        }
    }
}
