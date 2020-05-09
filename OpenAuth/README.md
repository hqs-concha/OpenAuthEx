
### 简介

该项目为我Java项目 OpenAuth 的 .net core上的授权扩展， 用于获取授权服务器下发的token

### 使用

> 在 `ConfigureServices` 中写如一下内容

```
services.AddOpenAuth(options => { 
    options.Authority = "http://oauth.com";
    options.ClientId = "clientId";
    options.ClientSecret = "clientSecret";
    options.ClientName = "clientName";
});
```

> 在 `Configure` 添加以下中间件，添加之前请删除掉 `app.UseAuthorization()`

```
app.UseOpenAuth();
```

### 内置地址

1. 获取token `http(s)://host:port/oauth/connect`
2. 刷新token `http(s)://host:port/oauth/refresh-token`
3. 每个请求都将验证token是否有效

