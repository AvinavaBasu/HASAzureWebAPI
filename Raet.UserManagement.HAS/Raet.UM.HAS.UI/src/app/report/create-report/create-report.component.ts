import {
  Component,
  OnInit,
  ViewEncapsulation,
  ChangeDetectorRef,
  ViewChildren,
  QueryList
} from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { ReportingService } from "../services/report/reporting.service";
import { GlobalService } from "src/app/services/global.service";
import { map } from "rxjs/operators";
import { ICreateReportRequest } from "../models/create-report-request";
import { ZoomButtonComponent } from "@zoomui/button";
import { ZoomDatePicker, ZoomDataLanguage } from "@zoomui/date-picker";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { format, isValid } from "date-fns";
import { SessionService } from "src/app/services/session.service";
import { ZoomFormSelectSingleComponent } from "@zoomui/form";
import { ZoomSelectOption } from "./create-report.mapping";
import { ICustomUser } from "../models/customUsers";
import { IExternalId, ExternalId } from "../models/externalId";

@Component({
  selector: "has-create-report",
  templateUrl: "./create-report.component.html",
  styleUrls: ["./create-report.component.scss"],
  encapsulation: ViewEncapsulation.None
})
export class CreateReportComponent implements OnInit {
  dataLanguageDemo = new ZoomDataLanguage();
  dateInputValue = new ZoomDatePicker();
  minValue = new ZoomDatePicker();
  maxValue = new ZoomDatePicker();
  @ViewChildren(ZoomFormSelectSingleComponent)
  selectSingleComponents: QueryList<ZoomFormSelectSingleComponent>;
  constructor(
    private reportingService: ReportingService,
    private globalService: GlobalService,
    protected changeDetectorRef: ChangeDetectorRef,
    private router: Router,
    private sessionService: SessionService
  ) {
    this.initialiseForm();
  }

  createForm: FormGroup;
  dateForm: FormGroup;
  allAuditedApplications: any[];
  allPermissions: IPermission[];
  isValidForm: boolean;
  disablePermission: boolean;
  click: Function;
  clickEnd: Function;
  disableEndDate: boolean;
  users: ZoomSelectOption[] = [];
  permissionNamelist: string[];
  selectedApp: string;
  permissionSubscription: Observable<IPermission[]>;
  userType: string;
  isTargetOrSourceSelected: boolean;
  customerUsers: ICustomUser[] = [];
  selectedUser: ICustomUser;
  isPermissionSelected: boolean = false;

  private initialiseForm(): void {
    this.isValidForm = false;
    this.loadauditingApplications(this.sessionService.Session.user.tenantId);
    this.disablePermission = true;
    this.disableEndDate = false;
    this.click = (value: string) => this.onClickEvent(value);
    this.clickEnd = (value: string) => this.onClickEndEvent(value);
    this.router
      .navigateByUrl("/create-report", { skipLocationChange: true })
      .then(() => {
        this.router.navigate(["/create-report"]);
      });

    this.createForm = new FormGroup({
      reportName: new FormControl("", [
        Validators.required,
        this.noWhitespaceValidator
      ]),
      auditedApplication: new FormControl("", [Validators.required]),
      multipleSelectPermissions: new FormControl("", [Validators.required]),
      reportType: new FormControl("", [Validators.required]),
      userGroupSearch: new FormControl("", [Validators.nullValidator])
    });
    this.dateForm = new FormGroup({
      startInputDate: new FormControl(
        this.minValue.selectedDate,
        [Validators.required],
        [this.invalidDate]
      ),
      endInputDate: new FormControl(
        this.maxValue.selectedDate,
        [Validators.required],
        [this.invalidDate]
      )
    });
    this.createForm.controls.reportType.setValue(true);
    this.globalService.setbackButton("/");
    this.dateForm
      .get("startInputDate")
      .valueChanges.subscribe((value: string) => {
        const newValue = new ZoomDatePicker({
          selectedDate: new Date(value),
          maxDate: new Date()
        });
        this.minValue = newValue;
        const newMinValue = new ZoomDatePicker({
          minDate: this.minValue.selectedDate,
          maxDate: new Date()
        });
        this.maxValue = newMinValue;
      });
    this.dateForm
      .get("endInputDate")
      .valueChanges.subscribe((value: string) => {
        const newValue = new ZoomDatePicker({
          selectedDate: new Date(value),
          maxDate: new Date(),
          minDate: this.minValue.selectedDate
        });
        this.maxValue = newValue;
      });
  }

  onClickEvent(value: string) {
    this.dateForm.get("startInputDate").setValue(value);
    const endDateValue = this.dateForm.get("endInputDate").value;
    const endDate = new ZoomDatePicker({
      selectedDate: new Date(endDateValue)
    });
    if (this.minValue.selectedDate > endDate.selectedDate) {
      this.dateForm.get("endInputDate").setValue(format(new Date(), "P"));
    }
    this.disableEndDate = true;
  }

  onClickEndEvent(value: string) {
    this.dateForm.get("endInputDate").setValue(value);
  }

  public noWhitespaceValidator(control: FormControl) {
    const isWhitespace = (control.value || "").trim().length === 0;
    const isValid = !isWhitespace;
    return isValid ? null : { whitespace: true };
  }

  private loadauditingApplications(tenant: string): any {
    this.reportingService.getApplications(tenant).subscribe(applications => {
      this.allAuditedApplications = [];
      applications.forEach(app => {
        this.allAuditedApplications.push({ id: app, name: app });
      });
      this.changeDetectorRef.markForCheck();
      this.disablePermission = true;
    });
  }

  private loadallPermissions(app: string, tenant: string): any {
    this.reportingService
      .getPermissions(app, tenant)
      .pipe(map(x => x))
      .subscribe(x => {
        x.forEach(permission => {
          permission.name = permission.id;
        });
        this.disablePermission = false;
        this.allPermissions = x;
      });
  }

  private async loadtUsers(
    inputType: string,
    permissions: string[],
    app: string,
    tenantId: string
  ): Promise<any> {
    const response = await this.reportingService.getUsers(
      inputType,
      permissions,
      app,
      tenantId
    );
    response.subscribe(users => {
      users.forEach((user: { key: IExternalId; userName: any }) => {
        this.users.push({ id: user.key.id, name: user.userName });
        this.customerUsers.push({ key: user.key, userName: user.userName });
        this.isTargetOrSourceSelected = true;
      });
      this.changeDetectorRef.markForCheck();
    });
  }

  searchUsers() {
    let temp = this.selectSingleComponents.last.searchValueStartAt;
    let data = this.users.filter(val => val.name.toLowerCase().includes(temp));
    this.selectSingleComponents.last.updateData(data);
  }

  invalidDate(control: FormControl): Promise<any> | Observable<any> {
    return new Promise(resolve => {
      const date = isValid(new Date(control.value));
      if (!date) {
        resolve({ customError: true });
      } else {
        resolve(null);
      }
    });
  }

  hasCustomValidationError(element: {
    errors: { customError: boolean };
  }): boolean {
    if (element && element.errors) {
      return element.errors.customError;
    }
    return false;
  }

  onChanges(): void {
    this.createForm.get("reportName").valueChanges.subscribe(() => {});
    this.createForm.get("auditedApplication").valueChanges.subscribe(val => {
      this.selectedApp = val;
      this.loadallPermissions(val, this.sessionService.Session.user.tenantId);
    });
    this.createForm
      .get("multipleSelectPermissions")
      .valueChanges.subscribe(val => {
        this.permissionNamelist = val;
        this.isPermissionSelected = true;
        this.isTargetOrSourceSelected = false;
        this.createForm.controls["reportType"].reset("Global");
        //this.createForm.controls["reportType"].setValue("Global");
      });
    this.createForm.get("reportType").valueChanges.subscribe(async val => {
      if (val != "Global") {
        //this.createForm.controls['userGroupSearch'].setValue('Select User');
        this.userType = val + " user";
        this.users = [];
        await this.loadtUsers(
          val,
          this.permissionNamelist,
          this.selectedApp,
          this.sessionService.Session.user.tenantId
        );
      } else {
        this.isTargetOrSourceSelected = false;
      }
    });
    this.createForm.get("userGroupSearch").valueChanges.subscribe(val => {
      this.selectedUser = this.customerUsers.filter(x => x.key.id == val)[0];
    });
  }

  ngOnInit() {
    this.minValue = new ZoomDatePicker({
      maxDate: new Date()
    });
    this.maxValue = new ZoomDatePicker({
      maxDate: new Date()
    });
    this.dateForm.get("endInputDate").setValue(format(new Date(), "P"));
    this.onChanges();
  }

  onClickCreateReport(createButton: ZoomButtonComponent): void {
    if (createButton && this.createForm.valid && this.dateForm.valid) {
      const request: ICreateReportRequest = {
        application: this.createForm.value.auditedApplication,
        permissions: this.createForm.value.multipleSelectPermissions,
        sourceUser:
          this.createForm.value.reportType == "Source"
            ? this.selectedUser.key
            : new ExternalId(),
        targetUser:
          this.createForm.value.reportType == "Target"
            ? this.selectedUser.key
            : new ExternalId(),
        fileName: this.createForm.value.reportName.trim(),
        startDate: this.dateForm.value.startInputDate,
        endDate: this.dateForm.value.endInputDate,
        tenantId: this.sessionService.Session.user.tenantId
      } as ICreateReportRequest;

      createButton.showLoading();

      this.reportingService.createReport(request).subscribe(() => {
        createButton.done();
        this.router.navigate(["/"]);
      });
    }
  }
}
