namespace ProductManagement.Infrastructure.EmailRender.Interfaces;
public interface IRazorViewToStringRenderService
{
    Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
}
