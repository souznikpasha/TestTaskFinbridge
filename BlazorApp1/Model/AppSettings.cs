using Microsoft.Extensions.Options;

namespace BlazorApp1.Model
{
    public class AppSettings
    {
        public int MaxArgumentCount { get; set; }
        public double MinArgumentValue { get; set; }
        public double MaxArgumentValue { get; set; }
        public int DelayMinMilliseconds { get; set; }
        public int DelayMaxMilliseconds { get; set; }
    }
}
