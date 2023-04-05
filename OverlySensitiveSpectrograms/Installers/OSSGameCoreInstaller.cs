using OverlySensitiveSpectrograms.AffinityPatches;
using OverlySensitiveSpectrograms.Managers;
using Zenject;

namespace OverlySensitiveSpectrograms.Installers
{
    internal class OSSGameCoreInstaller : Installer
    {
        public override void InstallBindings()
        {
            // Patches
            Container.BindInterfacesTo<BasicSpectrogramDataPatch>().AsSingle();

            // Managers
            Container.BindInterfacesAndSelfTo<EnvironmentGameObjectGroupManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnvironmentSpectrogramManager>().AsSingle();
        }
    }
}
