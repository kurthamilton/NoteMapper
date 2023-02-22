using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NoteMapper.Core.Users;
using NoteMapper.Data.Core.Contact;
using NoteMapper.Data.Core.Errors;
using NoteMapper.Data.Core.Instruments;
using NoteMapper.Data.Core.Questionnaires;
using NoteMapper.Data.Core.Users;
using NoteMapper.Data.Cosmos;
using NoteMapper.Data.Cosmos.Repositories;
using NoteMapper.Data.Sql.Repositories;
using NoteMapper.Data.Sql.Repositories.Contact;
using NoteMapper.Data.Sql.Repositories.Errors;
using NoteMapper.Data.Sql.Repositories.Questionnaires;
using NoteMapper.Data.Sql.Repositories.Users;
using NoteMapper.Emails;
using NoteMapper.Identity;
using NoteMapper.Identity.Microsoft;
using NoteMapper.Infrastructure.Extensions;
using NoteMapper.Services;
using NoteMapper.Services.Emails;
using NoteMapper.Services.Instruments;
using NoteMapper.Services.Logging;
using NoteMapper.Services.Questionnaires;
using NoteMapper.Services.Users;
using NoteMapper.Services.Web.Contact;
using NoteMapper.Services.Web.Instruments;
using NoteMapper.Services.Web.NoteMap;
using NoteMapper.Services.Web.Questionnaires;
using NoteMapper.Services.Web.Security;
using NoteMapper.Services.Web.StateManagement;

namespace NoteMapper.Infrastructure
{
    public static class DependencyConfig
    {
        public static void RegisterDependencies(IDependencyContainer container, IConfiguration config)
        {
            RegisterData(container, config);
            RegisterIdentity(container, config);
            RegisterServices(container, config);
            RegisterWebServices(container, config);
        }

        private static void RegisterData(IDependencyContainer container, IConfiguration config)
        {
            container
                .AddSingleton(new SqlRepositorySettings
                {
                    ConnectionString = config.GetConnectionString("note-mapper-sql") ?? "",
                    CurrentEnvironment = config.GetEnum<ApplicationEnvironment>("Environment")
                })
                .AddScoped<IApplicationErrorRepository, ApplicationErrorSqlRepository>()
                .AddScoped<IContactRepository, ContactSqlRepository>()
                .AddScoped<IQuestionnaireQuestionRepository, QuestionnaireQuestionSqlRepository>()
                .AddScoped<IQuestionnaireRepository, QuestionnaireSqlRepository>()
                .AddScoped<IRegistrationCodeRepository, RegistrationCodeRepository>()
                .AddScoped<IUserActivationRepository, UserActivationSqlRepository>()
                .AddScoped<IUserLoginTokenRepository, UserLoginTokenSqlRepository>()
                .AddScoped<IUserPasswordRepository, UserPasswordSqlRepository>()
                .AddScoped<IUserPasswordResetCodeRepository, UserPasswordResetCodeSqlRepository>()
                .AddScoped<IUserPreferenceRepository, UserPreferenceSqlRepository>()
                .AddScoped<IUserQuestionResponseRepository, UserQuestionResponseSqlRepository>()
                .AddScoped<IUserRegistrationCodeRepository, UserRegistrationCodeSqlRepository>()
                .AddScoped<IUserRepository, UserSqlRepository>();

            container
                .AddSingleton(new AzureCosmosRepositorySettings
                {
                    ApplicationName = config.GetValue("Application.Name"),
                    ConnectionString = config.GetConnectionString("note-mapper-cosmos") ?? "",
                    CurrentEnvironment = config.GetEnum<ApplicationEnvironment>("Environment"),
                    DatabaseId = config.GetValue("Data.Azure.CosmosDB.DatabaseId"),
                    DefaultUserId = config.GetValue("Data.Azure.CosmosDB.DefaultUserId")
                })
                .AddScoped<IUserInstrumentRepository, UserInstrumentAzureCosmosRepository>();
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
                    ApplicationName = config.GetValue("Application.Name"),
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
                .AddScoped<IErrorLoggingService, ErrorLoggingService>()
                .AddSingleton(new ErrorLoggingServiceSettings
                {
                    CurrentEnvironment = config.GetEnum<ApplicationEnvironment>("Environment"),
                    Enabled = config.GetBool("Logging.Enabled")
                })
                .AddScoped<IQuestionnaireService, QuestionnaireService>()
                .AddScoped<IUserAdminService, UserAdminService>()
                .AddScoped<IUserService, UserService>();

            container
                .AddScoped<IInstrumentFactory, InstrumentFactory>()
                .AddScoped<IMusicTheoryService, MusicTheoryService>()
                .AddScoped<IRecaptchaService, RecaptchaService>()
                .AddSingleton(new RecaptchaServiceSettings(
                    config.GetValue("Recaptcha.SecretKey"),
                    config.GetValue("Recaptcha.SiteKey"), 
                    config.GetValue("Recaptcha.VerifyUrl")))
                .AddScoped<IUserInstrumentService, UserInstrumentService>()
                .AddScoped<IUserInstrumentViewModelService, UserInstrumentViewModelService>();
        }

        private static void RegisterWebServices(IDependencyContainer container, IConfiguration config)
        {
            container
                .AddScoped<IContactService, ContactService>()
                .AddSingleton(new ContactServiceSettings
                {
                    ApplicationName = config.GetValue("Application.Name"),
                    ContactEmailAddress = config.GetValue("Contact.EmailAddress"),
                    Enabled = config.GetBool("Contact.Enabled")
                })
                .AddScoped<INoteMapViewModelService, NoteMapViewModelService>()
                .AddScoped<IQuestionnaireViewModelService, QuestionnaireViewModelService>()
                .AddSingleton(new QuestionnaireViewModelServiceSettings
                {
                    ApplicationName = config.GetValue("Application.Name"),
                    NotificationEmailAddress = config.GetValue("Contact.EmailAddress"),
                });

            container.AddSingleton<IStateContainer>(new StateContainer());
        }
    }
}
