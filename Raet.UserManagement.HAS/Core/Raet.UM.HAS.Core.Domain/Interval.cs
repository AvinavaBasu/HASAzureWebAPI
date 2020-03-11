using System;

namespace Raet.UM.HAS.Core.Domain
{
    public class Interval
    {
        public Interval(DateTime start, DateTime? end)
        {
            Start = start;
            End = end;
        }
        public DateTime Start { get; }
        public DateTime? End { get; }

        public bool IsClosed => End.HasValue;
    }
}