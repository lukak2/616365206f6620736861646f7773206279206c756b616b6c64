using Cysharp.Threading.Tasks;
using Scripts.Runtime.Common;
using Scripts.Runtime.Core;

namespace Scripts.Runtime.UserInterface
{
    public class ReturnToMainMenuButton : BaseButton
    {
        protected override void OnClicked()
        {
            SceneLoader.LoadSceneAsync(Constants.Scenes.MainMenu).Forget();
        }
    }
}
