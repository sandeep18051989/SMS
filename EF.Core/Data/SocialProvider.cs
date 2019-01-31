namespace EF.Core.Data
{
    public partial class SocialProvider : BaseEntity
    {
        public string DisplayIdentifier { get; set; }
        public string ProviderSystemName { get; set; }
        public bool IsActive { get; set; }
        public string AuthenticationMethodService { get; set; }
        public string Version { get; set; }
        public string AuthenticationMethodServiceNamespace { get; set; }
        public string AssemblyName { get; set; }
        public bool IsInstalled { get; set; }

    }

}
