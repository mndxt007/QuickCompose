namespace QuickCompose.Model
{
    public class ChatGptConfig
    {
        public string Endpoint { get; set; }
        public string ApiKey { get; set; }
        public string Instruction { get; set; }
        public string Prompt { get; set; }
    }

    public class Prompt
    {
        public string Operation { get; set; }
        public string Instruction { get; set; }
    }
}
