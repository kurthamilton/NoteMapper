using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NoteMapper.Core.Users;
using NoteMapper.Data.Core.Users;
using NoteMapper.Data.Sql;
using NoteMapper.Data.Sql.Repositories.Users;
using NoteMapper.Emails;
using NoteMapper.Identity;
using NoteMapper.Identity.Microsoft;
using NoteMapper.Infrastructure.Extensions;
using NoteMapper.Services;
using NoteMapper.Services.Emails;
using NoteMapper.Services.Users;
using NoteMapper.Services.Web;

namespace NoteMapper.Infrastructure
{
    public static class DependencyConfig
    {
        public static void RegisterDependencies(IDependencyContainer container, IConfiguration config)
        {
            RegisterData(container, config);
            RegisterIdentity(container, config);
            RegisterServices(container, config);
        }

        private static void RegisterData(IDependencyContainer container, IConfiguration config)
        {            
            container
                .AddSingleton(new NoteMapperContextSettings
                {
                    ConnectionString = config.GetConnectionString("note-mapper") ?? ""
                })
                .AddScoped<NoteMapperContext>()
                .AddScoped<IUserActivationRepository, UserActivationSqlRepository>()
                .AddScoped<IUserLoginTokenRepository, UserLoginTokenSqlRepository>()
                .AddScoped<IUserPasswordRepository, UserPasswordSqlRepository>()
                .AddScoped<IUserRepository, UserSqlRepository>();
        }

        private static void RegisterIdentity(IDependencyContainer container, IConfiguration config)
        {
            string baseUrl = config.GetValue("BaseUrl");

            container
                .AddScoped<IIdentityService, MicrosoftIdentityService>()
                .AddSingleton(new MicrosoftIdentityServiceSettings
                {
                    ActivationCodeExpiresAfterMinutes = config.GetInt("Account.ActivationCodeExpiresAfterMinutes"),
                    ActivationUrl = baseUrl + config.GetValue("Account.ActivationUrl"),
                    LoginTokenExpiresAfterSeconds = config.GetInt("Account.LoginTokenExpiresAfterSeconds"),
                    PasswordResetCodeExpiresAfterHours = config.GetInt("Account.PasswordResetCodeExpiresAfterHours"),
                    PasswordResetUrl = baseUrl + config.GetValue("Account.PasswordResetUrl"),
                    RegistrationType = config.GetEnum<RegistrationType>("Account.RegistrationType")
                })
                .AddScoped<IPasswordHasher, CustomPasswordHasher>()
                .AddSingleton(new CustomPasswordHasherSettings
                {
                    HashByteSize = config.GetInt("Security.Passwords.HashByteSize"),
                    HashIterations = config.GetInt("Security.Passwords.HashIterations"),
                    SaltByteSize = config.GetInt("Security.Passwords.SaltByteSize")
                })
                .AddScoped<IUserStore<IdentityUser>, CustomUserStore>();
        }

        private static void RegisterServices(IDependencyContainer container, IConfiguration config)
        {
            container
                .AddScoped<IEmailSenderService, MailjetEmailSenderService>()
                .AddSingleton(new MailjetEmailSenderServiceSettings
                {
                    ApiKey = config.GetValue("Emails.Mailjet.ApiKey"),
                    ApiSecret = config.GetValue("Emails.Mailjet.ApiSecret"),
                    FromEmail = config.GetValue("Emails.Mailjet.FromEmail"),
                    FromName = config.GetValue("Emails.Mailjet.FromName"),
                });

            container
                .AddScoped<IInstrumentFactory, InstrumentFactory>()
                .AddScoped<IMusicTheoryService, MusicTheoryService>();

            container
                .AddScoped<INoteMapViewModelService, NoteMapViewModelService>();
        }
    }
}
