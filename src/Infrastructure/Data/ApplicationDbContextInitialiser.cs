using Sumployable.Domain.Constants;
using Sumployable.Domain.Entities;
using Sumployable.Domain.Enums;
using Sumployable.Domain.ValueObjects;
using Sumployable.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Sumployable.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            // See https://jasontaylor.dev/ef-core-database-initialisation-strategies
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }

        // Default job applications
        if (!_context.JobApplications.Any())
        {
            _context.JobApplications.AddRange(
                new JobApplication { RoleName = "Senior Agile Delivery Manager", CompanyName = "Workonomics", RoleType = RoleType.Permanent, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£85K/yr - £100K/yr", Url = "https://www.linkedin.com/jobs/view/3890137057/?trackingId=9WBJ0QvbStedUqa1R%2BRL1w%3D%3D&refId=BeRIV5KtTCGxt0nOZBEpmw%3D%3D&midToken=AQHCoeH_Y6Hylw&midSig=1aNvIWOAJZIrc1&trk=eml-email_jobs_viewed_job_reminder_01-job_card-0-jobcard_body&trkEmail=eml-email_jobs_viewed_job_reminder_01-job_card-0-jobcard_body-null-11m6ht~lv1bixri~2d-null-null&eid=11m6ht-lv1bixri-2d&otpToken=MTQwNjFmZTgxMjI4Y2RjNWJlMmYwMmViNDUxY2U2YjE4N2NhZDI0OTlhYTY4NTYyNzVjZTA3NmQ0ZjUzNWRmMmZjOGNhZmJhNTNiNWNlODc1Y2EyN2M4Mjg5YzBlYjFmOTZjZDc3NWIwYWUyNTg5ZDEyLDEsMQ%3D%3D", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today },
                new JobApplication { RoleName = "IT Manager", CompanyName = "Unknown", RoleType = RoleType.Permanent, Status = Status.Closed, ProcessStatus = ProcessStatus.Rejected, Source = Source.JobBoard, AdvertisedSalary = "£75K", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today },
                new JobApplication { RoleName = "Technical Project Manager", CompanyName = "Builder.ai", RoleType = RoleType.Permanent, Status = Status.Closed, ProcessStatus = ProcessStatus.Rejected, Source = Source.JobBoard, AdvertisedSalary = "Up to £70K", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today },
                new JobApplication { RoleName = "Scrum Master", CompanyName = "Pioneer Search", RoleType = RoleType.Contract, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£600 daily - £700 daily", Url = "https://www.linkedin.com/jobs/view/3888069492/?midToken=AQHCoeH_Y6Hylw&midSig=2Kiuh1g0m_IXc1&trk=eml-email_application_confirmation_with_nba_01-jymbii-3-job_card_view&trkEmail=eml-email_application_confirmation_with_nba_01-jymbii-3-job_card_view-null-11m6ht%7Elv210eq3%7Eyr-null-neptune%2Fjobs%2Eview", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-2) },
                new JobApplication { RoleName = "Agile Coach", CompanyName = "Candour Solutions", RoleType = RoleType.Permanent, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "Unknown", Url = "https://www.linkedin.com/jobs/view/3892441363/?midToken=AQHCoeH_Y6Hylw&midSig=2Kiuh1g0m_IXc1&trk=eml-email_application_confirmation_with_nba_01-jymbii-4-job_card_view&trkEmail=eml-email_application_confirmation_with_nba_01-jymbii-4-job_card_view-null-11m6ht%7Elv210eq3%7Eyr-null-neptune%2Fjobs%2Eview", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-2) },
                new JobApplication { RoleName = "Business Project Manager", CompanyName = "Primis", RoleType = RoleType.Contract, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£550/day Outside", Url = "https://www.linkedin.com/jobs/view/3897184234", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-2) },
                new JobApplication { RoleName = "Business Project Manager", CompanyName = "i3", RoleType = RoleType.Contract, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£81.25/hr", Url = "https://www.linkedin.com/jobs/view/3899378395", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-2) },
                new JobApplication { RoleName = "Mini-Projects Manager", CompanyName = "Hays Recruitment", RoleType = RoleType.Contract, Status = Status.Closed, ProcessStatus = ProcessStatus.Rejected, Source = Source.JobBoard, AdvertisedSalary = "£600/yr - £700/yr", Url = "https://www.linkedin.com/jobs/view/3901874765", Location = "London", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-3) },
                new JobApplication { RoleName = "Agile Delivery Manager", CompanyName = "Hays Recruitment", RoleType = RoleType.Contract, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£500 - £650 Inside IR35", Url = "https://www.hays.co.uk/job-detail/agile-delivery-manager---product-london_4527603", Commute = Commute.Hybrid, ApplicationDate =DateTime.Today.AddDays(-3) },
                new JobApplication { RoleName = "Lean Programme Manager", CompanyName = "Hays Recruitment", RoleType = RoleType.Contract, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "460 per day (Outside IR35)", Url = "https://www.hays.co.uk/job-detail/lean-programme-manager-%25E2%2580%2593-agile-scrum-delivery-assurance-london_4537916?q=", Commute = Commute.Hybrid, ApplicationDate =DateTime.Today.AddDays(-3) },
                new JobApplication { RoleName = "Project Manager", CompanyName = "Hays Recruitment", RoleType = RoleType.Permanent, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£90,000.00", Url = "https://www.hays.co.uk/job-detail/project-manager-farnborough_4515070/apply?applyId=JOB_5185912&jobTitle=Project%20Manager", Commute = Commute.Hybrid, ApplicationDate =DateTime.Today.AddDays(-4) },
                new JobApplication { RoleName = "Senior Delivery Manager- Housing Digital Transformation", CompanyName = "Akton Recruitment Ltd", RoleType = RoleType.Contract, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£549.20 per day", Url = "https://clicks.reed.co.uk/f/a/yP1wjqbajbxOkawZP0MF_A~~/AAAHagA~/RgRoALNjP0TkaHR0cHM6Ly93d3cucmVlZC5jby51ay9qb2JzL3Nlbmlvci1kZWxpdmVyeS1tYW5hZ2VyLWhvdXNpbmctZGlnaXRhbC10cmFuc2Zvcm1hdGlvbi81MjM4NjAzMT91dG1fc291cmNlPXRyYW5zYWN0aW9uYWwmdXRtX2NhbXBhaWduPWluX2pvX2IyY19lbV90cmFfcmV0X2FwcGxpY2F0aW9uX2pvYmFwcGxpY2F0aW9uY29uZmlybWF0aW9ucmVjcnVpdG1lbnRjb25zdWx0YW5jeSZ1dG1fbWVkaXVtPWVtYWlsVwVzcGNldUIKZhRjLh5mNfJGsFIXY2hyaXNtb3V0b243NEBnbWFpbC5jb21YBAAAALE~", Commute = Commute.Hybrid, ApplicationDate =DateTime.Today.AddDays(-4) },
                new JobApplication { RoleName = "Agile Delivery Manager", CompanyName = "Triad Group", RoleType = RoleType.Permanent, Status = Status.Closed, ProcessStatus = ProcessStatus.Rejected, Source = Source.JobBoard, AdvertisedSalary = "Unknown", Url = "Lon", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-5) },
                new JobApplication { RoleName = "Project Manager", CompanyName = "Lorien", RoleType = RoleType.Contract, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£400.00 - £425.00 per day", Url = "https://www.reed.co.uk/jobs/project-manager/52485261?utm_source=transactional&utm_campaign=in_jo_b2c_em_tra_ret_application_jobapplicationconfirmationrecruitmentconsultancy&utm_medium=email", Commute = Commute.Hybrid, ApplicationDate =DateTime.Today.AddDays(-6) },
                new JobApplication { RoleName = "IT Project Manager Data Analytics", CompanyName = "Hays Recruitment", RoleType = RoleType.Contract, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£500 - £600 per day", Url = "https://www.reed.co.uk/jobs/it-project-manager-data-analytics/52490257?source=searchResults&filter=%2Fjobs%2Ftechnology-project-manager-jobs-in-watford%3Fproximity%3D50", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-6) },
                new JobApplication { RoleName = "Project Manager - IT", CompanyName = "Robert Half", RoleType = RoleType.Permanent, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£85,000.00", Url = "https://www.reed.co.uk/jobs/project-manager-it/52428300?source=searchResults&filter=%2Fjobs%2Ftechnology-project-manager-jobs-in-watford%3Fproximity%3D50", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-6) },
                new JobApplication { RoleName = "Technical Project Manager - hybrid", CompanyName = "HW Select Ltd", RoleType = RoleType.Permanent, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£60,000 - £80,000", Url = "https://www.reed.co.uk/jobs/technical-project-manager-hybrid/52482479?source=searchResults&filter=%2Fjobs%2Ftechnology-project-manager-jobs-in-watford%3Fproximity%3D50%26pageno%3D2", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-7) },
                new JobApplication { RoleName = "Senior Agile Delivery Manager", CompanyName = "DWP", RoleType = RoleType.Permanent, Status = Status.Closed, ProcessStatus = ProcessStatus.Rejected, Source = Source.JobBoard, AdvertisedSalary = "£50,000 - £75,000", Url = "https://www.civilservicejobs.service.gov.uk/csr/jobs.cgi?jcode=1905627", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-7) },
                new JobApplication { RoleName = "Housing IT Project Manager", CompanyName = "AKTON Resourcing Ltd", RoleType = RoleType.Contract, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£600/day Outside IR35", Url = "https://www.cv-library.co.uk/job/221456321/Housing-IT-Project-Manager?utm_source=system&utm_medium=email&utm_campaign=1501_application_confirmation", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-8) },
                new JobApplication { RoleName = "Agile Project Manager", CompanyName = "Senitor Associates Ltd", RoleType = RoleType.Permanent, Status = Status.Expired, ProcessStatus = ProcessStatus.Applied, Source = Source.JobBoard, AdvertisedSalary = "£60,000 - £75,000", Url = "https://www.cv-library.co.uk/job/221462521/Agile-Project-Manager?hlkw=agile-delivery-manager&sid=820aee9d-7aeb-43b0-850f-d0c814bfc4f8", Commute = Commute.Hybrid, ApplicationDate = DateTime.Today.AddDays(-8) }
            );
            await _context.SaveChangesAsync();
        }
    }
}
