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
            ChatClient? client = CreateChatClient();

            if (client is null)
                return;

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
        """)
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
            ChatClient? client = CreateChatClient();

            if (client is null)
                return;

            List<ChatMessage> messages =
            [
                new SystemChatMessage(
        """
        You are a Senior .NET Technical Interviewer.

        Rules:
        1. Ask only one interview question at a time.
        2. Wait for my answer before asking the next question.
        3. Evaluate my answer.
        4. Give a score from 1 to 10.
        5. Explain what I did well.
        6. Explain what I should improve.
        7. Finally, provide the ideal answer.
        8. Keep the explanation suitable for an experienced .NET developer.
        """)
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

                try
                {
                    ChatCompletion completion =
                        await client.CompleteChatAsync(messages);

                    string answer = completion.Content[0].Text;

                    Console.WriteLine($"\nAI:\n{answer}");

                    messages.Add(new AssistantChatMessage(answer));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nOpenAI Error:\n{ex.Message}");

                    // Remove the failed user message.
                    messages.RemoveAt(messages.Count - 1);
                }
            }
        }

        public async Task CodeReviwerChatBotAsync()
        {
            ChatClient? client = CreateChatClient();

            if (client is null)
                return;

            List<ChatMessage> messages =
            [
                new SystemChatMessage(
        """
        You are a Senior .NET Code Reviewer.

        Rules:
        1. Review every C# code snippet I provide.
        2. Identify bugs and potential issues.
        3. Suggest improvements.
        4. Explain performance considerations.
        5. Evaluate readability and maintainability.
        6. Follow C# and .NET best practices.
        7. Provide corrected code when needed.
        8. Explain the important changes in simple English.
        """)
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

                try
                {
                    ChatCompletion completion =
                        await client.CompleteChatAsync(messages);

                    string answer = completion.Content[0].Text;

                    Console.WriteLine($"\nAI:\n{answer}");

                    messages.Add(new AssistantChatMessage(answer));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nOpenAI Error:\n{ex.Message}");
                    messages.RemoveAt(messages.Count - 1);
                }
            }
        }
        //Structured Output (JSON)

        public async Task StartStructuredOutputDemoAsync()
        {
            ChatClient? client = CreateChatClient();

            if (client is null)
                return;

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
          "question": "",
          "difficulty": "",
          "answer": "",
          "example": ""
        }
        """),

        new UserChatMessage("Explain Dependency Injection")
            ];

            try
            {
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

                if (result is null)
                {
                    Console.WriteLine("Could not parse the JSON response.");
                    return;
                }

                Console.WriteLine("Parsed Object");
                Console.WriteLine("---------------------");
                Console.WriteLine($"Question   : {result.Question}");
                Console.WriteLine($"Difficulty : {result.Difficulty}");
                Console.WriteLine($"Answer     : {result.Answer}");
                Console.WriteLine($"Example    : {result.Example}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON parsing error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OpenAI Error: {ex.Message}");
            }
        }

        public async Task StartFewShotPromptDemoAsync()
        {
            ChatClient? client = CreateChatClient();

            if (client is null)
                return;

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
        """),

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
        """)
            ];

            try
            {
                ChatCompletion completion =
                    await client.CompleteChatAsync(messages);

                Console.WriteLine(completion.Content[0].Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OpenAI Error: {ex.Message}");
            }
        }

        public async Task StartProductionFewShotDemoAsync()
        {
            ChatClient? client = CreateChatClient();

            if (client is null)
                return;

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
        """),

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
        """)
            ];

            try
            {
                ChatCompletion completion =
                    await client.CompleteChatAsync(messages);

                Console.WriteLine();
                Console.WriteLine("===== AI Classification =====");
                Console.WriteLine();

                Console.WriteLine(completion.Content[0].Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OpenAI Error: {ex.Message}");
            }
        }

        public async Task UserInputProductionFewShotDemoAsync()
        {
            ChatClient? client = CreateChatClient();

            if (client is null)
                return;

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
        """),

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

            try
            {
                ChatCompletion completion =
                    await client.CompleteChatAsync(messages);

                Console.WriteLine();
                Console.WriteLine("===== AI Classification =====");
                Console.WriteLine();

                Console.WriteLine(completion.Content[0].Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OpenAI Error: {ex.Message}");
            }
        }

        public async Task StartPromptChainingDemoAsync()
        {

            ChatClient? client = CreateChatClient();

            if (client is null)
                return;



            Console.Write("Enter .NET Topic: ");
            string? topic = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(topic))
            {
                Console.WriteLine("Topic cannot be empty.");
                return;
            }

            try
            {
                // STEP 1 - Classify Difficulty

                List<ChatMessage> difficultyMessages =
                [
                    new SystemChatMessage(
            """
            You are a Senior .NET Interview Expert.

            Your job is ONLY to classify interview topics.

            Rules:
            - Return ONLY one word.
            - Allowed values: Easy, Medium, Hard.
            - Never explain.
            - Never use markdown.
            - Never add punctuation.
            - Never return any other text.
            """),

            new UserChatMessage(
                $"Classify this .NET interview topic: {topic}")
                ];

                ChatCompletion difficultyResponse =
                    await client.CompleteChatAsync(difficultyMessages);

                string difficulty =
                    difficultyResponse.Content[0].Text.Trim();

                if (difficulty is not ("Easy" or "Medium" or "Hard"))
                {
                    Console.WriteLine($"Unexpected difficulty response: {difficulty}");
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("========== Step 1 ==========");
                Console.WriteLine($"Raw Response : {difficulty}");

                // STEP 2 - Explain Topic

                List<ChatMessage> explanationMessages =
                [
                    new SystemChatMessage(
            """
            You are a Senior .NET Trainer.
            Explain concepts clearly using simple English.
            Include practical, production-quality C# examples.
            """),

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

                // STEP 3 - Interview Questions

                List<ChatMessage> interviewMessages =
                [
                    new SystemChatMessage(
            """
            You are a Senior .NET Interviewer.
            Generate interview questions only.
            """),

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
            catch (Exception ex)
            {
                Console.WriteLine($"OpenAI Error: {ex.Message}");
            }
        }

        private ChatClient? CreateChatClient()
        {
            var token = ApiKeyProvider.GetApiKey("OPENAI_API_KEY");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("OPENAI_API_KEY is not configured.");
                return null;
            }

            return new ChatClient(
                model: "gpt-6-luna",
                credential: new ApiKeyCredential(token),
                options: new OpenAIClientOptions
                {
                    Endpoint = new Uri("https://api.openai.com/v1")
                });
        }
    }
}
