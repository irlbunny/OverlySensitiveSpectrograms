﻿using IPA;
using IPA.Config.Stores;
using OverlySensitiveSpectrograms.Installers;
using SiraUtil.Attributes;
using SiraUtil.Zenject;
using IPAConfig = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace OverlySensitiveSpectrograms;

[Plugin(RuntimeOptions.DynamicInit), NoEnableDisable, Slog]
public class Plugin
{
    [Init]
    public Plugin(IPALogger logger, IPAConfig conf, Zenjector zenjector)
    {
        zenjector.UseLogger(logger);

        var config = conf.Generated<Config>();
        zenjector.Install(Location.App, Container =>
        {
            Container.BindInstance(config).AsSingle();
        });

        zenjector.Install<OSSMenuInstaller>(Location.Menu);
        zenjector.Install<OSSGameCoreInstaller>(Location.GameCore);
    }
}
