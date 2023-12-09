using HMUI;
using Zenject;

namespace OverlySensitiveSpectrograms.UI
{
    internal class OSSFlowCoordinator : FlowCoordinator
    {
        [Inject] private MainFlowCoordinator _mainFlowCoordinator;
        [Inject] private OSSSettingsView _ossSettingsView;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("Overly Sensitive Spectrograms");
                showBackButton = true;

                ProvideInitialViewControllers(_ossSettingsView);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            _mainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}
