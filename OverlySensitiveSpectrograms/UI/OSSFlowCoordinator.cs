using BeatSaberMarkupLanguage;
using HMUI;
using Zenject;

namespace OverlySensitiveSpectrograms.UI
{
    internal class OSSFlowCoordinator : FlowCoordinator
    {
        private MainFlowCoordinator _mainFlowCoordinator;
        private OSSSettingsView _ossSettingsView;

        [Inject]
        public void Construct(MainFlowCoordinator mainFlowCoordinator, OSSSettingsView ossSettingsView)
        {
            _mainFlowCoordinator = mainFlowCoordinator;
            _ossSettingsView = ossSettingsView;
        }

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
