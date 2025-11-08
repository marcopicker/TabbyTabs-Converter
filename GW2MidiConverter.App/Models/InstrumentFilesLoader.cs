using System;
using System.Collections.Generic;
using System.IO;
using GW2MidiConverter.GuildWars.Instruments;

namespace GW2MidiConverter.App.Models;

internal class InstrumentFilesLoader : IInstrumentProvider
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
        DirectoryInfo directory = new DirectoryInfo(InstrumentFolder);
        if (!directory.Exists) _instruments = [];

        FileInfo[] files = directory.GetFiles($"*.{Instrument.INSTRUMENT_FILE_EXTENSION}");
        if (files.Length == 0) _instruments = [];

        List<Instrument> instruments = [];
        foreach (FileInfo file in files)
        {
            try
            {
                Instrument instrument = Instrument.FromFile(file.FullName);
                instruments.Add(instrument);
            }
            catch (Exception e)
            {
                // TODO MP: Log this.
                continue;
            }
        }

        _instruments = instruments;
    }
}