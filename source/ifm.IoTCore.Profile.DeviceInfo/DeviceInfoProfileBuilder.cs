namespace ifm.IoTCore.Profile.DeviceInfo;

using ElementManager.Contracts;
using ElementManager.Contracts.Elements;
using ElementManager.Contracts.Elements.Formats;
using ElementManager.Contracts.Elements.Valuations;
using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class DeviceInfoProfileBuilder
{
    private readonly IElementManager _elementManager;
    private readonly IDeviceInfo _deviceInfo;
    private readonly IBaseElement _targetElement;

    private const string ProfileName = "deviceinfo";
    private readonly List<string> _parameterProfile = new() { "parameter" };

    public DeviceInfoProfileBuilder(IElementManager elementManager, IBaseElement targetElement, IDeviceInfo deviceInfo)
    {
        _elementManager = elementManager ?? throw new ArgumentNullException(nameof(elementManager));
        _deviceInfo = deviceInfo ?? throw new ArgumentNullException(nameof(deviceInfo));
        _targetElement = targetElement;
    }

    public void Build(TimeSpan? timeout = null)
    {
        var deviceInfoElement = _targetElement.GetElementByIdentifier(ProfileName) ?? _elementManager.CreateStructureElement(_targetElement, ProfileName, profiles: new List<string> { ProfileName });

        if (_deviceInfo.DeviceName != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "devicename", _ => _deviceInfo.DeviceName, profiles: _parameterProfile, cacheTimeout: timeout);
        }


        if (_deviceInfo.DeviceFamily != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "devicefamily", _ => _deviceInfo.DeviceFamily, profiles: _parameterProfile, cacheTimeout: timeout);
        }


        if (_deviceInfo.SerialNumber != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "serialnumber", _ => _deviceInfo.SerialNumber, profiles: _parameterProfile, cacheTimeout: timeout);
        }


        if (_deviceInfo.ProductName != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "productname", _ => _deviceInfo.ProductName, profiles: _parameterProfile, cacheTimeout: timeout);
        }

        if (_deviceInfo.ProductCode != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "productcode", _ => _deviceInfo.ProductCode, profiles: _parameterProfile, cacheTimeout: timeout);
        }


        if (_deviceInfo.OrderNumber != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "ordernumber", _ => _deviceInfo.OrderNumber, profiles: _parameterProfile, cacheTimeout: timeout);
        }

        if (_deviceInfo.ProductionDate != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "productiondate", _ => _deviceInfo.ProductionDate, profiles: _parameterProfile, cacheTimeout: timeout);
        }


        if (_deviceInfo.HwRevision != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "hwrevision", _ => _deviceInfo.HwRevision, profiles: _parameterProfile, cacheTimeout: timeout);
        }

        if (_deviceInfo.SwRevision != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "swrevision", _ => _deviceInfo.SwRevision, profiles: _parameterProfile, cacheTimeout: timeout);
        }

        if (_deviceInfo.SwVersion != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "swversion", _ => _deviceInfo.SwRevision, profiles: _parameterProfile, cacheTimeout: timeout);
        }

        if (_deviceInfo.BootloaderRevision != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "bootloaderrevision", _ => _deviceInfo.BootloaderRevision, profiles: _parameterProfile, cacheTimeout: timeout);
        }

        if (_deviceInfo.Vendor != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "vendor", _ => _deviceInfo.Vendor, profiles: _parameterProfile, cacheTimeout: timeout);
        }

        if (_deviceInfo.HwVersion != null)
        {
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "hwversion", _ => _deviceInfo.HwVersion, profiles: _parameterProfile, cacheTimeout: timeout);
        }

        if (_deviceInfo.FieldbusType != null)
        {
            var valueList = new Dictionary<string, string>
            {
                {"0" , "Profinet"},
                {"1" , "reserved"},
                {"2" , "EtherNet/IP" },
                {"3" , "EtherCAT" },
                {"4" , "Modbus-TCP"},
                {"5" , "Internet of Things"},
                {"6" , "ASi"},
                {"7" , "POWERLINK"}
            };
            _elementManager.CreateReadOnlyDataElement(deviceInfoElement, "fieldbustype", _ => _deviceInfo.FieldbusType, profiles: _parameterProfile, cacheTimeout: timeout, format: new IntegerEnumFormat(new IntegerEnumValuation(valueList)));
        }

    }
}