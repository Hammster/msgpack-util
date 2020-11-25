using System.IO; 
using System.Text;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json;

using MessagePack;

namespace msgpack_util
{
    struct ProgramOptions {
        public string inputFilePath;
        public string outputFilePath;
        public MessagePackCompression compression;
        public bool trusted;
        public bool formattedJson;
    }

    class Program
    {
        static ProgramOptions options;

        static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Argument<FileInfo>("inputFile"),
                new Option<FileInfo>(
                    "--output-file",
                    description: "File the JSON string is written too"),
                new Option<MessagePackCompression>(
                    "--compression",
                    getDefaultValue: () => MessagePackCompression.None,
                    description: "The type of compression used"),
                new Option<bool>(
                    "--trusted",
                    getDefaultValue: () => false,
                    description: "Can this data be trusted (True, False)"),
                new Option<bool>(
                    "--formatted-json",
                    getDefaultValue: () => true,
                    description: "Do you want to format the json (True, False)"),
            };

            rootCommand.Description = "Commandline utilities for MessagePack";

            rootCommand.Handler = CommandHandler.Create<FileInfo, FileInfo, MessagePackCompression, bool, bool>((inputFile, outputFile, compression, trusted, formattedJson) =>
            {
                var determinedOutputFile = outputFile ?? new FileInfo($"{inputFile.FullName}.json");
                HandleOptions(inputFile, determinedOutputFile, compression, trusted, formattedJson);
            });

            return rootCommand.InvokeAsync(args).Result;
        }

        static void HandleOptions (FileInfo inputFile, FileInfo outputFile, MessagePackCompression compression, bool trusted, bool formattedJson)
        {
            options.inputFilePath = inputFile.FullName;
            options.outputFilePath = outputFile.FullName;
            options.compression = compression;
            options.trusted = trusted;
            options.formattedJson = formattedJson;
            ConfigureMessagepack();
            WriteJson();
        }

        static void ConfigureMessagepack()
        {
            MessagePackSerializer.DefaultOptions = MessagePackSerializerOptions.Standard
                .WithSecurity(options.trusted ? MessagePackSecurity.TrustedData : MessagePackSecurity.UntrustedData)
                .WithCompression(options.compression);
        }

        static void WriteJson()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = options.formattedJson,
            };

            var fileData = File.ReadAllBytes(options.inputFilePath);
            var jsonString = MessagePackSerializer.ConvertToJson(fileData, MessagePackSerializer.DefaultOptions);

            var output = options.formattedJson 
                ? JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonDocument>(jsonString), jsonOptions) 
                : jsonString;

            System.IO.File.WriteAllBytes(options.outputFilePath, Encoding.ASCII.GetBytes(output));
        }
    }
}
