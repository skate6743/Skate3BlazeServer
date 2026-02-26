using System.Collections.Concurrent;

namespace Servers
{
    public static class ServerLogger
    {
        private static readonly BlockingCollection<string> _queue = new();
        private static readonly string _logFile = "logs.txt";

        static ServerLogger()
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var entry in _queue.GetConsumingEnumerable())
                    File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _logFile), entry + Environment.NewLine);
            }, TaskCreationOptions.LongRunning);
        }

        public static void Log(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm dd/MM/yyyy");
            _queue.Add($"[{timestamp}] {message}");
        }

        public static void LogSignIn(string username) =>
            Log($"User {username} has signed in");
    }
}
