using System.Reflection;

namespace ProductManagement.Infrastructure;

/// <summary>
/// Provides a reference to the infrastructure layer assembly.
/// </summary>
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
