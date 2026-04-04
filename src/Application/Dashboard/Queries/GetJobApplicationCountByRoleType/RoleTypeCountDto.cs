namespace Sumployable.Application.Dashboard.Queries.GetJobApplicationCountByRoleType;

public class RoleTypeCountDto
{
    public int RoleType { get; init; }
    public string RoleTypeName { get; init; } = string.Empty;
    public int Count { get; init; }
}
