using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage
{
    public enum Severity
    {
        None,
        Message,
        Warning,
        Error,
        Fatal
    }

    public sealed class ErrorEntry
    {
        public string[] Lines { get; }
        public string Message { get; }
        public Severity Severity { get; }
        public SourceSpan Span { get; }

        public ErrorEntry(string mes, string[] lines, Severity severity, SourceSpan span)
        {
            Message = mes;
            Lines = lines;
            Severity = severity;
            Span = span;
        }

    }

    public class ErrorSink : IEnumerable<ErrorEntry>
    {
        List<ErrorEntry> errors;
        public IEnumerable<ErrorEntry> Errors => errors.AsReadOnly();
        public bool HasErrors => errors.Count > 0;

        public ErrorSink()
        {
            errors = new List<ErrorEntry>();
        }

        public void AddError(string mes, SourceCode sourceCode, Severity severity, SourceSpan sourceSpan)
        {
            errors.Add(new ErrorEntry(mes, sourceCode.GetLines(sourceSpan.Start.Line, sourceSpan.End.Line), severity, sourceSpan));
        }

        public void Clear()
        {
            errors.Clear();
        }

        public IEnumerator<ErrorEntry> GetEnumerator()
        {
            return errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return errors.GetEnumerator();
        }
    }
}
