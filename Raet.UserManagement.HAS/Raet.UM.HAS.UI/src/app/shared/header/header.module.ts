import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SubheaderComponent } from './subheader/subheader.component';
import { FormgroupComponent } from './formgroup/formgroup.component';



@NgModule({
  declarations: [SubheaderComponent, FormgroupComponent],
  imports: [
    CommonModule
  ],
  exports: [SubheaderComponent, FormgroupComponent]
})
export class HeaderModule { }
