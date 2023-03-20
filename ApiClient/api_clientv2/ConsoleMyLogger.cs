using System;

namespace BirokratNext {
    internal class ConsoleMyLogger : IMyLogger {
        public ConsoleMyLogger() {
        }

        public void LogError(string some) {
            Console.WriteLine(some);
        }

        public void LogWarning(string some) {
            Console.WriteLine(some);
        }

        public void LogInformation(string some) {
            Console.WriteLine(some);
        }
    }
}