using System;
using System.Collections.Generic;
using System.IO;
using Curiosity.Configuration;
using Curiosity.Hosting.Web;

namespace Heimdal.Configuration
{
    public class HeimdalConfiguration : CuriosityWebAppConfiguration
    {
        public UserOption[] Users { get; set; }
        
        public ResourceOption[] Resources { get; set; }
    }

    public class UserOption : ILoggableOptions, IValidatableOptions
    {
        public int Id { get; set; }
        
        public string Login { get; set; } = null!;
        
        public string PasswordHash { get; set; } = null!;
        
        
        public IReadOnlyCollection<ConfigurationValidationError> Validate(string? prefix = null)
        {
            var errorCollection = new ConfigurationValidationErrorCollection(prefix);
            
            errorCollection.AddErrorIf(Id <= 0, nameof(Id), "must be equal or greater than 1");
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

        public int[] AllowedUserIds { get; set; } = Array.Empty<int>();

        /// <inheritdoc />
        public IReadOnlyCollection<ConfigurationValidationError> Validate(string? prefix = null)
        {
            var errorCollection = new ConfigurationValidationErrorCollection(prefix);
            
            errorCollection.AddErrorIf(String.IsNullOrWhiteSpace(Name), nameof(Name), "can't be empty");
            errorCollection.AddErrorIf(String.IsNullOrWhiteSpace(Route), nameof(Route), "can't be empty");
            errorCollection.AddErrorIf(String.IsNullOrWhiteSpace(FilesDir), nameof(FilesDir), "can't be empty");
            errorCollection.AddErrorIf(!Directory.Exists(FilesDir), nameof(FilesDir), "directory doesn't exit");
            errorCollection.AddErrorIf(AllowedUserIds == null || AllowedUserIds.Length == 0, nameof(AllowedUserIds), "can't be empty");

            return errorCollection;
        }
    }
}