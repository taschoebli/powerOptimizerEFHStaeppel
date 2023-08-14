using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using powerOptimizerEFHStaeppel.Interfaces;
using powerOptimizerEFHStaeppel.Tests.Mocks;

namespace powerOptimizerEFHStaeppel.Tests
{
    public class LoggerTestData
    {
        private ILogger? _logger;

        private DateTimeProviderMock? _dateTimeProviderMock;

        #region Properties

        #region Test Units

        public ILogger? Logger
        {
            get => _logger ??= CreateLogger();

            set => _logger = value;
        }

        #endregion

        #region Modules

        public IDateTimeProvider DateTimeProvider => DateTimeProviderMock.Object;

        #endregion

        #region Mocks

        public DateTimeProviderMock DateTimeProviderMock
        {
            get => _dateTimeProviderMock ??= CreateDateTimeProviderMock();

            set => _dateTimeProviderMock = value;
        }

        #endregion

        #region Data

        public string MessagePVPower { get; } = "pv power: 5450";

        public string MessageExportPower { get; } = "export power: 1450";

        public string MessageLoadPower { get; } = "load power: 3000";

        public string MessageStoragePower { get; } = "storage power: 0";

        public string MessageBatteryLevel { get; } = "battery level: 970";

        public string MessageLoadSwitch { get; } = "load switch: enabled";

        private static string Year { get; } = "2023";

        private static string Month { get; } = "08";

        private static string Day { get; } = "05";

        public DateTime DateTimeValue { get; } = new DateTime(int.Parse(Year), int.Parse(Month), int.Parse(Day), 10, 50, 40);

        public string FileName => GetFileName();

        private string GetFileName()
        {
            var separator = Path.DirectorySeparatorChar;
            var result = Logger?.LoggerDirectoryFullPath + separator + Logger?.FileNamePrefix + $"{Year}_{Month}_{Day}" + Logger?.FileType;

            return result;
        }

        #endregion

        #endregion

        #region Methods

        #region Setup

        public void SetupDateTimeNow()
        {
            DateTimeProviderMock?.SetupDateTimeNow(DateTimeValue);
        }

        public void SetupMessageLineItems()
        {
            Logger?.AddMessageLine(MessagePVPower);
            Logger?.AddMessageLine(MessageExportPower);
        }

        #endregion

        #region Init & Reset

        public void Reset()
        {
            Logger = null;
            DateTimeProviderMock = null;
        }

        #endregion

        #region Factory

        protected virtual ILogger CreateLogger()
        {
            return new Logger(DateTimeProvider);
        }

        private DateTimeProviderMock CreateDateTimeProviderMock()
        {
            return new DateTimeProviderMock();
        }



        #endregion

        #endregion
    }
}
