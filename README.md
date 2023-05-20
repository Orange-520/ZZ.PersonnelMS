### ZZ.Commons

提供公共的方法



### ZZ.DomainCommons

提供公共的接口，实体类选择去实现



### ZZ.Domain

领域层，存放实体类、选择限定范围的枚举、要实现的仓储接口`IRepository.cs`。

备注：由于时间暂时比较匆忙，而且不熟练此开发方式，所以我是先写`Repository.cs`，后期再把`Repository.cs`中的方法，定义到 `IRepository.cs` 中，再让`Repository.cs`继承`IRepository.cs` 。

`Service.cs`类属于领域服务，只有抽象的业务逻辑，由于我的代码功底不够深，暂时领略不到这个类的魅力。



### ZZ.Infrastructure

基础设施层，负责实体类的配置、实现仓储接口。



### ZZ.WebAPI

应用层，组合前两个，最终落地。