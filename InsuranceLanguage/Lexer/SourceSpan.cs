using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage
{
    public struct SourceSpan : IEquatable<SourceSpan>
    {
        readonly SourceLocation end;
        readonly SourceLocation start;

        public SourceLocation End => end;
        public SourceLocation Start => start;

        public int Length => end.Index - start.Index;

        public SourceSpan(SourceLocation start, SourceLocation end)
        {
            this.end = end;
            this.start = start;
        }

        public static bool operator !=(SourceSpan l, SourceSpan r)
        {
            return !l.Equals(r);
        }

        public static bool operator ==(SourceSpan l, SourceSpan r)
        {
            return l.Equals(r);
        }

        public override bool Equals(object obj)
        {
            if(obj is SourceSpan)
            {
                return Equals((SourceSpan)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(SourceSpan other)
        {
            return other.Start == Start && other.End == End;
        }

        public override int GetHashCode()
        {
            return 0x509CE ^ Start.GetHashCode() ^ End.GetHashCode();
        }

        public override string ToString()
        {
            return $"{start.Line} {start.Column} {Length}";
        }
    }
}
