using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]

internal static class GlobalAssemblyInformation
{
    static GlobalAssemblyInformation()
    {
        var assembly = Assembly.GetCallingAssembly();
        var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
        AssemblyVersion = assembly.GetName()?.Version?.ToString() ?? throw new InvalidOperationException("The assembly version should be set in targets file.");
        CompanyName = fileVersion?.CompanyName ?? throw new InvalidOperationException("The company name should be set in targets file.");
        ProductName = fileVersion?.ProductName ?? throw new InvalidOperationException("The product name should be set in targets file.");
    }
    public static string AssemblyVersion { get; }
    public static string CompanyName { get; }
    public static string ProductName { get; }
    public const string PublicKey =
        "00240000048000009400000006020000002400005253413100040000010001008919ad97b1e62aae" +
        "79c2201121c505dc2277b0cf27082d8bec6b3f82bc3d59540429e0d8fc359e985723b66e7ca7ab0d" +
        "793f90e3fb7c73d66abb13b2a655075eaed6d744006c47cb6ff105ad4255eab75c0120d76583a0a7" +
        "37dc8e89c1e60b65f534266f33071bd4b71f6b8b93a32f2662b2547f58a0271e8935f41f0107a5cd";
}