using OverlySensitiveSpectrograms.Managers;
using OverlySensitiveSpectrograms.UI;
using Zenject;

namespace OverlySensitiveSpectrograms.Installers;

internal class OSSMenuInstaller : Installer
{
    public override void InstallBindings()
    {
        // UI
        Container.Bind<OSSSettingsView>().FromNewComponentAsViewController().AsSingle();
        Container.Bind<OSSFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();

        // Managers
        Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle();
    }
}
