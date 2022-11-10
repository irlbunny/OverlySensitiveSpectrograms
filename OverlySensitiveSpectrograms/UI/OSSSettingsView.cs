using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace OverlySensitiveSpectrograms.UI
{
    [ViewDefinition("OverlySensitiveSpectrograms.Views.settings-view.bsml")]
    [HotReload(RelativePathToLayout = @"..\Views\settings-view.bsml")]
    internal class OSSSettingsView : BSMLAutomaticViewController
    {
        private Config _config;

        [UIValue("spectrum-window-choices")]
        private List<object> _spectrumWindowChoices = ((object[])Enum.GetNames(typeof(FFTWindow))).ToList();

        [UIValue("spectrum-window")]
        public string SpectrumWindow
        {
            get => _config.SpectrumWindow.ToString();
            set => _config.SpectrumWindow = (FFTWindow)Enum.Parse(typeof(FFTWindow), value);
        }

        [UIValue("sample-boost")]
        public float SampleBoost
        {
            get => _config.SampleBoost;
            set => _config.SampleBoost = value;
        }

        [UIValue("sample-lerp-0")]
        public float SampleLerp0
        {
            get => _config.SampleLerp0;
            set => _config.SampleLerp0 = value;
        }

        [UIValue("sample-lerp-1")]
        public float SampleLerp1
        {
            get => _config.SampleLerp1;
            set => _config.SampleLerp1 = value;
        }

        [UIValue("use-instant-change-threshold")]
        public bool UseInstantChangeThreshold
        {
            get => _config.UseInstantChangeThreshold;
            set => _config.UseInstantChangeThreshold = value;
        }

        [UIValue("instant-change-threshold")]
        public float InstantChangeThreshold
        {
            get => _config.InstantChangeThreshold;
            set => _config.InstantChangeThreshold = value;
        }

        [Inject]
        public void Construct(Config config)
        {
            _config = config;
        }
    }
}
