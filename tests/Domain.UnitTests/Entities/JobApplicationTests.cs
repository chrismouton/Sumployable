using Sumployable.Domain.Common;
using Sumployable.Domain.Entities;
using Sumployable.Domain.Enums;
using NUnit.Framework;
using Shouldly;

namespace Sumployable.Domain.UnitTests.Entities;

public class JobApplicationTests
{
    private static JobApplication NewApplication() =>
        new() { RoleName = "Software Engineer" };

    [Test]
    public void ShouldDefaultRoleTypeToPermanent()
    {
        NewApplication().RoleType.ShouldBe(RoleType.Permanent);
    }

    [Test]
    public void ShouldDefaultStatusToActive()
    {
        NewApplication().Status.ShouldBe(Status.Active);
    }

    [Test]
    public void ShouldDefaultProcessStatusToApplied()
    {
        NewApplication().ProcessStatus.ShouldBe(ProcessStatus.Applied);
    }

    [Test]
    public void ShouldDefaultSourceToJobBoard()
    {
        NewApplication().Source.ShouldBe(Source.JobBoard);
    }

    [Test]
    public void ShouldDefaultCommuteToHybrid()
    {
        NewApplication().Commute.ShouldBe(Commute.Hybrid);
    }

    [Test]
    public void ShouldPersistAssignedRoleName()
    {
        var application = new JobApplication { RoleName = "Software Engineer" };

        application.RoleName.ShouldBe("Software Engineer");
    }

    [Test]
    public void ShouldDefaultOptionalStringPropertiesToNull()
    {
        var application = NewApplication();

        application.CompanyName.ShouldBeNull();
        application.AdvertisedSalary.ShouldBeNull();
        application.Url.ShouldBeNull();
        application.Location.ShouldBeNull();
        application.Note.ShouldBeNull();
    }

    [Test]
    public void ShouldRoundTripAllAssignedProperties()
    {
        var applicationDate = new DateTime(2026, 4, 21, 9, 30, 0, DateTimeKind.Utc);

        var application = new JobApplication
        {
            RoleName = "Senior Platform Engineer",
            CompanyName = "Acme Corp",
            RoleType = RoleType.Contract,
            Status = Status.Closed,
            ProcessStatus = ProcessStatus.OfferReceived,
            Source = Source.Networking,
            AdvertisedSalary = "£100,000",
            Url = "https://example.com/jobs/42",
            Location = "London",
            Commute = Commute.Remote,
            ApplicationDate = applicationDate,
            Note = "Referred by a former colleague"
        };

        application.RoleName.ShouldBe("Senior Platform Engineer");
        application.CompanyName.ShouldBe("Acme Corp");
        application.RoleType.ShouldBe(RoleType.Contract);
        application.Status.ShouldBe(Status.Closed);
        application.ProcessStatus.ShouldBe(ProcessStatus.OfferReceived);
        application.Source.ShouldBe(Source.Networking);
        application.AdvertisedSalary.ShouldBe("£100,000");
        application.Url.ShouldBe("https://example.com/jobs/42");
        application.Location.ShouldBe("London");
        application.Commute.ShouldBe(Commute.Remote);
        application.ApplicationDate.ShouldBe(applicationDate);
        application.Note.ShouldBe("Referred by a former colleague");
    }

    [Test]
    public void ShouldStartWithNoDomainEvents()
    {
        NewApplication().DomainEvents.ShouldBeEmpty();
    }

    [Test]
    public void ShouldAddAndClearDomainEvents()
    {
        var application = NewApplication();
        var domainEvent = new TestEvent();

        application.AddDomainEvent(domainEvent);
        application.DomainEvents.ShouldHaveSingleItem().ShouldBe(domainEvent);

        application.ClearDomainEvents();
        application.DomainEvents.ShouldBeEmpty();
    }

    private sealed class TestEvent : BaseEvent;
}
