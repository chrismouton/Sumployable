namespace Sumployable.Domain.Entities;

public class JobApplication : BaseAuditableEntity
{
    public required string RoleName { get; set; }
    
    public string? CompanyName { get; set; }
    
    public RoleType RoleType { get; set; } = RoleType.Permanent;
    
    public Status Status { get; set; } = Status.Active;

    public ProcessStatus ProcessStatus { get; set; } = ProcessStatus.Applied;
    
    public Source Source { get; set; } = Source.JobBoard;
    
    public string? AdvertisedSalary { get; set; }
    
    public string? Url { get; set; }
    
    public string? Location { get; set; }

    public Commute Commute { get; set; } = Commute.Hybrid;
    
    public DateTime ApplicationDate { get; set; }
    
    public string? Note { get; set; }
}
