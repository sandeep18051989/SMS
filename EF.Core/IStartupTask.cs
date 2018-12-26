namespace EF.Core
{
    public interface IStartupTask 
    {
        void Execute();
        int Order { get; }
    }
}
