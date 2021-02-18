using System;
using System.Collections.Generic;
using System.IO;
using Curiosity.Configuration;
using Curiosity.Hosting.Web;

namespace Heimdal.Configuration
{
    public class HeimdalConfiguration : CuriosityWebAppConfiguration
    {
        public UserOption[] Users { get; set; } = Array.Empty<UserOption>();

        public ResourceOption[] Resources { get; set; } = Array.Empty<ResourceOption>();

        public override IReadOnlyCollection<ConfigurationValidationError> Validate(string? prefix = null)
        {
            var errors = new ConfigurationValidationErrorCollection(prefix);
            errors.AddErrors(base.Validate(prefix));

            var usedUserLogins = new HashSet<string>();
            for (var i = 0; i < Users.Length; i++)
            {
                var userOption = Users[i];
                var userErrors = userOption.Validate($"{nameof(Users)}-{i}`");
                if (errors.Count > 0)
                {
                    errors.AddErrors(userErrors);
                    continue;
                }

                var lowerLogin = userOption.Login.ToLower();
                if (usedUserLogins.Contains(lowerLogin))
                {
                    errors.AddError($"{nameof(Users)}-{i}", $"{userOption.Login} is already in use");
                    continue;
                }

                usedUserLogins.Add(lowerLogin);
            }

            var usedRoutes = new HashSet<string>();
            for (var i = 0; i < Resources.Length; i++)
            {
                var resourceOption = Resources[i];
                var resourceErrors = resourceOption.Validate($"{nameof(Resources)}-{i}");
                if (resourceErrors.Count > 0)
                {
                    errors.AddErrors(resourceErrors);
                    continue;
                }

                var lowerResourcePath = resourceOption.Route.ToLower();
                if (usedRoutes.Contains(lowerResourcePath))
                {
                    errors.AddError($"{nameof(Resources)}-{i}", $"{resourceOption.Route} is already in use");
                    continue;
                }

                usedRoutes.Add(lowerResourcePath);
            }

            return errors;
        }
    }

    public class UserOption : ILoggableOptions, IValidatableOptions
    {
        public string Login { get; set; } = null!;
        
        public string PasswordHash { get; set; } = null!;
        
        public IReadOnlyCollection<ConfigurationValidationError> Validate(string? prefix = null)
        {
            var errorCollection = new ConfigurationValidationErrorCollection(prefix);
            
            errorCollection.AddErrorIf(String.IsNullOrWhiteSpace(Login), nameof(Login), "can't be empty");
            errorCollection.AddErrorIf(String.IsNullOrWhiteSpace(PasswordHash), nameof(PasswordHash), "can't be empty");

            return errorCollection;
        }
    }
    
    /// <summary>
    /// Options for single resource that requires a protection.
    /// </summary>
    public class ResourceOption : ILoggableOptions, IValidatableOptions
    {
        public string Name { get; set; } = null!;
        
        public string Route { get; set; } = null!;

        public string FilesDir { get; set; } = null!;

        public string[] AllowedUsers { get; set; } = Array.Empty<string>();

        /// <inheritdoc />
        public IReadOnlyCollection<ConfigurationValidationError> Validate(string? prefix = null)
        {
            var errorCollection = new ConfigurationValidationErrorCollection(prefix);
            
            errorCollection.AddErrorIf(String.IsNullOrWhiteSpace(Name), nameof(Name), "can't be empty");
            errorCollection.AddErrorIf(String.IsNullOrWhiteSpace(Route), nameof(Route), "can't be empty");
            errorCollection.AddErrorIf(String.IsNullOrWhiteSpace(FilesDir), nameof(FilesDir), "can't be empty");
            errorCollection.AddErrorIf(!Directory.Exists(FilesDir), nameof(FilesDir), "directory doesn't exit");
            errorCollection.AddErrorIf(AllowedUsers == null || AllowedUsers.Length == 0, nameof(AllowedUsers), "can't be empty");

            return errorCollection;
        }
    }
}