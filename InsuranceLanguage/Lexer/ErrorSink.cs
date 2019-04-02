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
        private List<ErrorEntry> _errors;
        public IEnumerable<ErrorEntry> Errors => _errors.AsReadOnly();
        public bool HasErrors => _errors.Count > 0;

        public ErrorSink()
        {
            _errors = new List<ErrorEntry>();
        }

        public void AddError(string mes, SourceCode sourceCode, Severity severity, SourceSpan sourceSpan)
        {
            _errors.Add(new ErrorEntry(mes, sourceCode.GetLines(sourceSpan.Start.Line, sourceSpan.End.Line), severity, sourceSpan));
        }

        public void Clear()
        {
            _errors.Clear();
        }

        public IEnumerator<ErrorEntry> GetEnumerator()
        {
            return _errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _errors.GetEnumerator();
        }
    }
}
