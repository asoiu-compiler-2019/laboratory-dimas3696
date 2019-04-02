using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage
{
    public struct SourceLocation : IEquatable<SourceLocation>
    {
        private readonly int _column;
        private readonly int _line;
        private readonly int _index;

        public int Column => _column;
        public int Line => _line;
        public int Index => _index;

        public SourceLocation(int index, int line, int column)
        {
            _index = index;
            _column = column;
            _line = line;
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
