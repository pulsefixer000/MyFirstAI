using MyFirstAI.Services;

ChatService obj = new ChatService();
//obj.StartChat_FirstDemo().Wait();
// await obj.StartChatBot();
 //await obj.StartStreamingChatAsync();


PromptEngineering obj2 = new PromptEngineering();
//await obj2.TeachingChatBotAsync();
//await obj2.InterviewChatBotAsync();
//await obj2.CodeReviwerChatBotAsync();
//await obj2.StartStructuredOutputDemoAsync();
//await obj2.StartFewShotPromptDemoAsync();
//await obj2.StartProductionFewShotDemoAsync();
//await obj2.UserInputProductionFewShotDemoAsync();
//await obj2.StartPromptChainingDemoAsync();

AIStreamingResponses obj3 = new AIStreamingResponses();
//obj3.StartStreamingResponseDemoAsync().Wait();
//obj3.StartStreamingChatBotAsync().Wait();
//obj3.StartStreamingChatBot2Async().Wait();
obj3.StartStreamingChatBotWithSpinnerAsync().Wait();
//obj3.StartStreamingChatBotWithThinkingAsync().Wait();
//obj3.StartStreamingChatBotWithThinkingIndicatorAsync().Wait();
//obj3.StartStreamingDotNetExpertChatBotAsync().Wait();