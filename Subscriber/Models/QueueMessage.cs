namespace Subscriber.Models
{
    public class QueueMessage
    {
        public string Topic { get; set; }
        public string Source { get; set; }
        public string EventType { get; set; }
        public int EventCount {get; set;}
    }
}