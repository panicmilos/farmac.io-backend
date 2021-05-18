using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Farmacio_Services.Implementation.Utils;
using Xunit;

namespace Farmacio_Tests.UnitTests.TimeIntervalUtilsTests
{
    public class TimeIntervalTimesOverlapTests
    {

        [Fact]
        public void TimeIntervalTimesOverlap_ForSameTimeIntervals_ReturnsTrue()
        {
                            // year month day hour minute second
            var firstFrom =  new DateTime(1999, 12, 23, 2, 2, 0);
            var firstTo =    new DateTime(1999, 12, 23, 4, 2, 0);
            var secondFrom = new DateTime(1999, 12, 23, 2, 2, 0);
            var secondTo =   new DateTime(1999, 12, 23, 4, 2, 0);

            var result = TimeIntervalUtils.TimeIntervalTimesOverlap(firstFrom, firstTo, secondFrom, secondTo);
            
            Assert.False(result);
        }
        
        [Fact]
        public void TimeIntervalTimesOverlap_ForFirstToBeforeSecondFrom_ReturnsFalse()
        {
                            // year month day hour minute second
            var firstFrom =  new DateTime(1999, 12, 23, 2, 2, 0);
            var firstTo =    new DateTime(1999, 12, 23, 4, 2, 0);
            var secondFrom = new DateTime(1999, 12, 23, 6, 2, 0);
            var secondTo =   new DateTime(1999, 12, 23, 8, 2, 0);

            var result = TimeIntervalUtils.TimeIntervalTimesOverlap(firstFrom, firstTo, secondFrom, secondTo);
            
            Assert.False(result);
        }
        
        [Fact]
        public void TimeIntervalTimesOverlap_ForFirstFromAfterSecondTo_ReturnsFalse()
        {
                            // year month day hour minute second
            var firstFrom =  new DateTime(1999, 12, 23, 6, 2, 0);
            var firstTo =    new DateTime(1999, 12, 23, 8, 2, 0);
            var secondFrom = new DateTime(1999, 12, 23, 2, 2, 0);
            var secondTo =   new DateTime(1999, 12, 23, 4, 2, 0);

            var result = TimeIntervalUtils.TimeIntervalTimesOverlap(firstFrom, firstTo, secondFrom, secondTo);
            
            Assert.False(result);
        }
        
        [Fact]
        public void TimeIntervalTimesOverlap_ForFirstToEqualsSecondFrom_ReturnsFalse()
        {
                            // year month day hour minute second
            var firstFrom =  new DateTime(1999, 12, 23, 2, 2, 0);
            var firstTo =    new DateTime(1999, 12, 23, 6, 2, 0);
            var secondFrom = new DateTime(1999, 12, 23, 6, 2, 0);
            var secondTo =   new DateTime(1999, 12, 23, 8, 2, 0);

            var result = TimeIntervalUtils.TimeIntervalTimesOverlap(firstFrom, firstTo, secondFrom, secondTo);
            
            Assert.False(result);
        }
        
        [Fact]
        public void TimeIntervalTimesOverlap_ForFirstFromEqualsSecondTo_ReturnsFalse()
        {
            // year month day hour minute second
            var firstFrom =  new DateTime(1999, 12, 23, 6, 2, 0);
            var firstTo =    new DateTime(1999, 12, 23, 8, 2, 0);
            var secondFrom = new DateTime(1999, 12, 23, 8, 2, 0);
            var secondTo =   new DateTime(1999, 12, 23, 9, 2, 0);

            var result = TimeIntervalUtils.TimeIntervalTimesOverlap(firstFrom, firstTo, secondFrom, secondTo);
            
            Assert.False(result);
        }
        
        [Fact]
        public void TimeIntervalTimesOverlap_ForFirstToInsideSecondInterval_ReturnsTrue()
        {
            // year month day hour minute second
            var firstFrom =  new DateTime(1999, 12, 23, 6, 2, 0);
            var firstTo =    new DateTime(1999, 12, 23, 8, 3, 0);
            var secondFrom = new DateTime(1999, 12, 23, 8, 2, 0);
            var secondTo =   new DateTime(1999, 12, 23, 9, 2, 0);

            var result = TimeIntervalUtils.TimeIntervalTimesOverlap(firstFrom, firstTo, secondFrom, secondTo);
            
            Assert.True(result);
        }
        
        [Fact]
        public void TimeIntervalTimesOverlap_ForFirstFromInsideSecondInterval_ReturnsTrue()
        {
            // year month day hour minute second
            var firstFrom =  new DateTime(1999, 12, 23, 8, 3, 0);
            var firstTo =    new DateTime(1999, 12, 23, 9, 3, 0);
            var secondFrom = new DateTime(1999, 12, 23, 8, 2, 0);
            var secondTo =   new DateTime(1999, 12, 23, 9, 2, 0);

            var result = TimeIntervalUtils.TimeIntervalTimesOverlap(firstFrom, firstTo, secondFrom, secondTo);
            
            Assert.True(result);
        }
        
        [Fact]
        public void TimeIntervalTimesOverlap_ForFirstIntervalInsideSecondInterval_ReturnsTrue()
        {
            // year month day hour minute second
            var firstFrom =  new DateTime(1999, 12, 23, 8, 3, 0);
            var firstTo =    new DateTime(1999, 12, 23, 9, 1, 0);
            var secondFrom = new DateTime(1999, 12, 23, 8, 2, 0);
            var secondTo =   new DateTime(1999, 12, 23, 9, 2, 0);

            var result = TimeIntervalUtils.TimeIntervalTimesOverlap(firstFrom, firstTo, secondFrom, secondTo);
            
            Assert.True(result);
        }

    }
}