using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.XPath;
using GW2MidiConverter.GuildWars.Instruments;
using GW2MidiConverter.Midi;
using GW2MidiConverter.Rendering;
using GW2MidiConverter.Tab;
using GW2MidiConverter.Tab.Octaves;

namespace GW2MidiConverter.CLI;

[SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters")]
internal static class Program
{
    private static int Main(string[] args)
    {
        RootCommand rootCommand = new RootCommand("GW2MidiConverter.CLI.exe is a tool that allows you to convert MIDI files into human-readable Guild Wars 2 music notation.");

        Argument<FileInfo> pathArgument = new Argument<FileInfo>("QuickFilePath")
        {
            Description = "The path to the MIDI file to convert",
            Arity = ArgumentArity.ZeroOrOne
        };
        rootCommand.Add(pathArgument);

        Option<FileInfo> inputFileOption = new Option<FileInfo>("--input", "-i")
        {
            Description = "The path to the MIDI file to convert",
            Required = true
        };

        Option<FileInfo> outputFileOption = new Option<FileInfo>("--output", "-o")
        {
            Description = "The path to the output file(s)."
        };

        Option<bool> explicitTextFileOption = new Option<bool>("--text-file", "-txt")
        {
            Description = "Whether to output the tab in a text file or not",
        };

        Option<bool> explicitCsvFileOption = new Option<bool>("--csv-file", "-csv")
        {
            Description = "Whether to output the tab in several CSV files or not",
        };

        Command convertToTabCommand = new Command("convert", "Converts a MIDI file into tablature.")
        {
            inputFileOption,
            outputFileOption,
            explicitTextFileOption,
            explicitCsvFileOption
        };
        rootCommand.Subcommands.Add(convertToTabCommand);

        convertToTabCommand.SetAction(result => ConvertToTab(
            result.GetValue(inputFileOption),
            result.GetValue(outputFileOption),
            result.GetValue(explicitTextFileOption),
            result.GetValue(explicitCsvFileOption))
        );

        ParseResult result = rootCommand.Parse(args);
        FileInfo? pathArgumentValue = result.GetValue(pathArgument);
        int exitCode = 0;
        if (pathArgumentValue?.Exists ?? false)
        {
            string outputFile = pathArgumentValue.FullName + ".txt";
            exitCode = ConvertToTab(pathArgumentValue, new FileInfo(outputFile), true, false);
            Console.WriteLine("I think it worked!");
            Console.WriteLine("Press any key to exit...");
        }
        else
        {
            exitCode = result.Invoke();
        }
        
        Console.ReadKey();
        return exitCode;
    }

    private static int ConvertToTab(FileInfo? inputFile, FileInfo? outputFile, bool explicitTextFile, bool explicitCsvFile)
    {
        ArgumentNullException.ThrowIfNull(inputFile);

        if (!inputFile.Exists)
        {
            WriteError($"The file at {inputFile.FullName} does not exist!");
            return -1;
        }

        try
        {
            MidiDocument midiDocument = MidiDocument.LoadFromMidiFile(inputFile.FullName);
            TabDocument tabDocument = TabDocument.FromMidiDocument(midiDocument, new LookAheadOctaveCalculator());

            Dictionary<int, Instrument> instrumentsByTrack = new Dictionary<int, Instrument>();
            foreach (MidiTrack midiTrack in midiDocument.GetTracks())
            {
                string instrumentName = midiTrack.Info.InstrumentName;
                if (instrumentName.Contains("Piano", StringComparison.InvariantCultureIgnoreCase))
                {
                    instrumentsByTrack[midiTrack.Info.TrackIndex] = Instrument.Piano;
                }
                else if (instrumentName.Contains("Bass", StringComparison.InvariantCultureIgnoreCase))
                {
                    instrumentsByTrack[midiTrack.Info.TrackIndex] = Instrument.Bass;
                }
                else if (instrumentName.Contains("Minstrel", StringComparison.InvariantCultureIgnoreCase))
                {
                    instrumentsByTrack[midiTrack.Info.TrackIndex] = Instrument.Minstrel;
                }
            }

            foreach (KeyValuePair<int, Instrument> pair in instrumentsByTrack)
            {
                tabDocument.EnableTrack(pair.Key, pair.Value);
            }

            bool createTextFile = (explicitTextFile == false && explicitCsvFile == false) || explicitTextFile;
            if (createTextFile)
            {
                TabRenderer renderer = new TabRenderer(tabDocument);
                string output = renderer.Render();
                if (outputFile != null)
                {
                    File.WriteAllText(outputFile.FullName, output);
                }
                else
                {
                    Console.WriteLine(output);
                }
            }
        }
#pragma warning disable CA1031
        catch (Exception e)
#pragma warning restore CA1031
        {
            WriteError(e.Message);
            return -1;
        }

        return 0;
    }

    private static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(message);
        Console.ResetColor();
    }
}