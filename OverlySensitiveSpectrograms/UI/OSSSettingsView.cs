using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using Zenject;

namespace OverlySensitiveSpectrograms.UI;

[ViewDefinition("OverlySensitiveSpectrograms.Views.settings-view.bsml")]
[HotReload(RelativePathToLayout = @"..\Views\settings-view.bsml")]
internal class OSSSettingsView : BSMLAutomaticViewController
{
    Config _config = null!;

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

    [UIValue("peak-z-offset")]
    public float PeakZOffset
    {
        get => _config.PeakZOffset;
        set => _config.PeakZOffset = value;
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

    [UIValue("disable-spectrograms-in-static-lights")]
    public bool DisableSpectrogramsInStaticLights
    {
        get => _config.DisableSpectrogramsInStaticLights;
        set => _config.DisableSpectrogramsInStaticLights = value;
    }

    [Inject]
    public void Construct(Config config)
    {
        _config = config;
    }
}
