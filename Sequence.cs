using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jera
{
    internal class Sequence
    {
        private string sequence = "";
        private List<Int32> keySequence = new List<Int32>();

        public Sequence() { }

        public string GetInputSequence() => sequence;

        public List<Int32> GetKeySequence() => keySequence;

        public string ResetInputSequence()
        {
            sequence = "";
            keySequence.Clear();
            return sequence;
        }

        public string AddToSequence(string input, Int32 code)
        {
            if (sequence.Length < 3)
            {
                sequence += input;
            }
            else
            {
                sequence = sequence.Substring(1, sequence.Length - 1) + input;
            }

            if (keySequence.Count < 4) {
                keySequence.Add(code);
            }
            else
            {
                keySequence.RemoveAt(0);
                keySequence.Add(code);
            }
            return sequence;
        }

        public string RemoveFromSequence()
        {
            if (sequence.Length == 0) {
                return sequence;
            }
            sequence = sequence.Substring(0, sequence.Length - 1);
            return sequence;
        }
    }
}
