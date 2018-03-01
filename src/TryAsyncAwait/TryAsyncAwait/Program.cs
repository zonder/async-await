using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TryAsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintThread(nameof(Main), "start");

            var contentLength = GetTextOccurrencesAsync("google.com", "http://google.com", 10).Result;
            Console.WriteLine($"Occurances count is {contentLength}");

            PrintThread(nameof(Main), "finish");
        }

        static async Task<int> GetTextOccurrencesAsync(string text, string url, int someParameter)
        {
            var client = new HttpClient();
            Task<string> contentTask = client.GetStringAsync(url);
            PrintThread(nameof(GetTextOccurrencesAsync), "start");

            DoSomeWork(); // Sync execution

            var count = await CalculateTagsCountAsync(text, await contentTask);

            PrintThread(nameof(GetTextOccurrencesAsync), "after first await");
            
            var calculationsResult = someParameter * await DoSomeWorkAsync(); // Uses variable from stack
            Console.WriteLine($"Calculated value is {calculationsResult}");

            PrintThread(nameof(GetTextOccurrencesAsync), "after second await");

            return count;
        }

        static void DoSomeWork()
        {
            Console.WriteLine("doing some synchronous work.");
            PrintThread(nameof(DoSomeWork), "_");
        }

        static async Task<int> DoSomeWorkAsync()
        {
            // Demo exceptions
            // throw new InvalidOperationException("Hell");

            PrintThread(nameof(DoSomeWorkAsync), "before await");
            var result = await Task.FromResult<int>(42);

            await Task.Delay(1000);

            PrintThread(nameof(DoSomeWorkAsync), "after await");

            return result;
        }

        // Async?
        static async Task<int> CalculateTagsCountAsync(string subString, string content)
        {
            // ??
            return await Task.FromResult<int>(new Regex(subString).Matches(content).Count);
        }

        static void PrintThread(string method, string details)
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] --- {method} - {details}");
        }

    }
}