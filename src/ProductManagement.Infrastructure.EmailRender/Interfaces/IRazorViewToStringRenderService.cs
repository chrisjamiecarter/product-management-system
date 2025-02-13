namespace ProductManagement.Infrastructure.EmailRender.Interfaces;

/// <summary>
/// Defines the service for rendering a razor view to a string.
/// </summary>
public interface IRazorViewToStringRenderService
{
    Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
}
