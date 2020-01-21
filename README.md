# OpenAuthEx
该项目为我Java项目 [OpenAuth](https://github.com/hqs666666/OpenAuth) 的 .net core上的授权扩展，
用于获取授权服务器下发的token

 添加包 `OpenAuth`

> 在 `ConfigureServices` 中写如一下内容

```csharp
services.AddOpenAuth(options => { 
    options.Authority = Configuration["OpenAuth:Authority"];
    options.ClientId = Configuration["OpenAuth:ClientId"];
    options.ClientSecret = Configuration["OpenAuth:ClientSecret"];
    options.ClientName = Configuration["OpenAuth:ClientName"];
});
```

> 在 `Configure` 添加以下中间件，添加之前请删除掉 `app.UseAuthorization();`

```csharp
app.UseOpenAuth();
```