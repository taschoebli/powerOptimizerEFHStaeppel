using Moq;
using powerOptimizerEFHStaeppel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace powerOptimizerEFHStaeppel.Tests.Mocks
{
    public class DateTimeProviderMock : Mock<IDateTimeProvider>
    {
        #region Methods

        #region Setup

        public void SetupDateTimeNow(DateTime now)
        {
            Setup(x => x.DateTimeNow).Returns(now);
        }
        
        #endregion
        
        #endregion
    }
}
