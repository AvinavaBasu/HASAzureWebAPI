using System;
using System.Collections.Generic;
using System.Text;

namespace Raet.UM.HAS.Core.Domain.Extensions
{
    internal static class QueueExtensions
    {
        /// <summary>
        /// Returns first date in the queue after starting date or null as default
        /// </summary>
        public static DateTime? GetNextDateStartingFrom(this Queue<DateTime> queue, DateTime startingDate)
        {
            var next = queue.GetNextDate();
            while (next.HasValue && next.Value < startingDate)
            {
                next = queue.GetNextDate();
            }

            return next;
        }
        
        /// <summary>
        ///  Returns first date in the queue or null as default
        /// </summary>
        private static DateTime? GetNextDate(this Queue<DateTime> queue)
        {
            try
            {
                return queue.Dequeue();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
