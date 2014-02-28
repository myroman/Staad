using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Staad.Domain.Helpers;

namespace Staad.Tests
{
    [Category("DateHelper")]
    public class DateHelperTests
    {
        [Test(Description = @"If now WhenItWas returns ""today""")]
        [Category("DateHelper")]
        public void IfNow_WhenItWas_ReturnsToday()
        {
            var dateToCheck = DateTime.Now;
            var when = dateToCheck.WhenItWas();
            Assert.AreEqual("today", when);
        }

        [Test(Description = @"If yesterday WhenItWas returns ""yesterday""")]
        [Category("DateHelper")]
        public void IfYesterday_WhenItWas_ReturnsYesterday()
        {
            var dateToCheck = DateTime.Now.AddDays(-1);
            var when = dateToCheck.WhenItWas();
            Assert.AreEqual("yesterday", when);
        }

        [Test(Description = @"If 2 days ago WhenItWas returns ""this month""")]
        [Category("DateHelper")]
        public void IfTwoDaysAgo_WhenItWas_ReturnsThisMonth()
        {
            var dateToCheck = DateTime.Now.AddDays(-2);
            var when = dateToCheck.WhenItWas();
            Assert.AreEqual("this month", when);
        }

        [Test(Description = @"If now is June WhenItWas for 31 December returns ""6 months ago""")]
        [Category("DateHelper")]
        public void IfNowIsJune_WhenItWas_For_31December_Returns6MonthsAgo()
        {
            var dateToCheck = new DateTime(DateTime.Now.AddYears(-1).Year, 12, 31);
            var when = dateToCheck.WhenItWas();
            Assert.AreEqual("6 months ago", when);
        }

        [Test(Description = @"If this month but last year WhenItWas returns ""a year ago""")]
        [Category("DateHelper")]
        public void IfThisMonthButLastYear_WhenItWas_ReturnsAYearAgo()
        {
            var dateToCheck = DateTime.Now.AddDays(-2).AddYears(-1);
            var when = dateToCheck.WhenItWas();
            Assert.AreEqual("a year ago", when);
        }
    }
}
