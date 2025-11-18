using System;
using System.Collections.Generic;
using System.IO;
using GW2MidiConverter.GuildWars.Instruments;
using Microsoft.Extensions.Logging;

namespace GW2MidiConverter.App.Models;

internal class InstrumentFilesLoader(ILogger<InstrumentFilesLoader> logger) : IInstrumentProvider
{
    private static string InstrumentFolder => Path.Combine("Resources", "Instruments");

    private IReadOnlyCollection<Instrument>? _instruments;

    public IReadOnlyCollection<Instrument> GetInstruments()
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

        List<Instrument> instruments = [];
        foreach (FileInfo file in files)
        {
            try
            {
                Instrument instrument = Instrument.FromFile(file.FullName);
                instruments.Add(instrument);
                logger.LogInformation("Loaded instrument file {FileName}", file.FullName);
            }
            catch (Exception e)
            {
                // Don't re-throw this; a broken file should not crash the program.
                logger.LogError(e, "Failed to load instrument file {FileName}", file.FullName);
            }
        }

        _instruments = instruments;
    }
}