using System;
using Microsoft.Extensions.Configuration;

namespace MyFirstAI.Services
{
    /// <summary>
    /// Centralized provider for retrieving API keys from environment or configuration.
    /// Usage: ApiKeyProvider.GetApiKey() or ApiKeyProvider.GetApiKey("GITHUB_TOKEN")
    /// </summary>
    public static class ApiKeyProvider
    {
        public static string? GetApiKey(string? envVarName = null, IConfiguration? configuration = null)
        {
            if (!string.IsNullOrWhiteSpace(envVarName))
            {
                var val = Environment.GetEnvironmentVariable(envVarName);
                if (!string.IsNullOrWhiteSpace(val))
                    return val;
            }

            string[] candidates = new[] {
                "OPENAI_API_KEY",
                "OPENAIKEY",
                "GITHUB_TOKEN",
                "AZURE_OPENAI_API_KEY",
                "AZURE_OPENAI_KEY"
            };

            foreach (var name in candidates)
            {
                var val = Environment.GetEnvironmentVariable(name);
                if (!string.IsNullOrWhiteSpace(val))
                    return val;
            }

            if (configuration != null)
            {
                var cfg = configuration["OpenAI:ApiKey"] ?? configuration["OpenAIApiKey"] ?? configuration["ApiKey"];
                if (!string.IsNullOrWhiteSpace(cfg))
                    return cfg;
            }

            return null;
        }
    }
}
