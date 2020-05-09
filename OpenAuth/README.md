
### ���

����ĿΪ��Java��Ŀ OpenAuth �� .net core�ϵ���Ȩ��չ�� ���ڻ�ȡ��Ȩ�������·���token

### ʹ��

> �� `ConfigureServices` ��д��һ������

```
services.AddOpenAuth(options => { 
    options.Authority = "http://oauth.com";
    options.ClientId = "clientId";
    options.ClientSecret = "clientSecret";
    options.ClientName = "clientName";
});
```

> �� `Configure` ��������м�������֮ǰ��ɾ���� `app.UseAuthorization()`

```
app.UseOpenAuth();
```

### ���õ�ַ

1. ��ȡtoken `http(s)://host:port/oauth/connect`
2. ˢ��token `http(s)://host:port/oauth/refresh-token`
3. ÿ�����󶼽���֤token�Ƿ���Ч

