using OverlySensitiveSpectrograms.AffinityPatches;
using OverlySensitiveSpectrograms.Managers;
using System.Collections.Generic;
using Zenject;

namespace OverlySensitiveSpectrograms.Installers;

internal class OSSGameCoreInstaller : Installer
{
    private static readonly List<string> _unsupportedPeakZOffsetEnvironments = new()
    {
        "LinkinParkEnvironment",
        "InterscopeEnvironment",
        "EDMEnvironment",
        "RockMixtapeEnvironment",
        "LinkinPark2Environment",
        "DaftPunkEnvironment",
    };

    private readonly GameplayCoreSceneSetupData _sceneSetupData;

    public OSSGameCoreInstaller(GameplayCoreSceneSetupData sceneSetupData)
    {
        _sceneSetupData = sceneSetupData;
    }

    public override void InstallBindings()
    {
        // Patches
        Container.BindInterfacesTo<BasicSpectrogramDataPatch>().AsSingle();

        // Managers
        Container.BindInterfacesAndSelfTo<EnvironmentGameObjectGroupManager>().AsSingle();

        var environmentSceneName = _sceneSetupData.environmentInfo.sceneInfo.sceneName;
        if (!_unsupportedPeakZOffsetEnvironments.Contains(environmentSceneName))
            Container.BindInterfacesAndSelfTo<PeakZOffsetManager>().AsSingle();
    }
}
