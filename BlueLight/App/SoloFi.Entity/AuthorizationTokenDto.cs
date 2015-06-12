using System.Collections.Generic;

namespace SoloFi.Entity
{
    public class AuthorizationTokenDto
    {

        public bool success { get; set; }
        public Tokens tokens { get; set; }
        public IList<string> messages { get; set; }

    }

    public class Tokens
    {
        public string Token { get; set; }
        public string Refresh { get; set; }
    }

}
