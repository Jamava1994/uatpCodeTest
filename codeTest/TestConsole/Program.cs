// See https://aka.ms/new-console-template for more information
UniversalFeesExchange.Sdk.UniversalFeesExchange universalFeesExchange = UniversalFeesExchange.Sdk.UniversalFeesExchange.GetInstance();
var a = await universalFeesExchange.GetCurrentFeePriceAsync();
Console.WriteLine(a);
Thread.Sleep(2000);
Console.WriteLine(a);
