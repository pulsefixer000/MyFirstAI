using System.ClientModel;
using OpenAI;
using OpenAI.Chat;

namespace MyFirstAI.Services
{
    public class ChatService
    {
        /// <summary>
        /// Single question and answer demo, you can ask a question and get an answer from the AI model.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// ChatBot demo, you can ask questions and get answers from the AI model.
        /// </summary>
        /// <returns></returns>
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

        public async Task StartStreamingChatAsync()
        {

            //List<ChatMessage> messages = new();
            List<ChatMessage> messages =
            [
                new SystemChatMessage("You are a helpful assistant.")
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

            Console.WriteLine("===== AI ChatBot =====");
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

                ChatCompletion completion =
                    await client.CompleteChatAsync(messages);

                Console.WriteLine($"\nAI : {completion.Content[0].Text}");

                messages.Add(new AssistantChatMessage(completion.Content[0].Text));
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------------------------------------");
            }

            Console.WriteLine("\nGood Bye!");
        }


    }
}
