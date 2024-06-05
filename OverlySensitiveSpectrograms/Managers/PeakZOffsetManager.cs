using SiraUtil.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OverlySensitiveSpectrograms.Managers;

internal class PeakZOffsetManager : IInitializable, IDisposable
{
    private const string SpectrogramsGameObjectGroupId = "Spectrograms";

    private static readonly int _peakOffsetId = Shader.PropertyToID("_PeakOffset");

    private readonly SiraLog _log;
    private readonly Config _config;
    private readonly EnvironmentGameObjectGroupManager _environmentGameObjectGroupManager;

    private float _currentPeakZOffset;
    private List<MeshRenderer> _meshRenderers = new();
    private Dictionary<MeshRenderer, Vector4> _peakOffsets = new();

    public PeakZOffsetManager(
        SiraLog log,
        Config config,
        EnvironmentGameObjectGroupManager environmentGameObjectGroupManager)
    {
        _log = log;
        _config = config;
        _environmentGameObjectGroupManager = environmentGameObjectGroupManager;
    }

    public void Initialize()
    {
        _config.Updated += Config_Updated;
        _currentPeakZOffset = _config.PeakZOffset;
        _environmentGameObjectGroupManager.Add<Spectrogram>(SpectrogramsGameObjectGroupId);

        var spectrograms = _environmentGameObjectGroupManager.Get<Spectrogram>(SpectrogramsGameObjectGroupId);
        foreach (var spectrogram in spectrograms)
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

    private void Config_Updated(Config config)
    {
        _currentPeakZOffset = config.PeakZOffset;
        SetPeakZOffset(_currentPeakZOffset);
    }

    private void SetPeakZOffset(float zOffset)
    {
        foreach (var meshRenderer in _meshRenderers)
        {
            if (_peakOffsets.TryGetValue(meshRenderer, out var peakOffset))
            {
                _log.Debug($"SetPeakZOffset {meshRenderer.gameObject.name}: X = {peakOffset.x}, Y = {peakOffset.y}, Z = {peakOffset.z}, W = {peakOffset.w}");

                var newPeakOffset = peakOffset;
                newPeakOffset.z *= zOffset;
                meshRenderer.material.SetVector(_peakOffsetId, newPeakOffset);
            }
        }
    }
}
