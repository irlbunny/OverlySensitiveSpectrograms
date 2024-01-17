using SiraUtil.Affinity;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OverlySensitiveSpectrograms.AffinityPatches
{
    internal class BasicSpectrogramDataPatch : IAffinity
    {
        [Inject] private Config _config;

        [AffinityPatch(typeof(BasicSpectrogramData), "ProcessSamples"), AffinityPrefix]
        private bool ProcessSamples(float[] sourceSamples, List<float> processedSamples)
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
}
