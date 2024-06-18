using Microsoft.AspNetCore.Builder;

namespace CommonWeb.Infrastructure;

public abstract class EndpointGroupBase
{
    public abstract void Map(WebApplication app);
}
