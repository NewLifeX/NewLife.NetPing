﻿using System.ComponentModel;
using NewLife.IoT.Drivers;

namespace NewLife.NetPing.Drivers;

/// <summary>NetPing参数</summary>
public class NetPingParameter : IDriverParameter
{
    /// <summary>超时。指定（发送回送消息后）等待 ICMP 回送答复消息的最大毫秒数，默认5000ms</summary>
    [Description("超时。指定（发送回送消息后）等待 ICMP 回送答复消息的最大毫秒数，默认5000ms")]
    public Int32 Timeout { get; set; } = 5000;

    /// <summary>采集状态。采集网络状态，在Ping失败时能够获取准确的失败原因，默认true</summary>
    [Description("采集状态。采集网络状态，在Ping失败时能够获取准确的失败原因，默认true")]
    public Boolean RetrieveStatus { get; set; } = true;
}