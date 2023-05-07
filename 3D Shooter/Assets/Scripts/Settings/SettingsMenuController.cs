using UnityEngine;
using UnityEngine.Events;

namespace Settings
{
    public class SettingsMenuController : MonoBehaviour
    {
        [field: SerializeField] public GameObject Menu { get; private set; }
        [field: SerializeField] public bool InitialActive { get; private set; } = false; 

        public static UnityEvent<bool> OnMenuActiveChange { get; } = new();

        private void Awake()
        {
            Menu.SetActive(InitialActive);
            
            InputHandler.EscUped += () =>
            {
                var newIsActive = !Menu.activeSelf;
                
                Menu.SetActive(newIsActive);
                OnMenuActiveChange?.Invoke(newIsActive);
                
                Cursor.lockState = newIsActive ? CursorLockMode.None : CursorLockMode.Locked;
                
                if (!newIsActive)
                    UserSettings.Save();
            };
        }
    }
}