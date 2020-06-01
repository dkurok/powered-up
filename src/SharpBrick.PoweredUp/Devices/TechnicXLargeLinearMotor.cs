using System;
using SharpBrick.PoweredUp.Knowledge;
using SharpBrick.PoweredUp.Protocol.Messages;

namespace SharpBrick.PoweredUp.Devices
{
    public class TechnicXLargeLinearMotor : IPowerdUpDevice
    {
        // TODO: consider recode this as pre-recorded messages (would simplify the onboarding of new devices)
        public void ApplyStaticPortInfo(PortInfo port)
        {
            //TODO: check version. later version can have different modes.
            port.IOTypeId = HubAttachedIOType.TechnicXLargeLinearMotor;
            port.HardwareRevision = new Version("0.0.0.1000"); // TODO
            port.SoftwareRevision = new Version("0.0.0.1000"); //TODO
            port.OutputCapability = true;
            port.InputCapability = true;
            port.LogicalCombinableCapability = true;
            port.LogicalSynchronizableCapability = true;

            port.Modes = new PortModeInfo[] {
                new PortModeInfo() {
                    PortId = port.PortId,
                    ModeIndex = 0,
                    Name= "POWER",
                    IsInput= false,
                    IsOutput= true,
                    RawMin= -100,
                    RawMax= 100,
                    PctMin= -100,
                    PctMax= 100,
                    SIMin= -100,
                    SIMax= 100,
                    Symbol= "PCT",
                    InputSupportsNull= false,
                    InputSupportFunctionalMapping20= false,
                    InputAbsolute= false,
                    InputRelative= false,
                    InputDiscrete= false,
                    OutputSupportsNull= false,
                    OutputSupportFunctionalMapping20= false,
                    OutputAbsolute= true,
                    OutputRelative= false,
                    OutputDiscrete= false,
                    NumberOfDatasets= 1,
                    DatasetType= PortModeInformationDataType.SByte,
                    TotalFigures= 1,
                    Decimals= 0,
                    DeltaInterval= 0,
                    NotificationEnabled= false,
                },
                new PortModeInfo() {
                    PortId = port.PortId,
                    ModeIndex = 1,
                    Name= "SPEED",
                    IsInput= true,
                    IsOutput= true,
                    RawMin= -100,
                    RawMax= 100,
                    PctMin= -100,
                    PctMax= 100,
                    SIMin= -100,
                    SIMax= 100,
                    Symbol= "PCT",
                    InputSupportsNull= false,
                    InputSupportFunctionalMapping20= false,
                    InputAbsolute= true,
                    InputRelative= false,
                    InputDiscrete= false,
                    OutputSupportsNull= false,
                    OutputSupportFunctionalMapping20= false,
                    OutputAbsolute= true,
                    OutputRelative= false,
                    OutputDiscrete= false,
                    NumberOfDatasets= 1,
                    DatasetType= PortModeInformationDataType.SByte,
                    TotalFigures= 4,
                    Decimals= 0,
                    DeltaInterval= 0,
                    NotificationEnabled= false,
                },
                new PortModeInfo() {
                    PortId = port.PortId,
                    ModeIndex = 2,
                    Name= "POS",
                    IsInput= true,
                    IsOutput= true,
                    RawMin= -360,
                    RawMax= 360,
                    PctMin= -100,
                    PctMax= 100,
                    SIMin= -360,
                    SIMax= 360,
                    Symbol= "DEG",
                    InputSupportsNull= false,
                    InputSupportFunctionalMapping20= false,
                    InputAbsolute= false,
                    InputRelative= true,
                    InputDiscrete= false,
                    OutputSupportsNull= false,
                    OutputSupportFunctionalMapping20= false,
                    OutputAbsolute= false,
                    OutputRelative= true,
                    OutputDiscrete= false,
                    NumberOfDatasets= 1,
                    DatasetType= PortModeInformationDataType.Int32,
                    TotalFigures= 4,
                    Decimals= 0,
                    DeltaInterval= 0,
                    NotificationEnabled= false,
                },
                new PortModeInfo() {
                    PortId = port.PortId,
                    ModeIndex = 3,
                    Name= "APOS",
                    IsInput= true,
                    IsOutput= true,
                    RawMin= -360,
                    RawMax= 360,
                    PctMin= -100,
                    PctMax= 100,
                    SIMin= -360,
                    SIMax= 360,
                    Symbol= "DEG",
                    InputSupportsNull= false,
                    InputSupportFunctionalMapping20= false,
                    InputAbsolute= false,
                    InputRelative= true,
                    InputDiscrete= false,
                    OutputSupportsNull= false,
                    OutputSupportFunctionalMapping20= false,
                    OutputAbsolute= false,
                    OutputRelative= true,
                    OutputDiscrete= false,
                    NumberOfDatasets= 1,
                    DatasetType= PortModeInformationDataType.Int16,
                    TotalFigures= 3,
                    Decimals= 0,
                    DeltaInterval= 0,
                    NotificationEnabled= false,
                },
                new PortModeInfo() {
                    PortId = port.PortId,
                    ModeIndex = 4,
                    Name= "LOAD",
                    IsInput= true,
                    IsOutput= true,
                    RawMin= 0,
                    RawMax= 127,
                    PctMin= 0,
                    PctMax= 100,
                    SIMin= 0,
                    SIMax= 127,
                    Symbol= "PCT",
                    InputSupportsNull= false,
                    InputSupportFunctionalMapping20= false,
                    InputAbsolute= false,
                    InputRelative= true,
                    InputDiscrete= false,
                    OutputSupportsNull= false,
                    OutputSupportFunctionalMapping20= false,
                    OutputAbsolute= false,
                    OutputRelative= true,
                    OutputDiscrete= false,
                    NumberOfDatasets= 1,
                    DatasetType= PortModeInformationDataType.SByte,
                    TotalFigures= 1,
                    Decimals= 0,
                    DeltaInterval= 0,
                    NotificationEnabled= false,
                },
                new PortModeInfo() {
                    PortId = port.PortId,
                    ModeIndex = 5,
                    Name= "CALIB",
                    IsInput= false,
                    IsOutput= false,
                    RawMin= 0,
                    RawMax= 512,
                    PctMin= 0,
                    PctMax= 100,
                    SIMin= 0,
                    SIMax= 512,
                    Symbol= "RAW",
                    InputSupportsNull= false,
                    InputSupportFunctionalMapping20= false,
                    InputAbsolute= false,
                    InputRelative= false,
                    InputDiscrete= false,
                    OutputSupportsNull= false,
                    OutputSupportFunctionalMapping20= false,
                    OutputAbsolute= false,
                    OutputRelative= false,
                    OutputDiscrete= false,
                    NumberOfDatasets= 3,
                    DatasetType= PortModeInformationDataType.Int16,
                    TotalFigures= 3,
                    Decimals= 0,
                    DeltaInterval= 0,
                    NotificationEnabled= false,
                },
            };

            port.ModeCombinations = new ushort[] {
                0b0000_0000_0000_1110
            };

            port.UsedCombinationIndex = 0;
            port.MultiUpdateEnabled = false;
            port.ConfiguredModeDataSetIndex = Array.Empty<int>();
        }
    }
}