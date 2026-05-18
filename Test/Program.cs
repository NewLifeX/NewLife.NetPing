using NewLife.IoT.Drivers;
using NewLife.IoT.ThingModels;
using NewLife.Log;
using NewLife.NetPing.Drivers;

XTrace.UseConsole();

var driver = new NetPingDriver();
var pm = new NetPingParameter();

//var points = driver.GetDefaultPoints();
//foreach (var item in points)
//{
//    XTrace.WriteLine("{0}={1}", item.Name, item.Address);
//}
var spec = driver.GetSpecification();
XTrace.WriteLine(spec.ToJson());

var node = driver.Open(null, pm);

var point = new PointModel
{
    Name = "newlife",
    Address = "newlifex.com",
    Type = "Int32",
};
var point2 = new PointModel
{
    Name = "google",
    Address = "google.com",
    Type = "Int32",
};

var result = driver.Read(node, new[] { point, point2 });
for (var i = 0; i < result.Points.Length; i++)
{
    XTrace.WriteLine("{0}\t= {1}", result.Points[i].Name, result.Values[i]);
}

driver.Close(node);