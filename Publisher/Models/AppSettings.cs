namespace Publisher.Models
{
    public static class AppSettings
    {
        public static Topic Topic1 {get; set;}
        public static Topic Topic2 {get; set;}

    }

    public class Topic
    {   
        public string Endpoint { get; set; }
        public string Key { get; set; }
    }
}