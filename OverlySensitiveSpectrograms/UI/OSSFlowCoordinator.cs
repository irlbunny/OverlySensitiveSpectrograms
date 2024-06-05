using HMUI;
using Zenject;

namespace OverlySensitiveSpectrograms.UI;

internal class OSSFlowCoordinator : FlowCoordinator
{
    private MainFlowCoordinator _mainFlowCoordinator = null!;
    private OSSSettingsView _settingsView = null!;

    [Inject]
    public void Construct(MainFlowCoordinator mainFlowCoordinator, OSSSettingsView settingsView)
    {
        _mainFlowCoordinator = mainFlowCoordinator;
        _settingsView = settingsView;
    }

    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        if (firstActivation)
        {
            SetTitle("Overly Sensitive Spectrograms");
            showBackButton = true;

            ProvideInitialViewControllers(_settingsView);
        }
    }

    protected override void BackButtonWasPressed(ViewController topViewController)
    {
        _mainFlowCoordinator.DismissFlowCoordinator(this);
    }
}
