using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTPLib.Util
{
    public static class Equality
    {
        public static bool SequenceEqualWithNullSupport<TSequence>(IEnumerable<TSequence> first, IEnumerable<TSequence> second)
        {
            if (first == null && second == null)
                return true;

            if (first == null || second == null)
                return false;

            return first.SequenceEqual(second);
        }
    }
}
