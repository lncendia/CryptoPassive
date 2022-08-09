using Autofac;
using CryptoPassive;
using CryptoPassive.BLL.Services;
using CryptoPassive.Core.Services;
using CryptoPassive.Infrastructure;
using CryptoPassive.Infrastructure.Services;
using IContainer = Autofac.IContainer;

const string logFile = "log.txt", logoName = "logo.png";
await using var scope = CompositionRoot().BeginLifetimeScope();
var app = scope.Resolve<Application>();
await app.Run();

IContainer CompositionRoot()
{
    var builder = new ContainerBuilder();
    builder.RegisterType<Application>().WithParameter("fileName", logFile).WithParameter("logoName", logoName);
    builder.RegisterType<MnemonicService>().As<IMnemonicService>();
    builder.RegisterType<BalanceChecker>().As<IBalanceChecker>();
    builder.RegisterType<BalanceManager>().As<IBalanceManager>();
    builder.RegisterType<AuthenticationManager>().As<IAuthenticationManager>();
    builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
    builder.RegisterType<LoggerService>().As<ILoggerService>().WithParameter("fileName", logFile);
    builder.RegisterType<FileLoggerService>().As<IFileLoggerService>();
    builder.RegisterType<ApplicationDbContext>();
    return builder.Build();
}