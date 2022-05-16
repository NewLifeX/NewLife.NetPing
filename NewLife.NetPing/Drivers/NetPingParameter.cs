using System.ComponentModel;
using NewLife.IoT.Drivers;

namespace NewLife.NetPing.Drivers;

/// <summary>NetPing参数</summary>
public class NetPingParameter : IDriverParameter
{
    /// <summary>超时。指定（发送回送消息后）等待 ICMP 回送答复消息的最大毫秒数，默认5000ms</summary>
    [Description("超时。指定（发送回送消息后）等待 ICMP 回送答复消息的最大毫秒数，默认5000ms")]
    public Int32 Timeout { get; set; } = 5000;
}