using System.ClientModel;
using OpenAI;
using OpenAI.Chat;

namespace MyFirstAI.Services
{
    public class ChatService
    {
        public async Task StartChat_FirstDemo()
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

            ChatCompletion completion = await client.CompleteChatAsync(
                "Explain Generative AI in simple words.");

            Console.WriteLine(completion.Content[0].Text);
        }

        public async Task StartChatBot()
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

            Console.WriteLine("===== AI ChatBot =====");
            Console.WriteLine("Type 'exit' to quit.");

            while (true)
            {
                Console.Write("\nYou: ");
                var question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                ChatCompletion completion =
                    await client.CompleteChatAsync(question);

                Console.WriteLine($"\nAI : {completion.Content[0].Text}");
            }

            Console.WriteLine("\nGood Bye!");
        }
    }
}
