### Filter

```
Validate		模型验证过滤器
在实体类中加验证注解即可

services.AddControllers(options => options.Filters.Add(typeof(ValidateFilter)))
        .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
全局配置，需要将 SuppressModelStateInvalidFilter 设为 true

```

```
AntiReplay		防重放过滤器	（需要使用MemoryCache）
请求头验证
X-CA-Key		客户端Id
X-CA-NONCE		随机字符串
X-CA-TIMESTAMP	时间戳（10分钟内的同一个nonce会禁止请求）
X-CA-SIGNATURE	签名（参数+key按照a-z排序，去掉key值为空和null，转化为字符串，然后再md5加密）
例：
url: http://api.com/getlist?name=jimmy&type=
key: ajuych23421jksdkf
body: {"age":18,"class":"three","game":""}
转化为字符串为：age18classthreekeyajuych23421jksdkfnamejimmy
md5：0d727e4105b0fb9bd26642cef115c923 （32位，不区分大小写）
```

### Attribute

```
IgnoreVerify			忽略模型验证
```

```
IgnoreAntiReplay		忽略防重放
```

### Middleware

```
ExceptionMiddleware						全局异常处理
Exception is CustomException			自定义异常
```

