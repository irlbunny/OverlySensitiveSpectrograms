#if !LATEST
using BeatSaberMarkupLanguage;
#endif
using BeatSaberMarkupLanguage.MenuButtons;
using OverlySensitiveSpectrograms.UI;
using System;
#if !LATEST
using System.Threading;
using System.Threading.Tasks;
#endif
using Zenject;

namespace OverlySensitiveSpectrograms.Managers;

internal class MenuButtonManager : IInitializable, IDisposable
{
    [Inject] readonly MainFlowCoordinator _mainFlowCoordinator = null!;
    [Inject] readonly OSSFlowCoordinator _ossFlowCoordinator = null!;
#if LATEST
    [Inject] readonly MenuButtons _menuButtons = null!;
#endif

    MenuButton _menuButton = null!;

    public async void Initialize()
    {
        _menuButton = new MenuButton("<size=80%>Overly Sensitive Spectrograms", "These spectrograms wildin' over here.", MenuButton_onClick);

#if LATEST
        _menuButtons.RegisterButton(_menuButton);
#else
        await Task.Run(() => Thread.Sleep(100));
        MenuButtons.instance.RegisterButton(_menuButton);
#endif
    }

    public void Dispose()
    {
#if LATEST
        _menuButtons.UnregisterButton(_menuButton);
#else
        if (BSMLParser.IsSingletonAvailable && MenuButtons.IsSingletonAvailable)
            MenuButtons.instance.UnregisterButton(_menuButton);
#endif
    }

    void MenuButton_onClick()
    {
        _mainFlowCoordinator.PresentFlowCoordinator(_ossFlowCoordinator);
    }
}
