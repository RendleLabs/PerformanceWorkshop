namespace JaegerTracing
{
    internal class JaegerOptions
    {
        public string ServiceName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string[] Sources { get; set; }

        public bool IsValid => ServiceName is {Length: > 0} && Host is {Length: > 0} && Port > 0;
    }
}