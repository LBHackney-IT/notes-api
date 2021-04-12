using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NotesApi.Tests
{
    public class InMemoryTraceListener : TraceListener
    {
        private readonly List<string> _traces = new List<string>();

        public void Reset()
        {
            _traces.Clear();
        }

        public bool ContainsTrace(string msg)
        {
            return _traces.Any(x => x.Contains(msg));
        }

        public override void Write(string message)
        {
            _traces.Add(message);
        }

        public override void WriteLine(string message)
        {
            _traces.Add(message);
        }
    }
}
