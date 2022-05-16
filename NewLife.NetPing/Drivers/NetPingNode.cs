namespace NewLife.IoT.Drivers;

/// <summary>
/// 节点
/// </summary>
public class NetPingNode : INode
{
    /// <summary>通道</summary>
    public IDriver Driver { get; set; }

    /// <summary>设备</summary>
    public IDevice Device { get; set; }

    /// <summary>参数</summary>
    public IDriverParameter Parameter { get; set; }
}