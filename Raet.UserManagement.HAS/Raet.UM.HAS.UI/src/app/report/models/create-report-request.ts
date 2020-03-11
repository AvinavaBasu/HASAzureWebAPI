import { IExternalId } from './externalId';

export class ICreateReportRequest {
    application: string;
    permissions: string[];
    sourceUser: IExternalId;
    targetUser: IExternalId;
    startDate: Date;
    endDate: Date;
    fileName: string;
    tenantId: string;
}
