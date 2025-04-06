using UnityEngine;

namespace Scripts.Runtime.UserInterface
{
    [CreateAssetMenu(fileName = "TaskButtonData", menuName = "Game/Data/Task Button Data")]
    public class TaskButtonData : ScriptableObject
    {
        [field: SerializeField] public string TaskName { get; private set; }
        [field: SerializeField] public string SceneName { get; private set; }
    }
}