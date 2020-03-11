using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.Core.Domain;
using System;
using System.Linq;

namespace Raet.UM.HAS.Core.Tests.Domain
{
    [TestClass]
    public class AuthorizationTimelineTests
    {
        [TestMethod]
        public void CalculateIntervals_0Starts0Ends_Returns0Intervals()
        {
            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(0, intervals.Count);
        }

        [TestMethod]
        public void CalculateIntervals_1Start0Ends_Returns1IntervalFromStartToEndOfTime()
        {
            var start = new DateTime(2018, 6, 1);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionStart(start);

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(start, intervals.First().Start);
            Assert.IsFalse(intervals.First().IsClosed);
            Assert.IsFalse(intervals.First().End.HasValue);
        }

        [TestMethod]
        public void CalculateIntervals_NStarts0Ends_Returns1IntervalFromOldestStartToEndOfTime()
        {
            var start = new DateTime(2018, 5, 28);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionStart(new DateTime(2018, 6, 1));
            eaTimeline.AddPermissionStart(new DateTime(2018, 6, 15));
            eaTimeline.AddPermissionStart(start);
            eaTimeline.AddPermissionStart(new DateTime(2018, 7, 2));

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(start, intervals.First().Start);
            Assert.IsFalse(intervals.First().IsClosed);
            Assert.IsFalse(intervals.First().End.HasValue);
        }

        [TestMethod]
        public void CalculateIntervals_0Starts1End_Returns0Intervals()
        {
            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionEnd(new DateTime(2018, 12, 25));

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(0, intervals.Count);
        }

        [TestMethod]
        public void CalculateIntervals_1StartAndThen1End_Returns1IntervalFromStartToEnd()
        {
            var start = new DateTime(2018, 6, 1);
            var end = new DateTime(2018, 12, 25);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionStart(start);
            eaTimeline.AddPermissionEnd(end);

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(start, intervals.First().Start);
            Assert.IsTrue(intervals.First().IsClosed);
            Assert.AreEqual(end, intervals.First().End);
        }
        
        [TestMethod]
        public void CalculateIntervals_1EndAndThen1Start_Returns1IntervalFromStartToEndOfTime()
        {
            var start = new DateTime(2018, 12, 25);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionEnd(new DateTime(2018, 6, 1));
            eaTimeline.AddPermissionStart(start);
                        
            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(start, intervals.First().Start);
            Assert.IsFalse(intervals.First().IsClosed);
            Assert.IsFalse(intervals.First().End.HasValue);
        }

        [TestMethod]
        public void CalculateIntervals_NStartsAndThen1End_Returns1IntervalFromOldestStartToEnd()
        {
            var start = new DateTime(2018, 4, 12);
            var end = new DateTime(2019, 1, 1);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionStart(new DateTime(2018, 6, 1));
            eaTimeline.AddPermissionStart(start);
            eaTimeline.AddPermissionStart(new DateTime(2018, 5, 21));
            eaTimeline.AddPermissionStart(new DateTime(2018, 8, 10));
            eaTimeline.AddPermissionEnd(end);

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(start, intervals.First().Start);
            Assert.IsTrue(intervals.First().IsClosed);
            Assert.AreEqual(end, intervals.First().End);
        }

        [TestMethod]
        public void CalculateIntervals_0StartsNEnds_Returns0Intervals()
        {
            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionEnd(new DateTime(2019, 1, 1));
            eaTimeline.AddPermissionEnd(new DateTime(2019, 2, 1));
            eaTimeline.AddPermissionEnd(new DateTime(2019, 3, 1));

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(0, intervals.Count);
        }

        [TestMethod]
        public void CalculateIntervals_1StartAndThenNEnds_Returns1IntervalFromStartToNearestEnd()
        {
            var start = new DateTime(2018, 1, 1);
            var end = new DateTime(2019, 1, 1);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionEnd(new DateTime(2019, 3, 1));
            eaTimeline.AddPermissionEnd(new DateTime(2019, 2, 1));
            eaTimeline.AddPermissionEnd(end);
            eaTimeline.AddPermissionStart(start);

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(1, intervals.Count);
            Assert.AreEqual(start, intervals.First().Start);
            Assert.IsTrue(intervals.First().IsClosed);
            Assert.AreEqual(end, intervals.First().End);
        }

        [TestMethod]
        public void CalculateIntervals_NStartsMEnds_AlternateSequence_ReturnsRightIntervals()
        {
            var start1 = new DateTime(2018, 1, 1);
            var end1 = new DateTime(2018, 1, 31);
            var start2 = new DateTime(2018, 6, 1);
            var end2 = new DateTime(2018, 6, 30);
            var start3 = new DateTime(2019, 1, 1);
            var end3 = new DateTime(2019, 1, 31);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionEnd(end1);
            eaTimeline.AddPermissionStart(start2);
            eaTimeline.AddPermissionEnd(end2);
            eaTimeline.AddPermissionEnd(end3);
            eaTimeline.AddPermissionStart(start3);
            eaTimeline.AddPermissionStart(start1);
            
            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(3, intervals.Count);
            Assert.AreEqual(start1, intervals[0].Start);
            Assert.AreEqual(end1, intervals[0].End);
            Assert.AreEqual(start2, intervals[1].Start);
            Assert.AreEqual(end2, intervals[1].End);
            Assert.AreEqual(start3, intervals[2].Start);
            Assert.AreEqual(end3, intervals[2].End);
        }

        [TestMethod]
        public void CalculateIntervals_NStartsMEnds_MixedSequence_ReturnsRightIntervals()
        {
            var start1 = new DateTime(2018, 1, 1);
            var end1 = new DateTime(2018, 1, 31);
            var start2 = new DateTime(2018, 3, 5);
            var end2 = new DateTime(2018, 3, 15);
            var start3 = new DateTime(2018, 4, 1);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionStart(new DateTime(2018, 4, 20));
            eaTimeline.AddPermissionEnd(new DateTime(2018, 2, 20));
            eaTimeline.AddPermissionStart(start3);
            eaTimeline.AddPermissionEnd(new DateTime(2018, 2, 28));
            eaTimeline.AddPermissionStart(new DateTime(2018, 1, 15));
            eaTimeline.AddPermissionStart(start1);
            eaTimeline.AddPermissionEnd(end1);
            eaTimeline.AddPermissionStart(start2);
            eaTimeline.AddPermissionEnd(end2);
            eaTimeline.AddPermissionStart(new DateTime(2018, 4, 10));
            
            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(3, intervals.Count);
            Assert.AreEqual(start1, intervals[0].Start);
            Assert.AreEqual(end1, intervals[0].End);
            Assert.AreEqual(start2, intervals[1].Start);
            Assert.AreEqual(end2, intervals[1].End);
            Assert.AreEqual(start3, intervals[2].Start);
            Assert.IsFalse(intervals[2].IsClosed);
            Assert.IsFalse(intervals[2].End.HasValue);
        }

        [TestMethod]
        public void CalculateIntervals_NStartsMEnds_MixedSequenceWithDuplicatedStartsAndEnds_1_ReturnsRightIntervals()
        {
            var start1 = new DateTime(2018, 1, 2);
            var end1 = new DateTime(2018, 1, 4);
            var start2 = new DateTime(2018, 1, 6);
            var end2 = new DateTime(2018, 1, 9);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionEnd(new DateTime(2018, 1, 1));
            eaTimeline.AddPermissionStart(start1);
            eaTimeline.AddPermissionStart(new DateTime(2018, 1, 3));
            eaTimeline.AddPermissionEnd(end1);
            eaTimeline.AddPermissionEnd(new DateTime(2018, 1, 4));
            eaTimeline.AddPermissionEnd(new DateTime(2018, 1, 5));
            eaTimeline.AddPermissionStart(start2);
            eaTimeline.AddPermissionStart(new DateTime(2018, 1, 6));
            eaTimeline.AddPermissionStart(new DateTime(2018, 1, 7));
            eaTimeline.AddPermissionStart(new DateTime(2018, 1, 7));
            eaTimeline.AddPermissionStart(new DateTime(2018, 1, 8));
            eaTimeline.AddPermissionEnd(end2);
            eaTimeline.AddPermissionEnd(new DateTime(2018, 1, 10));
            eaTimeline.AddPermissionEnd(new DateTime(2018, 1, 10));

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(2, intervals.Count);
            Assert.AreEqual(start1, intervals[0].Start);
            Assert.AreEqual(end1, intervals[0].End);
            Assert.AreEqual(start2, intervals[1].Start);
            Assert.AreEqual(end2, intervals[1].End);
        }

        [TestMethod]
        public void CalculateIntervals_NStartsMEnds_MixedSequenceWithDuplicatedStartsAndEnds_2_ReturnsRightIntervals()
        {
            var start1 = new DateTime(2017, 2, 1);
            var end1 = new DateTime(2017, 2, 28);
            var start2 = new DateTime(2017, 3, 1);
            var end2 = new DateTime(2017, 4, 15);
            var start3 = new DateTime(2017, 4, 18);
            var end3 = new DateTime(2017, 4, 21);
            var start4 = new DateTime(2017, 4, 22);
            var end4 = new DateTime(2017, 4, 23);
            var start5 = new DateTime(2017, 4, 24);
            var end5 = new DateTime(2017, 5, 1);
            var start6 = new DateTime(2017, 6, 12);
            var end6 = new DateTime(2017, 7, 12);
            var start7 = new DateTime(2017, 7, 30);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionEnd(new DateTime(2017, 2, 28));
            eaTimeline.AddPermissionStart(new DateTime(2017, 4, 24));
            eaTimeline.AddPermissionEnd(new DateTime(2017, 5, 2));
            eaTimeline.AddPermissionEnd(end6);
            eaTimeline.AddPermissionStart(start6);
            eaTimeline.AddPermissionStart(new DateTime(2017, 4, 22));
            eaTimeline.AddPermissionStart(new DateTime(2017, 4, 24));
            eaTimeline.AddPermissionStart(start7);
            eaTimeline.AddPermissionEnd(new DateTime(2017, 2, 28));
            eaTimeline.AddPermissionStart(start3);
            eaTimeline.AddPermissionStart(start2);
            eaTimeline.AddPermissionEnd(end4);
            eaTimeline.AddPermissionEnd(end1);
            eaTimeline.AddPermissionEnd(end3);
            eaTimeline.AddPermissionEnd(new DateTime(2017, 2, 28));
            eaTimeline.AddPermissionEnd(new DateTime(2017, 1, 1));
            eaTimeline.AddPermissionEnd(end5);
            eaTimeline.AddPermissionEnd(new DateTime(2017, 4, 17));
            eaTimeline.AddPermissionStart(new DateTime(2017, 2, 3));
            eaTimeline.AddPermissionStart(start1);
            eaTimeline.AddPermissionStart(new DateTime(2017, 2, 2));
            eaTimeline.AddPermissionEnd(new DateTime(2017, 2, 28));
            eaTimeline.AddPermissionEnd(new DateTime(2017, 2, 28));
            eaTimeline.AddPermissionEnd(new DateTime(2017, 2, 28));
            eaTimeline.AddPermissionEnd(end2);
            eaTimeline.AddPermissionEnd(new DateTime(2017, 4, 16));
            eaTimeline.AddPermissionStart(start4);
            eaTimeline.AddPermissionStart(start5);
            eaTimeline.AddPermissionStart(new DateTime(2017, 2, 2));
            eaTimeline.AddPermissionStart(new DateTime(2017, 8, 15));

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();

            Assert.AreEqual(7, intervals.Count);
            Assert.AreEqual(start1, intervals[0].Start);
            Assert.AreEqual(end1, intervals[0].End);
            Assert.AreEqual(start2, intervals[1].Start);
            Assert.AreEqual(end2, intervals[1].End);
            Assert.AreEqual(start3, intervals[2].Start);
            Assert.AreEqual(end3, intervals[2].End);
            Assert.AreEqual(start4, intervals[3].Start);
            Assert.AreEqual(end4, intervals[3].End);
            Assert.AreEqual(start5, intervals[4].Start);
            Assert.AreEqual(end5, intervals[4].End);
            Assert.AreEqual(start6, intervals[5].Start);
            Assert.AreEqual(end6, intervals[5].End);
            Assert.AreEqual(start7, intervals[6].Start);
            Assert.IsFalse(intervals[6].IsClosed);
            Assert.IsFalse(intervals[6].End.HasValue);
        }

        [TestMethod]
        public void CalculateIntervals_1StartAndThenOppositeStartAndEnd_Returns2ConsecutiveIntervals()
        {
            var start = new DateTime(2018, 1, 1);
            var opposite = new DateTime(2018, 2, 1);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionStart(start);
            eaTimeline.AddPermissionStart(opposite);
            eaTimeline.AddPermissionEnd(opposite);

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(2, intervals.Count);
            Assert.AreEqual(start, intervals[0].Start);
            Assert.AreEqual(opposite, intervals[0].End);
            Assert.AreEqual(opposite, intervals[1].Start);
            Assert.IsFalse(intervals[1].IsClosed);
            Assert.IsFalse(intervals[1].End.HasValue);
        }

        [TestMethod]
        public void CalculateIntervals_1EndAndThenOppositeStartAndEnd_Returns0Intervals()
        {
            var end = new DateTime(2018, 1, 1);
            var opposite = new DateTime(2018, 2, 1);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionEnd(end);
            eaTimeline.AddPermissionStart(opposite);
            eaTimeline.AddPermissionEnd(opposite);

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(0, intervals.Count);
        }

        [TestMethod]
        public void CalculateIntervals_1StartAndThenOppositeAndDuplicatedStartsAndEnds_Returns2ConsecutiveIntervals()
        {
            var start = new DateTime(2018, 1, 1);
            var oppositeAndDuplicated = new DateTime(2018, 2, 1);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionStart(start);
            eaTimeline.AddPermissionStart(oppositeAndDuplicated);
            eaTimeline.AddPermissionStart(oppositeAndDuplicated);
            eaTimeline.AddPermissionEnd(oppositeAndDuplicated);
            eaTimeline.AddPermissionEnd(oppositeAndDuplicated);
            eaTimeline.AddPermissionEnd(oppositeAndDuplicated);
            eaTimeline.AddPermissionEnd(oppositeAndDuplicated);

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(2, intervals.Count);
            Assert.AreEqual(start, intervals[0].Start);
            Assert.AreEqual(oppositeAndDuplicated, intervals[0].End);
            Assert.AreEqual(oppositeAndDuplicated, intervals[1].Start);
            Assert.IsFalse(intervals[1].IsClosed);
            Assert.IsFalse(intervals[1].End.HasValue);
        }

        [TestMethod]
        public void CalculateIntervals_1EndAndThenOppositeAndDuplicatedStartsAndEnds_Returns0Intervals()
        {
            var end = new DateTime(2018, 1, 1);
            var oppositeAndDuplicated = new DateTime(2018, 2, 1);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionEnd(end);
            eaTimeline.AddPermissionStart(oppositeAndDuplicated);
            eaTimeline.AddPermissionStart(oppositeAndDuplicated);
            eaTimeline.AddPermissionStart(oppositeAndDuplicated);
            eaTimeline.AddPermissionStart(oppositeAndDuplicated);
            eaTimeline.AddPermissionEnd(oppositeAndDuplicated);
            eaTimeline.AddPermissionEnd(oppositeAndDuplicated);

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(0, intervals.Count);
        }

        [TestMethod]
        public void CalculateIntervals_NStartsMEnds_MixedSequenceIncludingOppositeStartAndEndAfterAnEnd_ReturnsRightIntervals()
        {
            var start1 = new DateTime(2018, 1, 1);
            var end1 = new DateTime(2018, 1, 5);
            var opposite = new DateTime(2018, 2, 1);
            var start2 = new DateTime(2018, 3, 1);
            var end2 = new DateTime(2018, 3, 5);
            var start3 = new DateTime(2018, 4, 1);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionStart(start1);
            eaTimeline.AddPermissionEnd(end1);
            eaTimeline.AddPermissionStart(opposite);
            eaTimeline.AddPermissionEnd(opposite);
            eaTimeline.AddPermissionStart(start2);
            eaTimeline.AddPermissionEnd(end2);
            eaTimeline.AddPermissionStart(start3);

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(3, intervals.Count);
            Assert.AreEqual(start1, intervals[0].Start);
            Assert.AreEqual(end1, intervals[0].End);
            Assert.AreEqual(start2, intervals[1].Start);
            Assert.AreEqual(end2, intervals[1].End);
            Assert.AreEqual(start3, intervals[2].Start);
            Assert.IsFalse(intervals[2].IsClosed);
        }

        [TestMethod]
        public void CalculateIntervals_NStartsMEnds_MixedSequenceIncludingOppositeStartAndEndAfterAStart_ReturnsRightIntervals()
        {
            var start1 = new DateTime(2018, 1, 1);
            var opposite = new DateTime(2018, 2, 1);
            var start2 = new DateTime(2018, 3, 1);
            var end2 = new DateTime(2018, 3, 5);
            var start3 = new DateTime(2018, 4, 1);
            var end3 = new DateTime(2018, 5, 1);

            var eaTimeline = new EffectiveAuthorizationTimeline(GetFakeEffectiveAuthorization());
            eaTimeline.AddPermissionStart(start1);
            eaTimeline.AddPermissionStart(opposite);
            eaTimeline.AddPermissionEnd(opposite);
            eaTimeline.AddPermissionStart(start2);
            eaTimeline.AddPermissionEnd(end2);
            eaTimeline.AddPermissionStart(start3);
            eaTimeline.AddPermissionEnd(end3);

            var intervals = eaTimeline.CalculateEffectiveIntervals().ToList();
            Assert.AreEqual(3, intervals.Count);

            Assert.AreEqual(start1, intervals[0].Start);
            Assert.AreEqual(opposite, intervals[0].End);
            Assert.AreEqual(opposite, intervals[1].Start);
            Assert.AreEqual(end2, intervals[1].End);
            Assert.AreEqual(start3, intervals[2].Start);
            Assert.AreEqual(end3, intervals[2].End);
        }

        private EffectiveAuthorization GetFakeEffectiveAuthorization()
        {
            return new EffectiveAuthorization()
            {
                TenantId = "4024898",
                User = new ExternalId() { Context = "Context.Test", Id = "001" },
                Permission = new Permission() { Application = "Test App", Id = "T1" },
                Target = new ExternalId() { Context = "Context.Test", Id = "002" }
            };
        }
    }
}