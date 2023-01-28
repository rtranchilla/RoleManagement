using RoleManager.DataPersistence;

namespace RoleManager.WebService.Configuration;

#if DEBUG
public class TestData
{
    public TestData() { }

    private readonly RoleDbContext dbContext;

    public TestData(RoleDbContext dbContext) => this.dbContext = dbContext;

}
#endif