using System.Collections.Generic;

namespace Csharp.Extensions.Auth
{
    public class UserAuthProfile
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
