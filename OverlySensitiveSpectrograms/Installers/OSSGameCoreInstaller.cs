using OverlySensitiveSpectrograms.AffinityPatches;
using OverlySensitiveSpectrograms.Managers;
using System.Collections.Generic;
using Zenject;

namespace OverlySensitiveSpectrograms.Installers;

internal class OSSGameCoreInstaller : Installer
{
    static readonly List<string> _unsupportedPeakZOffsetEnvironments = new()
    {
        "LinkinParkEnvironment",
        "InterscopeEnvironment",
        "EDMEnvironment",
        "RockMixtapeEnvironment",
        "LinkinPark2Environment",
        "DaftPunkEnvironment",
    };

    readonly GameplayCoreSceneSetupData? _sceneSetupData;

    public OSSGameCoreInstaller([InjectOptional] GameplayCoreSceneSetupData? sceneSetupData)
    {
        _sceneSetupData = sceneSetupData;
    }

    public override void InstallBindings()
    {
        // Patches
        Container.BindInterfacesTo<BasicSpectrogramDataPatch>().AsSingle();

        // Managers
        Container.BindInterfacesAndSelfTo<EnvironmentGOGroupManager>().AsSingle();

        var environmentSceneName = _sceneSetupData != null ? _sceneSetupData.targetEnvironmentInfo.sceneInfo.sceneName : "";
        if (!_unsupportedPeakZOffsetEnvironments.Contains(environmentSceneName))
            Container.BindInterfacesAndSelfTo<PeakZOffsetManager>().AsSingle();
    }
}
