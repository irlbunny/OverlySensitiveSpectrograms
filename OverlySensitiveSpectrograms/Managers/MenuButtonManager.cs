using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using OverlySensitiveSpectrograms.UI;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zenject;

namespace OverlySensitiveSpectrograms.Managers
{
    internal class MenuButtonManager : IInitializable, IDisposable
    {
        private const string BUTTONTEXT = "<size=80%>Overly Sensitive Spectrograms";
        private const string BUTTONHINT = "These spectrograms wildin' over here.";

        private readonly MainFlowCoordinator _mainFlowCoordinator;
        private readonly OSSFlowCoordinator _ossFlowCoordinator;

        private readonly MenuButton _menuButton;

        public MenuButtonManager(MainFlowCoordinator mainFlowCoordinator, OSSFlowCoordinator ossFlowCoordinator)
        {
            _mainFlowCoordinator = mainFlowCoordinator;
            _ossFlowCoordinator = ossFlowCoordinator;

            _menuButton = new MenuButton(BUTTONTEXT, BUTTONHINT, OnMenuButtonClick);
        }

        public async void Initialize()
        {
            await Task.Run(() => Thread.Sleep(100));

            MenuButtons.instance.RegisterButton(_menuButton);
        }

        public void Dispose()
        {
            if (BSMLParser.IsSingletonAvailable && MenuButtons.IsSingletonAvailable)
                MenuButtons.instance.UnregisterButton(_menuButton);
        }

        private void OnMenuButtonClick()
        {
            _mainFlowCoordinator.PresentFlowCoordinator(_ossFlowCoordinator);
        }
    }
}
