using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NETFX_CORE
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

#endif

public class BluetoothWatcher : MonoBehaviour {

    public static BluetoothWatcher instance;
    public delegate void ValueReceivedDelegate(string value);
    public ValueReceivedDelegate ValueReceived;
    public string lastValueReceived = "";

#if NETFX_CORE
        List<DeviceInformation> filteredDevices = new List<DeviceInformation>();
        DeviceInformation device;
        BluetoothLEDevice leDevice;
        RingSensor ringSensor;
#endif

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    void Start () {

#if NETFX_CORE
        // Initialize Device Picker
        UnityEngine.Debug.Log("Test for Build");
        OnDeviceSelected();
#endif

    }

    public void OnValueReceived(string value)
    {
        UnityEngine.Debug.Log("OnValueReceived Called with " + value);

        if(ValueReceived != null)
        {
            ValueReceived.Invoke(value);
        }
    }

#if NETFX_CORE
    private async void OnDeviceSelected()
    {

        // Try to just get the list of available devices
        device = await enumerateSnapshot();

        UnityEngine.Debug.Log("OnDeviceSelected Called");

        // Assign the BluetoothLEDevice object
        UnityEngine.Debug.Log("First device in filtered list is ID: " + device.Id);
        leDevice = await BluetoothLEDevice.FromIdAsync(device.Id);
        UnityEngine.Debug.Log(leDevice.DeviceId);

        var services = await leDevice.GetGattServicesAsync();
        GattDeviceService selectedService = null;
        foreach (var service in services.Services)
        {
            // TODO: Fix this - right now it's randomly going through and assigning services
            // You'll want to actually filter here
            UnityEngine.Debug.Log("Found a service: " + service.Uuid);
            selectedService = service;
        }

        InitializeRingSensor(selectedService);
    }


        async Task<DeviceInformation> enumerateSnapshot()
        {
            // select only paired bluetooth devices
            
            UnityEngine.Debug.Log("enumerateSnapshot called");
            DeviceInformationCollection collection = await DeviceInformation.FindAllAsync();
            UnityEngine.Debug.Log("number of devices in collection: " + collection.Count);
            foreach(DeviceInformation d in collection)
            {
                if(d.Pairing.IsPaired == true)
                {
                    UnityEngine.Debug.Log("Pairing status is " + d.Pairing.IsPaired);
                    UnityEngine.Debug.Log("Paired Device ID: " + d.Id);
                    object itemNameDisplay;
                    d.Properties.TryGetValue("System.ItemNameDisplay", out itemNameDisplay);
                    string itemNameDisplayString = itemNameDisplay.ToString();
                    UnityEngine.Debug.Log("Paired device is called " + itemNameDisplayString);

                    if (itemNameDisplayString == "Arduino" || itemNameDisplayString == "UART")
                    {
                        UnityEngine.Debug.Log("This device matches filter string and is added to filteredDevices");
                        filteredDevices.Add(d);
                    }
                }
            }

            return filteredDevices[0];
        }


    protected async void InitializeRingSensor(GattDeviceService service)
    {
        UnityEngine.Debug.Log("InitializeRingSensor Called");

        ringSensor = new RingSensor(service);
        await ringSensor.EnableNotifications();
    }

#endif

}

// SensorBase and RingSensor classes
#if NETFX_CORE
public class SensorBase : IDisposable
{
    protected GattDeviceService deviceService;
    protected string sensorDataUuid;
    protected byte[] data;
    protected bool isNotificationSupported = false;
    private GattCharacteristic dataCharacteristic;

    public SensorBase(GattDeviceService dataService, string sensorDataUuid)
    {
        UnityEngine.Debug.Log("SensorBase Initialized");
        this.deviceService = dataService;
        this.sensorDataUuid = sensorDataUuid;
    }

    public virtual async Task EnableNotifications()
    {
        UnityEngine.Debug.Log("EnableNotifications Called");
        isNotificationSupported = true;

        UnityEngine.Debug.Log("Attempting to enable notifications for " + sensorDataUuid);

        dataCharacteristic = (await deviceService.GetCharacteristicsForUuidAsync(
            new Guid(sensorDataUuid))).Characteristics[0];
        dataCharacteristic.ValueChanged += dataCharacteristic_ValueChanged;
        GattCommunicationStatus status =
            await dataCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                GattClientCharacteristicConfigurationDescriptorValue.Notify);
    }

    public virtual async Task DisableNotifications()
    {
        UnityEngine.Debug.Log("DisableNotifications Called");
        isNotificationSupported = false;
        dataCharacteristic = (await deviceService.GetCharacteristicsForUuidAsync(
            new Guid(sensorDataUuid))).Characteristics[0];
        dataCharacteristic.ValueChanged -= dataCharacteristic_ValueChanged;
        GattCommunicationStatus status =
            await dataCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
            GattClientCharacteristicConfigurationDescriptorValue.None);
    }

    protected async Task<byte[]> ReadValue()
    {
        UnityEngine.Debug.Log("ReadValue Called");
        if (!isNotificationSupported)
        {
            if (dataCharacteristic == null)
                dataCharacteristic = (await deviceService.GetCharacteristicsForUuidAsync(
                    new Guid(sensorDataUuid))).Characteristics[0];
            GattReadResult readResult =
                await dataCharacteristic.ReadValueAsync(BluetoothCacheMode.Uncached);
            data = new byte[readResult.Value.Length];
            DataReader.FromBuffer(readResult.Value).ReadBytes(data);
        }
        return data;
    }

    private void dataCharacteristic_ValueChanged(GattCharacteristic sender,
        GattValueChangedEventArgs args)
    {
        // UnityEngine.Debug.Log("dataCharacteristic_ValueChanged Called");
        data = new byte[args.CharacteristicValue.Length];
        DataReader.FromBuffer(args.CharacteristicValue).ReadBytes(data);
        // string dataValue = ((string)BitConverter.ToString(data));
        // UnityEngine.Debug.Log(dataValue);
        string dataASCII = System.Text.Encoding.ASCII.GetString(data);
        UnityEngine.Debug.Log(dataASCII);

        BluetoothWatcher.instance.OnValueReceived(dataASCII);
        BluetoothWatcher.instance.lastValueReceived = dataASCII;

    }

    public async void Dispose()
    {
        await DisableNotifications();
    }

}


public class RingSensor: SensorBase
{

    public RingSensor(GattDeviceService dataService) : base (dataService, "6e400003-b5a3-f393-e0a9-e50e24dcca9e")
    {
        UnityEngine.Debug.Log("RingSensor Called");

    }

}

#endif
