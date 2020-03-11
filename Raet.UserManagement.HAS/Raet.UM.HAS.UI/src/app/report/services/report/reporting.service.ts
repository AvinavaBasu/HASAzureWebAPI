import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ICreateReportRequest } from '../../models/create-report-request';
import { map } from 'rxjs/operators';
import { ConfigService } from 'src/app/services/configservice.service';
import { ICustomUser } from '../../models/customUsers';

@Injectable({
  providedIn: 'root'
})
export class ReportingService {
base: string;

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.base= this.configService.getConfig().reportingBaseUrl;
    }

  getApplications(tenant: string): Observable<any> {
    const endPoint = this.base + 'ReportDetail/Application/' + tenant;
    return this.http.get(endPoint);
  }

  getPermissions(app: string, tenant: string): Observable<any> {
    const endPoint = this.base + 'ReportDetail/Permission/' + app + '/' + tenant;
    return this.http.get<IPermission[]>(endPoint);
  }

  async getUsers(val:string,perList: string[],app: string,tenant: string): Promise<any> {
      const endPoint = this.base+'ReportDetail/'+val+'User/'+app+'/'+tenant;
      return await Promise.resolve(this.http.post<ICustomUser[]>(endPoint,perList));
  }

  createReport(request: ICreateReportRequest): Observable<any> {
    const endPoint = this.base + 'Report/Generate';
    return this.http.post<Observable<any>>(endPoint, request);
  }

  getReports(tenant: string): Observable<IReport[]> {
    const endPoint = this.base + 'Report/Get/' + tenant;
    return this.http.get<IReport[]>(endPoint);
  }

  getDownloadUrl(tenant: string, id: string): Observable<any> {
    const endPoint = this.base + 'Report/Download?tenantId=' + tenant + '&guid=' + id;
    return this.http.get(endPoint, {
      responseType: 'blob',
      observe: 'response'
  })
      .pipe(
          map((res: any) => {
              return new Blob([res.body], { type: 'text/csv' });
          })
      );
  }

  // downloadReport(tenant: string, id: string | number): Observable<any> {
  //   const endPoint = 'https://raetgdpr-eventreporting-dev.azurewebsites.net/api/Report/Download?tenantId=' + tenant + '&guid=' + id;
  //   return this.http.get(endPoint,  {
  //     headers: new HttpHeaders().set('Content-Type', 'application/json'),
  //   }).pipe(
  //     map((file: ArrayBuffer) => {
  //         return file;
  //     })
  // );
  // }

}
