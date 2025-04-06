using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Runtime.UserInterface
{
    [RequireComponent(typeof(Button))]
    public class TaskButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private Button _button;
        
        private Action _taskAction;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        public void Initialize(string taskName, Action taskAction)
        {
            text.SetText(taskName);
            
            _taskAction = taskAction;
        }

        private void OnClicked()
        {
            _taskAction?.Invoke();
        }
    }
}