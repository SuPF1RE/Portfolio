using App.Models;

namespace App.Services
{
    public class ThreadGeneratorService
    {
        private readonly Random _random = new();
        public async Task StartAsync(int threadId,
            Action<ThreadData> onGenerated,
            CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    int delay = _random.Next(500, 2000);
                    await Task.Delay(delay, token).ConfigureAwait(false);

                    int length = _random.Next(5, 11);
                    string str = GenerateString(length);

                    var data = new ThreadData
                    {
                        ThreadId = threadId,
                        Time = DateTime.Now,
                        Data = str
                    };

                    onGenerated?.Invoke(data);
                }
            }
            catch (OperationCanceledException)
            {
                // Normal ending upon cancellation — ignore.
            }
            catch (Exception)
            {
                // Logging when necessary; 
            }
        }

        private string GenerateString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[_random.Next(chars.Length)])
                .ToArray());
        }
    }
}
