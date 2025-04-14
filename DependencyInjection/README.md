# 依存性の注入

## Asp.NET Coreでの依存性注入
https://learn.microsoft.com/ja-jp/training/modules/configure-dependency-injection/

- WebApplicationはPersonServiceに依存している
  
![webapp-classdiagram](../image/di-02.png)

- WebApplicationはインターフェースIPersonServiceに依存している
- PresonServiceもインターフェースPersonServiceに依存している
  
![webapp-di-classdiagram](../image/di-01.png)


```cs
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IPersonService, PersonService>();

var app = builder.Build();
app.MapGet(
    pattern: "/", 
    //handler type: Func<IPersonService, string>
    handler: (IPersonService personService) =>
    {
        return $"Hello, {personService.GetPersonName()}!";
    });

app.Run();


interface IPersonService 
{
    void GetPersonName();
}
class PersonService(string Name) : IPersonService
{
    public void GetPersonName()
    {
        return Name;
    }
}

class WebApplication 
{
    private IServiceCollection services;
    private Dictionary<string,Delegate> actions = new();
    
    public void MapGet(string: pattern, Delegate handler)
    {
        actions.Add(pattern, handler);
    }

    public string Get(string pattern)
    {
        if(actions.TryGetValue(pattern, out ver hander))
        {
            // Reflectionでhandlerの引数の型を取得
            var pType = handler.Method.GetParameters().First().ParameterType;
            // その型のインスタンスをservicesから取得する
            var serviceInstance = services.Get(pType); 
            
            //取得したインスタンスをhandlerに渡して実行
            return handler(serviceInstance);
        }
    }
}
```

## DIコンテナの簡易実装

サンプルコード: [Program.cs](./Program.cs)

![di-container-sample](../image/di-03.png)

