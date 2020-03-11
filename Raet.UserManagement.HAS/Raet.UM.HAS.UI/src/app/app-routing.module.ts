import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
//import { HomeComponent } from './report/home/home.component';
import { CreateReportComponent } from './report/create-report/create-report.component';
import { HomeComponent } from './report/home/home.component';
import { AuthGuard } from './guards/auth.guard';



const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard]},
  { path: 'create-report', component: CreateReportComponent, canActivate: [AuthGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
