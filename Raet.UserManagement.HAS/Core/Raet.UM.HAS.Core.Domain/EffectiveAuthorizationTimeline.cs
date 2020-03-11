using Raet.UM.HAS.Core.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Raet.UM.HAS.Core.Domain
{
    public class EffectiveAuthorizationTimeline
    {
        private IList<DateTime> StartDates;

        private IList<DateTime> EndDates;

        public EffectiveAuthorization EffectiveAuthorization { get; }
        
        public EffectiveAuthorizationTimeline(EffectiveAuthorization effectiveAuthorization)
        {
            EffectiveAuthorization = effectiveAuthorization;
            StartDates = new List<DateTime>();
            EndDates = new List<DateTime>();
        }

        public void AddPermissionStart(DateTime from)
        {
            StartDates.Add(from);
        }

        public void AddPermissionEnd(DateTime from)
        {
            EndDates.Add(from);
        }

        public IEnumerable<Interval> CalculateEffectiveIntervals()
        {
            var intervals = new List<Interval>();

            var startDatesQueue = new Queue<DateTime>(StartDates.Distinct().OrderBy(x => x));
            var endDatesQueue = new Queue<DateTime>(EndDates.Distinct().OrderBy(x => x));

            var start = startDatesQueue.GetNextDateStartingFrom(DateTime.MinValue);
            if (!start.HasValue)
                return intervals;

            while (start.HasValue)
            {
                var end = endDatesQueue.GetNextDateStartingFrom(start.Value);
                if (start != end)
                {
                    intervals.Add(new Interval(start.Value, end));
                }
                start = end.HasValue ? startDatesQueue.GetNextDateStartingFrom(end.Value) : null;
            }

            return intervals;
        }
    }
}