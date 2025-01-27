using System.Reflection;

namespace ProductManagement.BlazorApp;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
