namespace EF.Services.Context
{
    /// <summary>
    /// Store context
    /// </summary>
    public interface IWebContext
    {
        /// <summary>
        /// Gets or sets the current store
        /// </summary>
        string CurrentStoreUrl { get; }
    }
}
