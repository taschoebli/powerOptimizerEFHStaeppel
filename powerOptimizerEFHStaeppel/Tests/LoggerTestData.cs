using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace powerOptimizerEFHStaeppel.Tests
{
    public class LoggerTestData
    {
        private ILogger? _logger;

        #region Properties

        #region Test Units

        public ILogger? Logger
        {
            get => _logger ??= CreateLogger();

            set => _logger = value;
        }

        #region Data

        public string MessagePVPower { get; } = "pv power: 5450";

        public string MessageExportPower { get; } = "export power: 1450";

        public string MessageLoadPower { get; } = "load power: 3000";

        public string MessageStoragePower { get; } = "storage power: 0";

        public string MessageBatteryLevel { get; } = "battery level: 970";

        public string MessageLoadSwitch { get; } = "load switch: enabled";

        #endregion

        #endregion

        #endregion

        #region Methods

        #region Init

        public void Reset()
        {
            Logger = null;
        }

        #endregion

        #region Factory

        protected virtual ILogger CreateLogger()
        {
            return new Logger();
        }

        #endregion

        #endregion
    }
}
