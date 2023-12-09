using BeatSaberMarkupLanguage.MenuButtons;
using OverlySensitiveSpectrograms.UI;
using System;
using Zenject;

namespace OverlySensitiveSpectrograms.Managers
{
    internal class MenuButtonManager : IInitializable, IDisposable
    {
        private const string BUTTONTEXT = "<size=80%>Overly Sensitive Spectrograms";
        private const string BUTTONHINT = "These spectrograms wildin' over here.";

        private readonly MainFlowCoordinator _mainFlowCoordinator;
        private readonly OSSFlowCoordinator _ossFlowCoordinator;
        private readonly MenuButtons _menuButtons;

        private readonly MenuButton _menuButton;

        public MenuButtonManager(MainFlowCoordinator mainFlowCoordinator, OSSFlowCoordinator ossFlowCoordinator, MenuButtons menuButtons)
        {
            _mainFlowCoordinator = mainFlowCoordinator;
            _ossFlowCoordinator = ossFlowCoordinator;
            _menuButtons = menuButtons;

            _menuButton = new MenuButton(BUTTONTEXT, BUTTONHINT, OnMenuButtonClick);
        }

        public void Initialize()
        {
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
}
