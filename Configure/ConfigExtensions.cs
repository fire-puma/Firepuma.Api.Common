using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Firepuma.Api.Common.Configure
{
    public static class ConfigExtensions
    {
        private static List<Action<IServiceProvider>> _validateActions = new List<Action<IServiceProvider>>();

        public static void ConfigureAndValidate<T>(this IServiceCollection serviceCollection, Action<T> configureOptions) where T : class, new()
        {
            _validateActions.Add(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<T>>();
                var unused = options.Value;
            });

            // Inspired by https://blog.bredvid.no/validating-configuration-in-asp-net-core-e9825bd15f10
            serviceCollection
                .Configure(configureOptions)
                .PostConfigure<T>(settings =>
                {
                    var configErrors = settings.ValidationErrors().ToArray();
                    if (configErrors.Any())
                    {
                        var aggregatedErrors = string.Join(",", configErrors);
                        var count = configErrors.Length;
                        var configType = typeof(T).FullName;
                        throw new ApplicationException($"There are {count} configuration error(s) in {configType}: {aggregatedErrors}");
                    }
                });
        }

        private static IEnumerable<string> ValidationErrors(this object @this)
        {
            var context = new ValidationContext(@this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(@this, context, results, true);
            foreach (var validationResult in results)
            {
                yield return validationResult.ErrorMessage;
            }
        }

        public static void ValidateAll(IServiceProvider serviceProvider)
        {
            foreach (var action in _validateActions)
            {
                action(serviceProvider);
            }

            _validateActions = null;
        }
    }
}