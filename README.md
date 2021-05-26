# AllLive

使用.NET实现的聚合直播。

## AllLive.Core

项目核心，.NET Standard 2.0类库。用于获取各个网站的信息及弹幕实现。

## AllLive.ConsoleApp

基于AllLive.Core的控制台程序。

输入直播间链接获取信息及播放地址：

```
AllLive.ConsoleApp -i [URL]
```

输入直播间链接获取弹幕：

```
AllLive.ConsoleApp -d [URL]
```

## AllLive.UWP

UWP客户端，可以在线观看各个网站直播。


## 参考及引用
[https://github.com/wbt5/real-url](https://github.com/wbt5/real-url)

[https://github.com/lovelyyoshino/Bilibili-Live-API/blob/master/API.WebSocket.md](https://github.com/lovelyyoshino/Bilibili-Live-API/blob/master/API.WebSocket.md)

[https://github.com/IsoaSFlus/danmaku](https://github.com/IsoaSFlus/danmaku)

[https://www.cnblogs.com/sdflysha/p/20210117-douyu-barrage-with-dotnet.html](https://www.cnblogs.com/sdflysha/p/20210117-douyu-barrage-with-dotnet.html)

[https://github.com/BacooTang/huya-danmu](https://github.com/BacooTang/huya-danmu)

[https://github.com/TarsCloud/Tars](https://github.com/TarsCloud/Tars)