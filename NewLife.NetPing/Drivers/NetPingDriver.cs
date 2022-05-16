using System.ComponentModel;
using System.Net.NetworkInformation;
using NewLife.IoT;
using NewLife.IoT.Drivers;
using NewLife.IoT.ThingModels;
using NewLife.Serialization;

namespace NewLife.NetPing.Drivers;

/// <summary>
/// 欧姆龙PLC驱动
/// </summary>
[Driver("NetPing")]
[DisplayName("设备网络心跳")]
public class NetPingDriver : DisposeBase, IDriver
{
    #region 方法
    /// <summary>
    /// 创建驱动参数对象，可序列化成Xml/Json作为该协议的参数模板
    /// </summary>
    /// <returns></returns>
    public virtual IDriverParameter CreateParameter() => new NetPingParameter();

    /// <summary>
    /// 打开通道。一个ModbusTcp设备可能分为多个通道读取，需要共用Tcp连接，以不同节点区分
    /// </summary>
    /// <param name="device">通道</param>
    /// <param name="parameters">参数</param>
    /// <returns></returns>
    public virtual INode Open(IDevice device, IDictionary<String, Object> parameters)
    {
        var pm = JsonHelper.Convert<NetPingParameter>(parameters);

        var node = new NetPingNode
        {
            Driver = this,
            Device = device,
            Parameter = pm,
        };

        return node;
    }

    /// <summary>
    /// 关闭设备驱动
    /// </summary>
    /// <param name="node"></param>
    public void Close(INode node) { }

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="node">节点对象，可存储站号等信息，仅驱动自己识别</param>
    /// <param name="points">点位集合，Address属性地址示例：D100、C100、W100、H100</param>
    /// <returns></returns>
    public virtual IDictionary<String, Object> Read(INode node, IPoint[] points)
    {
        var dic = new Dictionary<String, Object>();

        if (points == null || points.Length == 0) return dic;

        var p = node.Parameter as NetPingParameter;
        foreach (var point in points)
        {
            var ping = new Ping();
            var reply = ping.Send(point.Address, p.Timeout);
            if (reply.Status == IPStatus.Success)
                dic[point.Name] = reply.RoundtripTime;
        }

        return dic;
    }

    /// <summary>
    /// 写入数据
    /// </summary>
    /// <param name="node">节点对象，可存储站号等信息，仅驱动自己识别</param>
    /// <param name="point">点位，Address属性地址示例：D100、C100、W100、H100</param>
    /// <param name="value">数据</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public virtual Object Write(INode node, IPoint point, Object value) => throw new NotImplementedException();

    /// <summary>
    /// 控制设备，特殊功能使用
    /// </summary>
    /// <param name="node"></param>
    /// <param name="parameters"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Control(INode node, IDictionary<String, Object> parameters) => throw new NotImplementedException();
    #endregion
}