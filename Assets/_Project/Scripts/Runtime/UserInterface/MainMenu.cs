using Cysharp.Threading.Tasks;
using Scripts.Runtime.Core;
using UnityEngine;

namespace Scripts.Runtime.UserInterface
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TaskButton taskButtonPrefab;
        [SerializeField] private RectTransform taskButtonsContainer;

        [SerializeField] private TaskButtonData[] tasks;
        

        private void Start()
        {
            foreach (var taskButtonData in tasks)
            {
                AddTaskButton(taskButtonData);
            }
        }

        private void AddTaskButton(TaskButtonData taskButtonData)
        {
            var taskButtonInstance = Instantiate(taskButtonPrefab, taskButtonsContainer);

            taskButtonInstance.Initialize(taskButtonData.TaskName, () =>
            {
                SceneLoader.LoadSceneAsync(taskButtonData.SceneName).Forget();
            });
        }
    }
}
