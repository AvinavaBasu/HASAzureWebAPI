namespace Raet.UM.HAS.Core.Reporting
{
    public static class DomainAdapter
    {
        public static Domain.ReportingEvent MapReportingEvent(this DTOs.ReportingEvent dto)
        {
            try
            {
                return new Domain.ReportingEvent
                {
                    Application = dto.Application,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Permissions = dto.Permissions,
                    FileName = dto.FileName,
                    TenantId = dto.TenantId,
                    Source= (Domain.ExternalId)dto.SourceUser,
                    Target = (Domain.ExternalId)dto.TargetUser
                };
            }
            catch (System.Exception ex)
            {
                throw ex.InnerException;
            }
            
        }
    }
}
