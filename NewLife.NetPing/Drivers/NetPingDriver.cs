using System.ComponentModel;
using System.Net.NetworkInformation;
using NewLife.IoT.Drivers;
using NewLife.IoT.ThingModels;
using NewLife.IoT.ThingSpecification;

namespace NewLife.NetPing.Drivers;

/// <summary>
/// 设备网络心跳驱动
/// </summary>
/// <remarks>
/// IoT驱动，通过Ping探测到目标设备的网络情况，并收集延迟数据
/// </remarks>
[Driver("NetPing")]
[DisplayName("设备网络心跳")]
public class NetPingDriver : DriverBase<Node, NetPingParameter>
{
    #region 方法
    /// <summary>读取数据</summary>
    /// <param name="node">节点对象，可存储站号等信息，仅驱动自己识别</param>
    /// <param name="points">点位集合，Address属性地址示例：D100、C100、W100、H100</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>读取结果，包含点位数据、质量码和可选诊断帧</returns>
    public override Task<ReadResult> ReadAsync(INode node, IPoint[] points, CancellationToken cancellationToken = default)
    {
        if (points == null || points.Length == 0)
            return TaskEx.FromResult(ReadResult.Success([], []));

        var p = node.Parameter as NetPingParameter;
        var resultPoints = new List<IPoint>();
        var resultValues = new List<Object?>();

        foreach (var point in points)
        {
            if (!point.Address.IsNullOrEmpty())
            {
                try
                {
                    var reply = new Ping().Send(point.Address, p.Timeout);
                    if (reply.Status == IPStatus.Success)
                    {
                        resultPoints.Add(point);
                        resultValues.Add(reply.RoundtripTime);
                    }
                    if (p.RetrieveStatus)
                    {
                        resultPoints.Add(point);
                        resultValues.Add(reply.Status + "");
                    }
                }
                catch (Exception ex)
                {
                    resultPoints.Add(point);
                    resultValues.Add(ex.GetTrue().Message);
                }
            }
        }

        return TaskEx.FromResult(ReadResult.Success(resultPoints.ToArray(), resultValues.ToArray()));
    }

    /// <summary>填充产品物模型（发现本地网关和DNS点位）</summary>
    /// <param name="thingSpec">待填充的物模型对象</param>
    /// <returns>是否填充成功</returns>
    protected override Boolean OnGetSpecification(ThingSpec thingSpec)
    {
        var points = new List<PropertySpec>();

        // 所有网关地址和DNS地址
        var gaddrs = new List<String>();
        var daddrs = new List<String>();
        var gi = 1;
        var di = 1;
        foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
        {
            var ipps = item.GetIPProperties();
            foreach (var elm in ipps.GatewayAddresses)
            {
                var ip = elm.Address + "";
                if (!gaddrs.Contains(ip))
                {
                    var name = "Gateway";
                    if (gi > 1) name += gi++;
                    var ps = PropertySpec.Create(name, $"{item.Name}网关", "int", 0, ip);
                    ps.DataType.Specs = new DataSpecs { Unit = "ms", UnitName = "毫秒" };
                    points.Add(ps);
                    gaddrs.Add(ip);
                }
            }
            foreach (var elm in ipps.DnsAddresses)
            {
                if (!elm.IsIPv4()) continue;

                var ip = elm + "";
                if (!daddrs.Contains(ip))
                {
                    var name = "Dns";
                    if (di > 1) name += di++;
                    var ps = PropertySpec.Create(name, $"{item.Name}DNS", "int", 0, ip);
                    ps.DataType.Specs = new DataSpecs { Unit = "ms", UnitName = "毫秒" };
                    points.Add(ps);
                    daddrs.Add(ip);
                }
            }
        }

        thingSpec.Properties = points.ToArray();

        return points.Count > 0;
    }
    #endregion
}