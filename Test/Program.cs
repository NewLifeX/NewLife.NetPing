﻿using NewLife.IoT.Drivers;
using NewLife.IoT.ThingModels;
using NewLife.Log;
using NewLife.NetPing.Drivers;

XTrace.UseConsole();

var driver = new NetPingDriver();
var pm = new NetPingParameter();
var node = driver.Open(null, pm);

var point = new PointModel
{
    Name = "newlife",
    Address = "newlifex.com",
    Type = "Int32",
};
var point2 = new PointModel
{
    Name = "feifan",
    Address = "iot.feifan.link",
    Type = "Int32",
};

var dic = driver.Read(node, new[] { point, point2 });
foreach (var item in dic)
{
    XTrace.WriteLine("{0}\t= {1}", item.Key, item.Value);
}

driver.Close(node);