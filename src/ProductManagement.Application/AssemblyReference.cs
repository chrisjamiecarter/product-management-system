using System.Reflection;

namespace ProductManagement.Application;

/// <summary>
/// Provides a reference to the application layer assembly.
/// </summary>
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
