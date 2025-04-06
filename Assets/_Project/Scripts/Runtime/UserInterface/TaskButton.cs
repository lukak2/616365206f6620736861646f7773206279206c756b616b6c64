using System;
using TMPro;
using UnityEngine;

namespace Scripts.Runtime.UserInterface
{
    public class TaskButton : BaseButton
    {
        [SerializeField] private TMP_Text text;

        private Action _taskAction;

        public void Initialize(string taskName, Action taskAction)
        {
            text.SetText(taskName);

            _taskAction = taskAction;
        }

        protected override void OnClicked()
        {
            _taskAction?.Invoke();
        }
    }
}