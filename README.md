# OpenAuthEx
认证

```
            services.AddOpenAuth(options =>
            {
                options.Authority = "http://localhost:7000";
                options.ClientName = "OAuthClientApi";
                options.ClientId = "042c67a73b3611ea8fdb00163e0a23b0";
                options.ClientSecret = "113ee66b3b3611ea8fdb00163e0a23b0";
            });
```

```
            app.UseOpenAuth();
```