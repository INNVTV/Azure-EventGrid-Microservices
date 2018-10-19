using System;

namespace Subscriber.Models
{
    public class GridEvent<T> where T: class
    {
        public string Id { get; set;}
        public string EventType { get; set;}
        public string Subject {get; set;}
        public DateTime EventTime { get; set; } 
        public T Data { get; set; }  //<--Custom data
        public string Topic { get; set; }
    }
}