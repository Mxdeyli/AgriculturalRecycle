# AgriculturalRecycle

使用 Windows、.NET Framework 4.7.2 和 MySQL。用 Visual Studio 打开 `AgriculturalRecycle.sln`，还原 NuGet 包后构建。

启动前，将进程环境变量 `AGRICULTURALRECYCLE_CONNECTION_STRING` 设为现有项目数据库的 MySQL 连接字符串。仓库没有原始数据库的建库脚本。租赁功能涉及 `borrowequipment`、`borrowrecords`、`userinfo` 和 `userequipment`，这些表需要使用 InnoDB，应用才能在同一事务中提交借用、扣款和归还。
