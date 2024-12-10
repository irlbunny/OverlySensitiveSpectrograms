using SiraUtil.Affinity;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OverlySensitiveSpectrograms.AffinityPatches;

internal class BasicSpectrogramDataPatch : IAffinity
{
    readonly Config _config;
#if LATEST
    readonly EnvironmentEffectsFilterPreset _environmentEffectsFilterPreset = EnvironmentEffectsFilterPreset.AllEffects;
    readonly bool _staticLights;

    public BasicSpectrogramDataPatch(
        Config config,
        [InjectOptional] GameplayCoreSceneSetupData? gameplayCoreSceneSetupData)
#else
    public BasicSpectrogramDataPatch(Config config)
#endif
    {
        _config = config;

#if LATEST
        if (gameplayCoreSceneSetupData != null)
        {
            _environmentEffectsFilterPreset = gameplayCoreSceneSetupData.beatmapKey.difficulty == BeatmapDifficulty.ExpertPlus ?
                gameplayCoreSceneSetupData.playerSpecificSettings.environmentEffectsFilterExpertPlusPreset :
                gameplayCoreSceneSetupData.playerSpecificSettings.environmentEffectsFilterDefaultPreset;
        }

        _staticLights = _environmentEffectsFilterPreset == EnvironmentEffectsFilterPreset.NoEffects;
#endif
    }

    [AffinityPatch(typeof(BasicSpectrogramData), nameof(BasicSpectrogramData.ProcessSamples)), AffinityPrefix]
    bool ProcessSamplesPrefix(float[] sourceSamples, List<float> processedSamples)
    {
#if LATEST
        if (_staticLights && _config.DisableSpectrogramsInStaticLights)
            return false;
#endif

        var deltaTime = Time.deltaTime;

        for (var i = 0; i < sourceSamples.Length; i++)
        {
            var sample = Mathf.Log(sourceSamples[i] + 1f) * (i + 1) * _config.SampleBoost;
            if (processedSamples[i] < sample)
            {
                if (_config.UseInstantChangeThreshold && sample - processedSamples[i] > _config.InstantChangeThreshold)
                    processedSamples[i] = sample;
                else
                    processedSamples[i] = Mathf.Lerp(processedSamples[i], sample, deltaTime * _config.SampleLerp0);
            }
            else
                processedSamples[i] = Mathf.Lerp(processedSamples[i], sample, deltaTime * _config.SampleLerp1);
        }

        return false;
    }
}
