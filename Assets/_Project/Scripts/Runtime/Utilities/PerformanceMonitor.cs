using UnityEngine;

namespace Scripts.Runtime.Utilities
{
    public class PerformanceMonitor : MonoBehaviour
    {
        [SerializeField] private bool createPerformanceMonitor;
        
        [SerializeField] private GameObject performanceMonitorPrefab;
        
        
        private GameObject _performanceMonitorInstance;
        
        private void Awake()
        {
            if (performanceMonitorPrefab == null)
            {
                Debug.LogError("Performance Monitor prefab is not assigned.");
                return;
            }

            if (createPerformanceMonitor)
            {
                CreatePerformanceMonitor();
            }
        }

        private void CreatePerformanceMonitor()
        {
            _performanceMonitorInstance = Instantiate(performanceMonitorPrefab);
        }
    }
}
