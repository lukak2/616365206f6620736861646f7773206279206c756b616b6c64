using System.Collections.Generic;

namespace Scripts.Runtime.Gameplay.MagicWords
{

    public class Response
    {
        public List<Dialogue> dialogue { get; set; }
        public List<Emoji> emojies { get; set; }
        public List<Avatar> avatars { get; set; }
        
        public class Avatar
        {
            public string name { get; set; }
            public string url { get; set; }
            public string position { get; set; }
        }

        public class Dialogue
        {
            public string name { get; set; }
            public string text { get; set; }
        }

        public class Emoji
        {
            public string name { get; set; }
            public string url { get; set; }
        }
    }
}