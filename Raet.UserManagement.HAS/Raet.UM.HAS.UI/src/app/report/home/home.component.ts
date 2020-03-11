import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ZoomTableDataSource, ZoomDataSource, IZoomActionRow, IZoomTableHeaderCell, ZoomTableDisplayedColumns } from '@zoomui/table';
import { ReportingService } from '../services/report/reporting.service';
import { DatePipe } from '@angular/common';
import { saveAs } from 'file-saver';
import { SessionService } from 'src/app/services/session.service';


@Component({
  selector: 'has-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  providers: [DatePipe]
})
export class HomeComponent implements OnInit {

  constructor(private router: Router, private datePipe: DatePipe, private reportingService: ReportingService,
    private sessionService: SessionService) { }

  reportsDataSource: ZoomTableDataSource<ZoomDataSource, IZoomActionRow<Function>>;
  reports: IReport[];
  actionRows: IZoomActionRow<Function> = {};
  displayedColumns: ZoomTableDisplayedColumns<IZoomTableHeaderCell[]>;

  isInitialised = false;

  columns: IZoomTableHeaderCell[] = [
    {
      title: 'File Name',
      field: 'fileName'
    },
    {
      title: 'Created Date',
      field: 'createdAt'
    },
    {
      title: 'Checksum',
      field: 'hash'
    },
    {
      title: 'Status',
      field: 'status'
    },
    {
      title: 'Action',
      field: 'action'
    }];

  ngOnInit() {
    this.displayedColumns = new ZoomTableDisplayedColumns<IZoomTableHeaderCell[]>(
      this.columns
    );

    this.loadDataToTable();
  }
  handleClickRow(row: any) {
    this.downloadReport(row.id, row.action, row.fileName);
  }

  private loadDataToTable(): void {
    this.reportingService.getReports(this.sessionService.Session.user.tenantId).subscribe(x => {
      this.reports = [];
      x.forEach(fileInfo => {
        fileInfo.id = fileInfo.guid;
        fileInfo.createdAt = this.datePipe.transform(fileInfo.createdAt, 'MMM d, y, HH:mm');
        fileInfo.status = (fileInfo.status === 'Completed') ? 'Available' : fileInfo.status;
        switch (fileInfo.status) {
          case 'Available':
            fileInfo.action = 'Download';
            break;
          case 'Failed':
            fileInfo.action = 'Retry';
            break;
          case 'Processing':
            fileInfo.action = 'Refresh';
            break;
        }
        this.reports.push(fileInfo);
        this.actionRows[fileInfo.id] = () => { };
      });
      this.isInitialised = true;
      this.reportsDataSource = new ZoomTableDataSource(
        this.reports,
        this.actionRows);
    });
  }

  downloadReport(id: string | number, action: string, fileName: string): any {
    switch (action) {
      case 'Download':
        this.reportingService.getDownloadUrl(this.sessionService.Session.user.tenantId,id.toString()).subscribe(
          success => {
            saveAs(success, fileName + '.csv');
        },
        err => {
            alert('Server error while downloading file' + err);
        }
        );
        break;
      case 'Refresh':
        this.loadDataToTable();
        break;
      case 'Retry':
        this.router.navigate(['/create-report']);
        break;
    }
  }

  onClickCreateReport(): any {
    this.router.navigate(['/create-report']);
  }
}
