using Aivis.Speakers;

namespace Aivis.Tests.Speakers;

public class DeviceTests
{
    [Fact]
    public void ListPlaybackDevices_ReturnsEnumerable()
    {
        var devices = Device.ListPlaybackDevices();
        
        Assert.NotNull(devices);
        // Note: We can't test the actual device list contents as it depends on the system
    }

    [Fact]
    public void DefaultPlaybackDevice_ReturnsString()
    {
        var defaultDevice = Device.DefaultPlaybackDevice();
        
        Assert.NotNull(defaultDevice);
        // Note: The actual value depends on the system configuration
    }
}