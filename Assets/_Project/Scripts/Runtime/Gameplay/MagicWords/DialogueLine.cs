namespace Scripts.Runtime.Gameplay.MagicWords
{
    public class DialogueLine
    {
        public string AvatarName { get; set; }
        public string DialogueText { get; set; }
        
        public DialogueLine(string avatarName, string dialogueText)
        {
            AvatarName = avatarName;
            DialogueText = dialogueText;
        }
    }
}