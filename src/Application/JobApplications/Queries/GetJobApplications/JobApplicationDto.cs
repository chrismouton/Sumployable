using Sumployable.Domain.Entities;

namespace Sumployable.Application.JobApplications.Queries.GetJobApplications;

public class JobApplicationDto
{
    public int Id { get; init; }

    public string RoleName { get; init; } = string.Empty;

    public string? CompanyName { get; init; }

    public int RoleType { get; init; }

    public int Status { get; init; }

    public int ProcessStatus { get; init; }

    public int Source { get; init; }

    public string? AdvertisedSalary { get; init; }

    public string? Url { get; init; }

    public string? Location { get; init; }

    public int Commute { get; init; }

    public DateTime ApplicationDate { get; init; }

    public string? Note { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<JobApplication, JobApplicationDto>()
                .ForMember(d => d.RoleType, opt => opt.MapFrom(s => (int)s.RoleType))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (int)s.Status))
                .ForMember(d => d.ProcessStatus, opt => opt.MapFrom(s => (int)s.ProcessStatus))
                .ForMember(d => d.Source, opt => opt.MapFrom(s => (int)s.Source))
                .ForMember(d => d.Commute, opt => opt.MapFrom(s => (int)s.Commute));
        }
    }
}
