using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Parser
{
    public class ParserOptions
    {
        public bool AllowRootStatements { get; set; }
        public bool UseSemicolons { get; set; }

        public static readonly ParserOptions Default = new ParserOptions();
        public static readonly ParserOptions OptionalSemicolon = new ParserOptions { UseSemicolons = false };

        public ParserOptions()
        {
            AllowRootStatements = true;
            UseSemicolons = true;
        }
    }
}
