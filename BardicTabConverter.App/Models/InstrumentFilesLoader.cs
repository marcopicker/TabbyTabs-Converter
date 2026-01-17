using System;
using System.Collections.Generic;
using System.IO;
using BardicTabConverter.GuildWars.Instruments;
using Microsoft.Extensions.Logging;

namespace BardicTabConverter.App.Models;

internal class InstrumentFilesLoader(ILogger<InstrumentFilesLoader> logger) : IInstrumentProvider
{
    private static string InstrumentFolder => Path.Combine("Resources", "Instruments");

    private IReadOnlyCollection<InstrumentProviderResult>? _instruments;

    public IReadOnlyCollection<InstrumentProviderResult> GetInstruments()
    {
        if (_instruments == null) LoadInstruments();
        return _instruments ?? [];
    }

    private void LoadInstruments()
    {
        using IDisposable? _ = logger.BeginScope(new
        {
            Operation = "Loading instrument files",
            Folder = InstrumentFolder
        });

        DirectoryInfo directory = new DirectoryInfo(InstrumentFolder);
        if (!directory.Exists)
        {
            logger.LogWarning("Instrument folder does not exist.");
            _instruments = [];
        }

        FileInfo[] files = directory.GetFiles($"*.{Instrument.INSTRUMENT_FILE_EXTENSION}");
        if (files.Length == 0)
        {
            logger.LogWarning("No instrument files found in provided folder.");
            _instruments = [];
        }

        List<InstrumentProviderResult> instruments = [];
        foreach (FileInfo file in files)
        {
            try
            {
                Instrument instrument = Instrument.FromFile(file.FullName);
                instruments.Add(new InstrumentProviderResult(instrument, file.FullName));
                logger.LogInformation("Loaded instrument file {FileName}", file.FullName);
            }
            catch (Exception e)
            {
                // Don't re-throw this; an invalid instrument file should not crash the program.
                logger.LogError(e, "Failed to load instrument file {FileName}", file.FullName);
                instruments.Add(new InstrumentProviderResult(null, file.FullName, false, e.Message));
            }
        }

        _instruments = instruments;
    }
}