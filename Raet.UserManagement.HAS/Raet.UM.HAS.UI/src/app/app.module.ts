import { BrowserModule } from '@angular/platform-browser';
import {  BrowserAnimationsModule} from '@angular/platform-browser/animations';

import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ZoomPageTemplateModule } from '@zoomui/page-template';
import { ZoomButtonModule } from '@zoomui/button';
import { ZoomFormModule } from '@zoomui/form';
import { ZoomDialogModule } from '@zoomui/dialog';
import {ZoomDatePickerModule} from '@zoomui/date-picker';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppConfigService } from './services/app-config.service';
// import { HomeComponent } from './report/home/home.component';
import { HeaderModule } from './shared/header/header.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CreateReportComponent } from './report/create-report/create-report.component';
import { HomeComponent } from './report/home/home.component';
import { GlobalService } from './services/global.service';
import { ZoomTableModule } from '@zoomui/table';
import { AuthInterceptor } from './interceptors/auth-interceptor';
import { CommonModule } from '@angular/common';
import { ConfigService } from './services/configservice.service';

export function loadConfigService(configService: AppConfigService): () => void {
  return () => {
    return configService.load();
  };
}

export function configFactory(configService: ConfigService) {
  return () => configService.loadConfig();
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    CreateReportComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ZoomPageTemplateModule,
    HeaderModule,
    HttpClientModule,
    AppRoutingModule,
    ZoomButtonModule,
    ZoomFormModule,
    ZoomDialogModule,
    ZoomTableModule,
    ZoomDatePickerModule
  ],
  providers: [
    AppConfigService,
    GlobalService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: APP_INITIALIZER,
      useFactory: configFactory,
      deps: [ConfigService],
      multi: true
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
