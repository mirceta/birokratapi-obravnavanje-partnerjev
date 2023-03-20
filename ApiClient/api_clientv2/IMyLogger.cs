namespace BirokratNext {
    public interface IMyLogger {
        void LogError(string some);
        void LogInformation(string some);
        void LogWarning(string some);
    }
}