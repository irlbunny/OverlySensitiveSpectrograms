using IPA.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace OverlySensitiveSpectrograms.Managers
{
    internal class EnvironmentSpectrogramManager : IInitializable, IDisposable
    {
        private const string SPECTROGRAMGROUPID = "Spectrogram";

        private static readonly int PEAKOFFSETID = Shader.PropertyToID("_PeakOffset");

        private static readonly FieldAccessor<Spectrogram, MeshRenderer[]>.Accessor _meshRenderersAccessor =
            FieldAccessor<Spectrogram, MeshRenderer[]>.GetAccessor("_meshRenderers");

        private readonly Config _config;
        private readonly EnvironmentGameObjectGroupManager _environmentGameObjectGroupManager;

        private float _currentPeakOffset;
        private List<MeshRenderer> _meshRenderers = new();

        public EnvironmentSpectrogramManager(Config config, EnvironmentGameObjectGroupManager environmentGameObjectGroupManager)
        {
            _config = config;
            _environmentGameObjectGroupManager = environmentGameObjectGroupManager;
        }

        public void Initialize()
        {
            _config.Updated += Config_Updated;
            _currentPeakOffset = _config.PeakOffset;
            _environmentGameObjectGroupManager.Add<Spectrogram>(SPECTROGRAMGROUPID);

            GetMeshRenderers();
            SetPeakOffset(_currentPeakOffset);
        }

        public void Dispose()
        {
            _config.Updated -= Config_Updated;

            //SetPeakOffset(_currentPeakOffset, true);

            _meshRenderers.Clear();
        }

        private void Config_Updated(Config config)
        {
            SetPeakOffset(_currentPeakOffset, true);

            _currentPeakOffset = config.PeakOffset;

            SetPeakOffset(_currentPeakOffset);
        }

        private void GetMeshRenderers()
        {
            var spectrograms = _environmentGameObjectGroupManager.Get(SPECTROGRAMGROUPID);
            foreach (var spectrogram in spectrograms)
            {
                var component = spectrogram.GetComponent<Spectrogram>();
                _meshRenderers.AddRange(_meshRenderersAccessor(ref component));
            }
        }

        private void SetPeakOffset(float offset, bool multiplyOrDivide = false)
        {
            foreach (var meshRenderer in _meshRenderers)
            {
                var peakOffset = meshRenderer.material.GetVector(PEAKOFFSETID);
                peakOffset.z = !multiplyOrDivide ? peakOffset.z * offset : peakOffset.z / offset;
                meshRenderer.material.SetVector(PEAKOFFSETID, peakOffset);
            }
        }
    }
}
