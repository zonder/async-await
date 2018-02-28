using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TryAsyncAwait
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Main - Thread {Thread.CurrentThread.ManagedThreadId}");

      var contentLength = GetContentLengthAsync(10).Result;
      Console.WriteLine($"Content length is {contentLength}");

      Console.WriteLine($"Main End- Thread {Thread.CurrentThread.ManagedThreadId}");
    }

    static async Task<int> GetContentLengthAsync(int param123)
    {
      var client = new HttpClient();
      Task<string> contentTask = client.GetStringAsync("http://google.com");
      Console.WriteLine($"GetContentLengthAsync - Thread {Thread.CurrentThread.ManagedThreadId}");

      DoSomeWork();

      var content = await contentTask;

      Console.WriteLine($"GetContentLengthAsync after first await - Thread {Thread.CurrentThread.ManagedThreadId}");
      // Uses variable from stack
      var delta = param123 + await DoSomeWorkAsync();

      Console.WriteLine($"GetContentLengthAsync after second await - Thread {Thread.CurrentThread.ManagedThreadId}");
      return content.Length + delta;
    }

    static void DoSomeWork()
    {
      Console.WriteLine("doing some work.");
      Console.WriteLine($"DoSomeWork - Thread {Thread.CurrentThread.ManagedThreadId}");
    }

    static async Task<int> DoSomeWorkAsync()
    {
      // Demo exceptions
      // throw new InvalidOperationException("Hell");

      Console.WriteLine($"DoSomeWorkAsync before await - Thread {Thread.CurrentThread.ManagedThreadId}");
      var result = await Task.FromResult<int>(42);
      Console.WriteLine($"DoSomeWorkAsync after await - Thread {Thread.CurrentThread.ManagedThreadId}");
      //await Task.Delay(1000);
      return result;
    }

  }
}