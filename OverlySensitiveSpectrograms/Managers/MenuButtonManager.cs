#if !LATEST
using BeatSaberMarkupLanguage;
#endif
using BeatSaberMarkupLanguage.MenuButtons;
using OverlySensitiveSpectrograms.UI;
using System;
using Zenject;

namespace OverlySensitiveSpectrograms.Managers;

internal class MenuButtonManager : IInitializable, IDisposable
{
    private readonly MainFlowCoordinator _mainFlowCoordinator;
    private readonly OSSFlowCoordinator _ossFlowCoordinator;
    private readonly MenuButtons _menuButtons;

    private MenuButton _menuButton = null!;

    public MenuButtonManager(
        MainFlowCoordinator mainFlowCoordinator,
        OSSFlowCoordinator ossFlowCoordinator,
        MenuButtons menuButtons)
    {
        _mainFlowCoordinator = mainFlowCoordinator;
        _ossFlowCoordinator = ossFlowCoordinator;
        _menuButtons = menuButtons;
    }

    public void Initialize()
    {
        _menuButton = new MenuButton("<size=80%>Overly Sensitive Spectrograms", "These spectrograms wildin' over here.", OnMenuButtonClick);
        _menuButtons.RegisterButton(_menuButton);
    }

    public void Dispose()
    {
        _menuButtons.UnregisterButton(_menuButton);
    }

    private void OnMenuButtonClick()
    {
        _mainFlowCoordinator.PresentFlowCoordinator(_ossFlowCoordinator);
    }
}
