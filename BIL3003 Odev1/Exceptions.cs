using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIL3003_Odev1
{
    class FileNameException : Exception
    {
        public FileNameException(string message) : base(message) { }
    }
    class ValueRangeException : Exception
    {
        public ValueRangeException(string message) : base(message) { }
    }
    class SelectionException : Exception
    {
        public SelectionException(string message) : base(message) { }
    }
}
