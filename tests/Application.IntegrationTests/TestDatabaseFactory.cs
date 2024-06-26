﻿namespace Application.IntegrationTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
#if DEBUG
        var database = new SqlServerTestDatabase();
#else
        var database = new TestcontainersTestDatabase();
#endif
        
        await database.InitialiseAsync();

        return database;
    }
}
