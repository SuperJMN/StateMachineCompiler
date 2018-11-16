using System;
using System.Linq;

namespace Compiler
{
    public class Fsm
    {
        public Header[] Headers { get; }
        public Transition[] Logic { get; }

        public Fsm(Header[] headers, Transition[] logic)
        {
            Headers = headers;
            Logic = logic;
        }

        public override string ToString()
        {
            var headers = string.Join(Environment.NewLine, Headers.Select(x => x.ToString()));
            var logic = string.Join(Environment.NewLine, Logic.Select(x => x.ToString()));
            return string.Join(Environment.NewLine, headers, logic);
        }
    }
}