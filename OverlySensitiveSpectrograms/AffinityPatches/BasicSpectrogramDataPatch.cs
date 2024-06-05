using SiraUtil.Affinity;
using System.Collections.Generic;
using UnityEngine;

namespace OverlySensitiveSpectrograms.AffinityPatches;

internal class BasicSpectrogramDataPatch : IAffinity
{
    private readonly Config _config;

    public BasicSpectrogramDataPatch(Config config)
    {
        _config = config;
    }

    [AffinityPatch(typeof(BasicSpectrogramData), nameof(BasicSpectrogramData.ProcessSamples)), AffinityPrefix]
    private bool ProcessSamplesPrefix(float[] sourceSamples, List<float> processedSamples)
    {
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
