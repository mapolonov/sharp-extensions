namespace Csharp.Extensions.Auth
{
    public class BearerOptions
    {
        public string SecretKey { get; set; }
        public string Authority { get; set; }
        public string Audience { get; set; }
        public string ValidIssuer { get; set; }
        public int Expires { get; set; }
    }
}
