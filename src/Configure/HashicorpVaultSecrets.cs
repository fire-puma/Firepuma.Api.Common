using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.HashiCorpVault;
using Microsoft.Extensions.Configuration.HashiCorpVault.Test;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Firepuma.Api.Common.Configure
{
    public static class HashicorpVaultSecrets
    {
        public static void AddHashicorpVaultSecretsToConfiguration(
            WebHostBuilderContext context,
            IConfigurationBuilder configBuilder,
            string vaultOptionsSectionKey,
            string vaultSeederSectionKey)
        {
            var configuration = configBuilder.Build();

            // bind vault options
            var options = new VaultOptions();
            configuration.Bind(vaultOptionsSectionKey, options);

            // bind seeder
            var seedData = new List<VaultSeeder>();
            configuration.Bind(vaultSeederSectionKey, seedData);

            var logger = new LoggerFactory()
                .CreateLogger<VaultWriteService>();

            // seed
            new VaultWriteService(
                logger,
                options,
                seedData
            ).SeedVault();

            var vaultConfigurationBuilder = new ConfigurationBuilder();
            var vaultConfiguration = vaultConfigurationBuilder.AddHashiCorpVault(configuration).Build();

            var prefixedConfigValues = new Dictionary<string, string>();
            foreach (var secretPath in options.Secrets)
            {
                var configSectionName = secretPath.Replace("/", ":");
                var jsonString = vaultConfiguration.GetValue<string>(configSectionName);
                var secretValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
                foreach (var keyValue in secretValues)
                {
                    prefixedConfigValues.Add(configSectionName + ":" + keyValue.Key, keyValue.Value);
                }
            }
            configBuilder.AddInMemoryCollection(prefixedConfigValues);

            context.Configuration = configBuilder.Build();
        }
    }
}