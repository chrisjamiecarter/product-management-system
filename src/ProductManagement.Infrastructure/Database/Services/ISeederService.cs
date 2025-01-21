namespace ProductManagement.Infrastructure.Database.Services;

internal interface ISeederService
{
    Task SeedDatabaseAsync();
}