namespace FluentWebApi.Routing
{
    public class HttpVerb
    {
        internal HttpVerb(string verb)
        {
            Verb = verb;
        }

        public string Verb { get; private set; }

        public static readonly HttpVerb Get = new HttpVerb("GET");
        public static readonly HttpVerb Post = new HttpVerb("POST");
        public static readonly HttpVerb Put = new HttpVerb("PUT");
        public static readonly HttpVerb Delete = new HttpVerb("DELETE");
        public static readonly HttpVerb Head = new HttpVerb("HEAD");
        public static readonly HttpVerb Patch = new HttpVerb("PATCH");
        public static readonly HttpVerb Merge = new HttpVerb("MERGE");

        public static HttpVerb GetVerb(string verb)
        {
            verb = verb.ToUpperInvariant();

            switch (verb)
            {
                case "GET": return Get;
                case "POST": return Post;
                case "PUT": return Put;
                case "DELETE": return Delete;
                case "HEAD": return Head;
                case "PATCH": return Patch;
                case "MERGE": return Merge;
            }

            return new HttpVerb(verb);
        }
    }
}