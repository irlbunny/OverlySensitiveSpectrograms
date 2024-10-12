using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OverlySensitiveSpectrograms.Managers;

internal class PeakZOffsetManager : IInitializable, IDisposable
{
    const string SPECTROGRAMSGROUPID = "Spectrograms";

    static readonly int _peakOffsetId = Shader.PropertyToID("_PeakOffset");

    readonly Config _config;
    readonly EnvironmentGOGroupManager _environmentGOGroupManager;

    float _currentPeakZOffset;
    List<MeshRenderer> _meshRenderers = new();
    Dictionary<MeshRenderer, Vector4> _peakOffsets = new();

    public PeakZOffsetManager(Config config, EnvironmentGOGroupManager environmentGOGroupManager)
    {
        _config = config;
        _environmentGOGroupManager = environmentGOGroupManager;
    }

    public void Initialize()
    {
        _config.Updated += Config_Updated;
        _currentPeakZOffset = _config.PeakZOffset;
        _environmentGOGroupManager.Add<Spectrogram>(SPECTROGRAMSGROUPID);

        foreach (var spectrogram in _environmentGOGroupManager.Get<Spectrogram>(SPECTROGRAMSGROUPID))
        {
            _meshRenderers.AddRange(spectrogram._meshRenderers);
        }
        foreach (var meshRenderer in _meshRenderers)
        {
            _peakOffsets.Add(meshRenderer, meshRenderer.material.GetVector(_peakOffsetId));
        }

        SetPeakZOffset(_currentPeakZOffset);
    }

    public void Dispose()
    {
        _config.Updated -= Config_Updated;
        _meshRenderers.Clear();
        _peakOffsets.Clear();
    }

    void Config_Updated(Config config)
    {
        _currentPeakZOffset = config.PeakZOffset;
        SetPeakZOffset(_currentPeakZOffset);
    }

    void SetPeakZOffset(float zOffset)
    {
        foreach (var meshRenderer in _meshRenderers)
        {
            if (_peakOffsets.TryGetValue(meshRenderer, out var peakOffset))
            {
                var newPeakOffset = peakOffset;
                newPeakOffset.z *= zOffset;
                meshRenderer.material.SetVector(_peakOffsetId, newPeakOffset);
            }
        }
    }
}
