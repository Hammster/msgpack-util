# Msgpack Util
![Minimum Dotnet Versions badge](https://img.shields.io/badge/.Net-Core%203.1+%20&%205.0.0+-blue) ![Current Version badge](https://img.shields.io/badge/latest-1.0.0%20alpha-green) ![MIT licence badge](https://img.shields.io/badge/Licence-MIT-000000)

```
msgpack-util:
  Commandline utilities for MessagePack

Usage:
  msgpack-util [options] <inputFile>

Arguments:
  <inputFile>

Options:
  --output-file <output-file>                    File the JSON string is written too
  --compression <Lz4Block|Lz4BlockArray|None>    The type of compression used [default: None]
  --trusted                                      Can this data be trusted (True, False) [default: False]
  --formatted-json                               Do you want to format the json (True, False) [default: True]
  --version                                      Show version information
  -?, -h, --help                                 Show help and usage information
```

# Build instructions

- `dotnet restore`
- `dotnet build --configuration Release`
- Prepare Software for distribution

# Development instructions

- `dotnet restore`
- `dotnet run [options] <inputFile>`

# Changelog

1.0.0-alpha

- Initial version with MessagePack to JSON conversion
