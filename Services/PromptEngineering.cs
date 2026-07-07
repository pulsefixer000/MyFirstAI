using MyFirstAI.Models;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Text.Json;

namespace MyFirstAI.Services
{
    public class PromptEngineering
    {
        /// <summary>
        /// More advanced ChatBot demo using PromptEngineering, you can ask questions and get answers from the AI model. The AI model will follow the rules defined in the system message.
        /// </summary>
        /// <returns></returns>
        public async Task TeachingChatBotAsync()
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

            List<ChatMessage> messages =
            [
                new SystemChatMessage(
            """
            You are a Senior .NET Mentor.

            Your goal is to teach .NET and AI concepts clearly.

            Follow these rules:
            1. Explain in simple English.
            2. Use bullet points whenever possible.
            3. Provide one real-world analogy.
            4. Provide one practical C# example if applicable.
            5. Highlight important keywords in **bold**.
            6. Keep answers under 250 words unless I ask for more details.
            7. If I ask an interview question, first provide the answer, then explain why it is correct.
            8. If I ask for code, write clean, production-quality C# code and explain it step by step.
            """
        )
            ];

            while (true)
            {
                Console.Write("\nYou: ");
                string? question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                messages.Add(new UserChatMessage(question));

                ChatCompletion completion = await client.CompleteChatAsync(messages);

                string answer = completion.Content[0].Text;

                Console.WriteLine($"\nAI:\n{answer}");

                messages.Add(new AssistantChatMessage(answer));
            }
        }

        public async Task InterviewChatBotAsync()
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

            List<ChatMessage> messages =
            [
               new SystemChatMessage(
    """
    You are a Senior .NET Technical Interviewer.

    Rules:
    - Ask only one interview question at a time.
    - Wait for my answer.
    - Evaluate my answer.
    - Give a score from 1 to 10.
    - Explain what I did well.
    - Explain what I should improve.
    - Finally provide the ideal answer.
    """
)
            ];

            while (true)
            {
                Console.Write("\nYou: ");
                string? question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                messages.Add(new UserChatMessage(question));

                ChatCompletion completion = await client.CompleteChatAsync(messages);

                string answer = completion.Content[0].Text;

                Console.WriteLine($"\nAI:\n{answer}");

                messages.Add(new AssistantChatMessage(answer));
            }
        }

        public async Task CodeReviwerChatBotAsync()
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

            List<ChatMessage> messages =
            [
               new SystemChatMessage(
    """
    You are a Senior .NET Code Reviewer.

    Rules:
    Review every C# code I provide.
    Suggest improvements.
    Mention performance, readability, and best practices.
    """
)
            ];

            while (true)
            {
                Console.Write("\nYou: ");
                string? question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                if (question.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                messages.Add(new UserChatMessage(question));

                ChatCompletion completion = await client.CompleteChatAsync(messages);

                string answer = completion.Content[0].Text;

                Console.WriteLine($"\nAI:\n{answer}");

                messages.Add(new AssistantChatMessage(answer));
            }
        }

        //Structured Output (JSON)

        public async Task StartStructuredOutputDemoAsync()
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

            List<ChatMessage> messages =
            [
                new SystemChatMessage(
            """
            You are a Senior .NET Mentor.

            Return ONLY valid JSON.

            Do not write markdown.

            Do not write explanation outside JSON.

            Return this schema:

            {
              "question":"",
              "difficulty":"",
              "answer":"",
              "example":""
            }
            """),

        new UserChatMessage("Explain Dependency Injection")
            ];

            ChatCompletion completion =
                await client.CompleteChatAsync(messages);

            string json = completion.Content[0].Text;

            Console.WriteLine("Raw JSON");
            Console.WriteLine("---------------------");
            Console.WriteLine(json);

            Console.WriteLine();

            InterviewAnswer? result =
                JsonSerializer.Deserialize<InterviewAnswer>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            Console.WriteLine("Parsed Object");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Question   : {result?.Question}");
            Console.WriteLine($"Difficulty : {result?.Difficulty}");
            Console.WriteLine($"Answer     : {result?.Answer}");
            Console.WriteLine($"Example    : {result?.Example}");
        }

        public async Task StartFewShotPromptDemoAsync()
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

            List<ChatMessage> messages =
            [
                new SystemChatMessage(
            """
            You are a .NET Interview Expert.

            Classify interview questions.

            Return only:

            Question:
            Difficulty:
            Reason:
            """
        ),

        new UserChatMessage(
            """
            Example 1

            Question:
            What is int?

            Difficulty:
            Easy

            Reason:
            Basic C# concept.

            --------------------

            Example 2

            Question:
            Explain async/await.

            Difficulty:
            Medium

            Reason:
            Requires understanding asynchronous programming.

            --------------------

            Example 3

            Question:
            Explain CAP Theorem.

            Difficulty:
            Hard

            Reason:
            Distributed systems concept.

            --------------------

            Now classify this:

            Explain Dependency Injection.
            """
        )
            ];

            ChatCompletion completion =
                await client.CompleteChatAsync(messages);

            Console.WriteLine(completion.Content[0].Text);
        }

        public async Task StartProductionFewShotDemoAsync()
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

            List<ChatMessage> messages =
            [
                new SystemChatMessage(
            """
            You are a Senior .NET Technical Interview Expert.

            Your task is to classify .NET interview questions.

            Classification Rules:

            Easy (0-2 Years):
            - Variables
            - Data Types
            - Loops
            - Arrays
            - Operators
            - Basic OOP

            Medium (3-5 Years):
            - LINQ
            - Delegates
            - Events
            - async/await
            - Collections
            - Exception Handling
            - Entity Framework

            Hard (6-10+ Years):
            - Dependency Injection
            - SOLID Principles
            - Design Patterns
            - Microservices
            - CQRS
            - Event Sourcing
            - Middleware
            - Garbage Collection
            - Performance Tuning

            Return ONLY this format:

            Question:
            Difficulty:
            Experience:
            Reason:
            """
        ),

        new UserChatMessage(
            """
            Example 1

            Question:
            What is int?

            Difficulty:
            Easy

            Experience:
            0-2 Years

            Reason:
            Basic C# syntax.

            ------------------------

            Example 2

            Question:
            Explain async/await.

            Difficulty:
            Medium

            Experience:
            3-5 Years

            Reason:
            Requires understanding of asynchronous programming.

            ------------------------

            Example 3

            Question:
            Explain SOLID Principles.

            Difficulty:
            Hard

            Experience:
            6-10+ Years

            Reason:
            Senior-level software design concept.

            ------------------------

            Now classify:

            Explain Dependency Injection.
            """
        )
            ];

            ChatCompletion completion =
                await client.CompleteChatAsync(messages);

            Console.WriteLine();
            Console.WriteLine("===== AI Classification =====");
            Console.WriteLine();

            Console.WriteLine(completion.Content[0].Text);
        }

        public async Task UserInputProductionFewShotDemoAsync()
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

            Console.Write("Enter Interview Question: ");
            string? interviewQuestion = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(interviewQuestion))
            {
                Console.WriteLine("Question cannot be empty.");
                return;
            }

            List<ChatMessage> messages =
            [
                new SystemChatMessage(
            """
            You are a Senior .NET Technical Interview Expert.

            Your task is to classify .NET interview questions.

            Classification Rules:

            Easy (0-2 Years):
            - Variables
            - Data Types
            - Loops
            - Arrays
            - Operators
            - Basic OOP

            Medium (3-5 Years):
            - LINQ
            - Delegates
            - Events
            - async/await
            - Collections
            - Exception Handling
            - Entity Framework

            Hard (6-10+ Years):
            - Dependency Injection
            - SOLID Principles
            - Design Patterns
            - Microservices
            - CQRS
            - Event Sourcing
            - Middleware
            - Garbage Collection
            - Performance Tuning

            Return ONLY in this format:

            Question:
            Difficulty:
            Experience:
            Reason:
            """
        ),

        new UserChatMessage($$"""
            Example 1

            Question:
            What is int?

            Difficulty:
            Easy

            Experience:
            0-2 Years

            Reason:
            Basic C# syntax.

            ------------------------

            Example 2

            Question:
            Explain async/await.

            Difficulty:
            Medium

            Experience:
            3-5 Years

            Reason:
            Requires understanding of asynchronous programming.

            ------------------------

            Example 3

            Question:
            Explain SOLID Principles.

            Difficulty:
            Hard

            Experience:
            6-10+ Years

            Reason:
            Senior-level software design concept.

            ------------------------

            Now classify this interview question:

            {{interviewQuestion}}
            """)
            ];

            ChatCompletion completion = await client.CompleteChatAsync(messages);

            Console.WriteLine();
            Console.WriteLine("===== AI Classification =====");
            Console.WriteLine();

            Console.WriteLine(completion.Content[0].Text);
        }

        public async Task StartPromptChainingDemoAsync()
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

            Console.Write("Enter .NET Topic: ");
            string? topic = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(topic))
                return;

            //-------------------------------------------------
            // STEP 1 - Classify Difficulty
            //-------------------------------------------------

            List<ChatMessage> difficultyMessages =
            [
                new SystemChatMessage(
            """
            You are a Senior .NET Interview Expert.

            Your job is ONLY to classify interview topics.

            Rules:
            - Return ONLY one word.
            - Allowed values are:
              Easy
              Medium
              Hard
            - Never explain.
            - Never use markdown.
            - Never add punctuation.
            - Never return any other text.
            """
        ),

        new UserChatMessage(
            $"Classify this .NET interview topic: {topic}")
            ];

            ChatCompletion difficultyResponse =
                await client.CompleteChatAsync(difficultyMessages);

            string difficulty = difficultyResponse.Content[0].Text.Trim();

            Console.WriteLine();
            Console.WriteLine("========== Step 1 ==========");
            Console.WriteLine($"Raw Response : {difficulty}");

            //-------------------------------------------------
            // STEP 2 - Explain Topic
            //-------------------------------------------------

            List<ChatMessage> explanationMessages =
            [
                new SystemChatMessage(
            """
            You are a Senior .NET Trainer.
            Explain concepts clearly.
            """
        ),

        new UserChatMessage(
            $"""
            Explain "{topic}" for a {difficulty} level .NET developer.

            Include:

            1. Simple Explanation

            2. Real-world Analogy

            3. Production C# Example
            """)
            ];

            ChatCompletion explanationResponse =
                await client.CompleteChatAsync(explanationMessages);

            Console.WriteLine();
            Console.WriteLine("========== Step 2 : Explanation ==========");
            Console.WriteLine();

            Console.WriteLine(explanationResponse.Content[0].Text);

            //-------------------------------------------------
            // STEP 3 - Interview Questions
            //-------------------------------------------------

            List<ChatMessage> interviewMessages =
            [
                new SystemChatMessage(
            """
            You are a Senior .NET Interviewer.
            Generate interview questions only.
            """
        ),

        new UserChatMessage(
            $"""
            Generate 3 interview questions.

            Topic:
            {topic}

            Difficulty:
            {difficulty}

            Return only the questions.
            """)
            ];

            ChatCompletion interviewResponse =
                await client.CompleteChatAsync(interviewMessages);

            Console.WriteLine();
            Console.WriteLine("========== Step 3 : Interview Questions ==========");
            Console.WriteLine();

            Console.WriteLine(interviewResponse.Content[0].Text);
        }
    }
}
