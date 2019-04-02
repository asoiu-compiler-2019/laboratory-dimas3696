﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage
{
    public sealed class SourceCode
    {
        Lazy<string[]> lines;
        string sourceCode;

        public string Contents => sourceCode;

        public string[] Lines => lines.Value;

        public char this[int index]
        {
            get {
                if (index > sourceCode.Length - 1 || index < 0)
                    return '\0';
                return sourceCode[index];
            }
        }

        private class Subset<T> : IEnumerable<T>
        {
            int end;
            IEnumerable<T> set;

            int start;

            private struct SubsetEnumerator : IEnumerator<T>
            {
                bool disposed;
                int index;
                Subset<T> subset;

                public T Current => subset.set.ElementAt(index);

                object IEnumerator.Current => subset.set.ElementAt(index);

                public SubsetEnumerator(Subset<T> subset)
                {
                    disposed = false;
                    index = subset.start - 1;
                    this.subset = subset;
                }

                public void Dispose()
                {
                    disposed = true;
                } 

                public bool MoveNext()
                {
                    if (disposed)
                        throw new ObjectDisposedException("SubsetEnumerator");
                    if (index == subset.end)
                        return false;
                    index++;
                    return true;
                }

                public void Reset()
                {
                    if (disposed)
                        throw new ObjectDisposedException("SubsetEnumerator");
                    index = subset.start;
                }
            }

            public Subset(IEnumerable<T> collection, int start, int end)
            {
                set = collection;
                this.start = start;
                this.end = end;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new SubsetEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new SubsetEnumerator(this);
            }
        }

        public SourceCode(string sourceCode)
        {
            this.sourceCode = sourceCode;
            lines = new Lazy<string[]>(() => this.sourceCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
        }

        public string GetLine(int line)
        {
            if (line < 1)
                throw new IndexOutOfRangeException($"{nameof(line)} mustn`t be less than 1!");
            if (line > Lines.Length)
                throw new IndexOutOfRangeException($"{line} not fount!");

            return Lines[line - 1];
        }

        public string[] GetLines(int start, int end)
        {
            if (end < start)
                throw new IndexOutOfRangeException("Cannot retrieve negative range!");
            if(start < 1)
                throw new IndexOutOfRangeException($"{nameof(start)} mustn`t be less than 1!");
            if (end > Lines.Length)
                throw new IndexOutOfRangeException($"Cannot retrieve more lines than exist in file!");

            return new Subset<string>(Lines, start - 1, end - 1).ToArray();
        }

        public string GetSpan(SourceSpan span)
        {
            int start = span.Start.Index;
            int length = span.Length;
            return sourceCode.Substring(start, length);
        }

        //Add later........................................................
        //public string GetSpan(SyntaxNode node)
        //{
        //    return GetSpan(node.Span);
        //}

    }
}
