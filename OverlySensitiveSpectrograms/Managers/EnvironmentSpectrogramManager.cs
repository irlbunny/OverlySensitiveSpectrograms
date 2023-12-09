using IPA.Utilities;
using OverlySensitiveSpectrograms.Utilities;
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

        [Inject] private Config _config;
        [Inject] private EnvironmentGameObjectGroupManager _environmentGameObjectGroupManager;

        private float _currentPeakOffset;
        private List<MeshRenderer> _meshRenderers = new();
        private Dictionary<MeshRenderer, Vector4> _peakOffsets = new();

        public void Initialize()
        {
            _config.Updated += Config_Updated;
            _currentPeakOffset = _config.PeakOffset;
            _environmentGameObjectGroupManager.Add<Spectrogram>(SPECTROGRAMGROUPID);

            var spectrograms = _environmentGameObjectGroupManager.Get(SPECTROGRAMGROUPID);
            foreach (var spectrogram in spectrograms)
            {
                var component = spectrogram.GetComponent<Spectrogram>();
                _meshRenderers.AddRange(_meshRenderersAccessor(ref component));
            }

            foreach (var meshRenderer in _meshRenderers)
            {
                var peakOffset = meshRenderer.material.GetVector(PEAKOFFSETID);
                _peakOffsets.Add(meshRenderer, peakOffset);
            }

            SetPeakOffset(_currentPeakOffset);
        }

        public void Dispose()
        {
            _config.Updated -= Config_Updated;

            // TODO(Kaitlyn): Do we even need to reset here?
            /*foreach (var meshRenderer in _meshRenderers)
            {
                if (_peakOffsets.TryGetValue(meshRenderer, out var peakOffset))
                    meshRenderer.material.SetVector(PEAKOFFSETID, peakOffset);
            }*/

            _meshRenderers.Clear();
            _peakOffsets.Clear();
        }

        private void Config_Updated(Config config)
        {
            _currentPeakOffset = config.PeakOffset;
            SetPeakOffset(_currentPeakOffset);
        }

        private void SetPeakOffset(float offset)
        {
            foreach (var meshRenderer in _meshRenderers)
            {
                if (_peakOffsets.TryGetValue(meshRenderer, out var peakOffset))
                {
                    var newPeakOffset = peakOffset;
                    var maxCoordType = VectorUtil.GetMaxCoordType(newPeakOffset);
                    var coordValue = VectorUtil.GetCoordValue(newPeakOffset, maxCoordType);
                    VectorUtil.SetCoordValue(ref newPeakOffset, maxCoordType, coordValue * offset);
                    meshRenderer.material.SetVector(PEAKOFFSETID, newPeakOffset);
                }
            }
        }
    }
}
