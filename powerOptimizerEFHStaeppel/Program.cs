﻿using FluentModbus;
using powerOptimizerEFHStaeppel;

//Read Registers
const int TotalDCPowerAddress = 5017 - 1;
const int LoadPowerAddress = 13008 - 1;
const int ExportPowerAddress = 13010 - 1;
const int BatteryPowerAddress = 13022 - 1;
const int BatteryLevelAddress = 13023 - 1;

//Holding Registers
const int Load1AdjustmentModeAddress = 13002 - 1;
const int Load1Address = 13011 - 1;
const ushort Load1ON = 0x00AA;
const ushort Load1OFF = 0x0055;

// Heater Threshold
const short WaterHeatPumpThresholdPower = 1500;

const ushort BatteryLevelThreshold = 950;
const ushort BatteryLevelLowerThreshold = 10;

// Inverter IP Address
const string IPAddress = "192.168.1.125"; // WiNet Espressif 192.168.1.125, LAN connector 192.168.1.172
const ushort MaxWriteTimeout = 10000;
const ushort MaxReadTimeout = 10000;

Console.WriteLine("This is the pv optimizer EFH Stäppel. Press any key to stop application and just wait...");
Console.WriteLine("Wait 10s");
Thread.Sleep(10000);

var client = new ModbusTcpClient();
var dateTimeProvider = new DateTimeProvider();
var logger = new Logger(dateTimeProvider);

logger.AddMessageLine($"connecting to {IPAddress}");
client.Connect(System.Net.IPAddress.Parse(IPAddress), ModbusEndianness.BigEndian);
client.WriteTimeout = MaxWriteTimeout;
client.ReadTimeout = MaxReadTimeout;

var unitIdentifier = 0x01;
var count = 1;
var waterHeatPumpEnabled = false;

if (client.IsConnected)
{
    logger.AddMessageLine("client connected.");
    logger.AddMessageLine("read inverter value on startup start:");
    ReadLoadSwitch(true);
    logger.AddMessageLine("read inverter value on startup finish:");

    //logger.Log($"Set Load 1 OFF");
    //client.WriteSingleRegister(unitIdentifier, Load1Address, Load1OFF);
    //logger.Log("Set ON/OFF mode.");
    //client.WriteSingleRegister(unitIdentifier, Load1AdjustmentModeAddress, 0x0001);


    //logger.Log($"Set Load 1 ON");
    //client.WriteSingleRegister(unitIdentifier, Load1Address, Load1ON);
    //waterHeatPumpEnabled = true;

}

while (client.IsConnected && (Console.KeyAvailable == false))
{
    //Running State only available over LAN connector
    //var runningStateData = client.ReadInputRegisters<ushort>(unitIdentifier, RunningStateAddress, count);
    //var runningState = runningStateData[0];
    //var runningStateBinAsString = Convert.ToString(runningState, 2);
    //logger.Log($"Running state:{runningState}");
    //logger.Log($"Running state binary:{runningStateBinAsString}");

    var exportData = client.ReadInputRegisters<short>(unitIdentifier, ExportPowerAddress, count);
    var exportPower = exportData[0];
    var exportPowerHexAsString = exportData[0].ToString("X4");
    logger.AddMessageLine($"export power: {exportPower}");
    //logger.Log($"Export power in W as hex value:{exportPowerHexAsString}");

    var totalDCPowerData = client.ReadInputRegisters<short>(unitIdentifier, TotalDCPowerAddress, count);
    var DCPower = totalDCPowerData[0];
    var DCPowerHexAsString = totalDCPowerData[0].ToString("X4");
    logger.AddMessageLine($"pv power: {DCPower}");
    //logger.Log($"Total DC PV power in W as hex value:{DCPowerHexAsString}");

    var loadData = client.ReadInputRegisters<short>(unitIdentifier, LoadPowerAddress, count);
    var loadPower = loadData[0];
    var loadPowerHexAsString = loadData[0].ToString("X4");
    logger.AddMessageLine($"load power: {loadPower}");
    //logger.Log($"Load power in W as hex value:{loadPowerHexAsString}");

    var batterData = client.ReadInputRegisters<ushort>(unitIdentifier, BatteryPowerAddress, count);
    var batteryPower = batterData[0];
    var batteryPowerHexAsString = batterData[0].ToString("X4");
    logger.AddMessageLine($"storage power: {batteryPower}");
    //logger.Log($"Battery power in W as hex value:{batteryPowerHexAsString}");

    var batteryLevelData = client.ReadInputRegisters<ushort>(unitIdentifier, BatteryLevelAddress, count);
    var batteryLevel = batteryLevelData[0];
    logger.AddMessageLine($"battery level: {batteryLevel}");

    ReadLoadSwitch();

    //var loadAdjustmentData = client.ReadHoldingRegisters<ushort>(unitIdentifier, Load1AdjustmentModeAddress, count);
    //var loadAdjustmentMode = loadAdjustmentData[0];
    //var loadAdjustmentModeHexAsString = loadAdjustmentMode.ToString("X2");
    //logger.Log($"Load 1 Adjustment Mode as hex value:{loadAdjustmentModeHexAsString}");


    if (!waterHeatPumpEnabled && (DCPower - (WaterHeatPumpThresholdPower + loadPower) > 0) && (exportPower > 0) && ((batteryLevel > BatteryLevelThreshold) || (batteryLevel < BatteryLevelLowerThreshold)))
    {
        waterHeatPumpEnabled = true;
        client.WriteSingleRegister(unitIdentifier, Load1Address, Load1ON);
        logger.AddMessageLine("--------------------------------");
        logger.AddMessageLine($"load enabled");
    }
    else if (waterHeatPumpEnabled && (((DCPower - loadPower) < 0) || (DCPower - WaterHeatPumpThresholdPower < 0)))
    {
        waterHeatPumpEnabled = false;
        client.WriteSingleRegister(unitIdentifier, Load1Address, Load1OFF);
        logger.AddMessageLine("--------------------------------");
        logger.AddMessageLine($"load disabled");
    }

    logger.AddMessageLine(string.Empty);
    logger.WriteMessagesToLogfile();
    Thread.Sleep(15000);
}

client.Disconnect();
logger.AddMessageLine($"is connected: {client.IsConnected} & dispose called");
client.Dispose();
logger.AddMessageLine("bye bye");
logger.WriteMessagesToLogfile();

void ReadLoadSwitch(bool waterHeatPumpOverrideEnabled = false)
{
    var holdingData = client.ReadHoldingRegisters<ushort>(unitIdentifier, Load1Address, count);
    var load1Switch = holdingData[0];
    var load1HexAsString = load1Switch.ToString("X2");
    string loadValue = "undefined";

    if (load1HexAsString == "AA")
    {
        loadValue = "enabled";
        if (waterHeatPumpOverrideEnabled)
        {
            waterHeatPumpEnabled = true;
        }
    }
    else if (load1HexAsString == "55")
    {
        loadValue = "disabled";
        if (waterHeatPumpOverrideEnabled)
        {
            waterHeatPumpEnabled = false;
        }
    }
    logger.AddMessageLine($"load switch: {loadValue}");
}

//client.WriteSingleRegister(unitIdentifier, Load1AdjustmentModeAddress, 0x0000); // 0: Timing mode, 1: On/Off mode
