using OverlySensitiveSpectrograms.AffinityPatches;
using Zenject;

namespace OverlySensitiveSpectrograms.Installers
{
    internal class OSSGameCoreInstaller : Installer
    {
        public override void InstallBindings()
        {
            // Patches
            Container.BindInterfacesTo<BasicSpectrogramDataPatch>().AsSingle();
        }
    }
}
