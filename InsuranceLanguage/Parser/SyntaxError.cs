using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace InsuranceLanguage.Parser
{
    [Serializable]
    internal class SyntaxError : Exception
    {
        public SyntaxError() { }
        public SyntaxError(string mes) : base(mes) { }
        public SyntaxError(string mes, Exception inner) : base(mes, inner) { }
        protected SyntaxError(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
