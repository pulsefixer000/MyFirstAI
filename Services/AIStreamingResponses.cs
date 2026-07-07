using OpenAI;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Text;

namespace MyFirstAI.Services
{
    public class AIStreamingResponses
    {
        public async Task StartStreamingResponseDemoAsync()
        {
            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("GITHUB_TOKEN is not configured.");
                return;
            }

            ChatClient client = new(
                model: "gpt-4.1-mini",
                credential: new ApiKeyCredential(token),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri("https://models.inference.ai.azure.com")
                });

            Console.Write("Ask AI: ");
            string? question = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(question))
                return;

            Console.WriteLine();
            Console.Write("AI: ");

            await foreach (var update in client.CompleteChatStreamingAsync(question))
            {
                foreach (var content in update.ContentUpdate)
                {
                    Console.Write(content.Text);
                }
            }

            Console.WriteLine();
        }
        public async Task StartStreamingChatBotAsync()
        {
            List<ChatMessage> messages = new();

            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("GITHUB_TOKEN is not configured.");
                return;
            }

            ChatClient client = new(
                model: "gpt-4.1-mini",
                credential: new ApiKeyCredential(token),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri("https://models.inference.ai.azure.com")
                });

            Console.WriteLine("===== AI Streaming ChatBot =====");
            Console.WriteLine("Type 'exit' to quit.");

            while (true)
            {
                Console.Write("\nYou: ");

                string? question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                // Save user message
                messages.Add(new UserChatMessage(question));

                Console.Write("\nAI: ");

                string fullResponse = string.Empty;

                await foreach (var update in client.CompleteChatStreamingAsync(messages))
                {
                    foreach (var content in update.ContentUpdate)
                    {
                        Console.Write(content.Text);
                        fullResponse += content.Text;
                    }
                }

                // Save AI response for conversation memory
                messages.Add(new AssistantChatMessage(fullResponse));

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(new string('-', 100));
            }

            Console.WriteLine("\nGood Bye!");
        }

        public async Task StartStreamingChatBot2Async()
        {
            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("GITHUB_TOKEN is not configured.");
                return;
            }

            ChatClient client = new(
                model: "gpt-4.1-mini",
                credential: new ApiKeyCredential(token),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri("https://models.inference.ai.azure.com")
                });

            List<ChatMessage> messages = new()
    {
        new SystemChatMessage(
            """
            You are a helpful AI assistant.
            Answer clearly and concisely.
            """
        )
    };

            Console.WriteLine("========== AI Streaming ChatBot ==========");
            Console.WriteLine("Type 'exit' to quit.");

            while (true)
            {
                Console.Write("\nYou: ");
                string? question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                messages.Add(new UserChatMessage(question));

                Console.Write("\nAI: ");

                string fullResponse = string.Empty;

                await foreach (var update in client.CompleteChatStreamingAsync(messages))
                {
                    foreach (var content in update.ContentUpdate)
                    {
                        Console.Write(content.Text);
                        fullResponse += content.Text;
                    }
                }

                messages.Add(new AssistantChatMessage(fullResponse));

                Console.WriteLine();
                Console.WriteLine("---------------------------------------------------------------");
            }

            Console.WriteLine("\nGood Bye!");
        }
        public async Task StartStreamingChatBotWithSpinnerAsync()
        {
            List<ChatMessage> messages = new();

            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("GITHUB_TOKEN is not configured.");
                return;
            }

            ChatClient client = new(
                model: "gpt-4.1-mini",
                credential: new ApiKeyCredential(token),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri("https://models.inference.ai.azure.com")
                });

            Console.WriteLine("===== AI Streaming ChatBot =====");
            Console.WriteLine("Type 'exit' to quit.");

            while (true)
            {
                Console.Write("\nYou: ");

                string? question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                messages.Add(new UserChatMessage(question));

                // Spinner variables
                bool responseStarted = false;
                char[] spinner = { '|', '/', '-', '\\' };

                using CancellationTokenSource cts = new();

                // Start spinner
                var spinnerTask = Task.Run(async () =>
                {
                    int index = 0;

                    while (!cts.Token.IsCancellationRequested)
                    {
                        Console.Write($"\rAI is thinking... {spinner[index++ % spinner.Length]}");
                        await Task.Delay(100);
                    }
                });

                string fullResponse = string.Empty;

                await foreach (var update in client.CompleteChatStreamingAsync(messages))
                {
                    // Stop spinner when first response arrives
                    if (!responseStarted)
                    {
                        responseStarted = true;

                        cts.Cancel();

                        try
                        {
                            await spinnerTask;
                        }
                        catch (TaskCanceledException)
                        {
                        }

                        // Clear spinner line
                        Console.Write("\r" + new string(' ', Console.WindowWidth - 1));
                        Console.Write("\rAI: ");
                    }

                    foreach (var content in update.ContentUpdate)
                    {
                        Console.Write(content.Text);
                        fullResponse += content.Text;
                    }
                }

                messages.Add(new AssistantChatMessage(fullResponse));

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(new string('-', 100));
            }

            Console.WriteLine("\nGood Bye!");
        }

        public async Task StartStreamingChatBotWithThinkingAsync()
        {
            List<ChatMessage> messages = new();

            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("GITHUB_TOKEN is not configured.");
                return;
            }

            ChatClient client = new(
                model: "gpt-4.1-mini",
                credential: new ApiKeyCredential(token),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri("https://models.inference.ai.azure.com")
                });

            Console.WriteLine("===== AI Thinking ChatBot =====");
            Console.WriteLine("Type 'exit' to quit.");

            while (true)
            {
                Console.Write("\nYou: ");

                string? question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                messages.Add(new UserChatMessage(question));

                Console.WriteLine();
                Console.WriteLine("🤔 Understanding your question...");
                await Task.Delay(800);

                Console.WriteLine("📚 Searching my knowledge...");
                await Task.Delay(800);

                Console.WriteLine("🧠 Preparing the best response...");
                await Task.Delay(800);

                Console.WriteLine();
                Console.Write("AI: ");

                string fullResponse = "";

                await foreach (var update in client.CompleteChatStreamingAsync(messages))
                {
                    foreach (var content in update.ContentUpdate)
                    {
                        Console.Write(content.Text);
                        fullResponse += content.Text;
                    }
                }

                messages.Add(new AssistantChatMessage(fullResponse));

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(new string('-', 100));
            }

            Console.WriteLine("\nGood Bye!");
        }

        public async Task StartStreamingChatBotWithThinkingIndicatorAsync()
        {
            List<ChatMessage> messages = new();

            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("GITHUB_TOKEN is not configured.");
                return;
            }

            ChatClient client = new(
                model: "gpt-4.1-mini",
                credential: new ApiKeyCredential(token),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri("https://models.inference.ai.azure.com")
                });

            Console.WriteLine("===== AI Streaming ChatBot =====");
            Console.WriteLine("Type 'exit' to quit.");

            while (true)
            {
                Console.Write("\nYou: ");

                string? question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                messages.Add(new UserChatMessage(question));

                // Thinking indicator
                Console.Write("AI is thinking");

                for (int i = 0; i < 3; i++)
                {
                    await Task.Delay(300);
                    Console.Write(".");
                }

                Console.WriteLine();
                Console.Write("AI: ");

                string fullResponse = string.Empty;

                await foreach (var update in client.CompleteChatStreamingAsync(messages))
                {
                    foreach (var content in update.ContentUpdate)
                    {
                        Console.Write(content.Text);
                        fullResponse += content.Text;
                    }
                }

                messages.Add(new AssistantChatMessage(fullResponse));

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(new string('-', 100));
            }

            Console.WriteLine("\nGood Bye!");
        }

        public async Task StartStreamingDotNetExpertChatBotAsync()
        {
            List<ChatMessage> messages =
            [
                new SystemChatMessage(
            """
            You are a Senior .NET Architect with 15 years of experience.

            Rules:
            - Answer only .NET, C#, SQL Server, Azure and Software Architecture questions.
            - Explain in simple English.
            - Give production examples whenever possible.
            - If the question is outside these topics, politely say:
              "I'm a .NET Technical Assistant. Please ask a .NET related question."
            """)
            ];

            var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("GITHUB_TOKEN is not configured.");
                return;
            }

            ChatClient client = new(
                model: "gpt-4.1-mini",
                credential: new ApiKeyCredential(token),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri("https://models.inference.ai.azure.com")
                });

            Console.WriteLine("===== .NET Expert AI =====");
            Console.WriteLine("Type 'exit' to quit.");

            while (true)
            {
                Console.Write("\nYou: ");

                string? question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                messages.Add(new UserChatMessage(question));

                Console.Write("\n.NET Expert: ");

                string fullResponse = "";

                await foreach (var update in client.CompleteChatStreamingAsync(messages))
                {
                    foreach (var content in update.ContentUpdate)
                    {
                        Console.Write(content.Text);
                        fullResponse += content.Text;
                    }
                }

                messages.Add(new AssistantChatMessage(fullResponse));

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(new string('-', 100));
            }

            Console.WriteLine("\nGood Bye!");
        }
    }
}
