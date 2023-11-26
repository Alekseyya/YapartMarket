using System;

namespace YapartMarket.Core.Config
{
    public sealed class StorageAccountSettings
    {
        public string? ConnectionString { get; init; }
        public string? Container { get; init; }
        public string? ReadFileName { get; init; }
        public string? FileName { get; init; }
    }
}
