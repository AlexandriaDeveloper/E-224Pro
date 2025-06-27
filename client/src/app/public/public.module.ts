import { NgModule } from '@angular/core';

import { PublicRoutingModule } from './public-routing.module';
import { PublicHomeComponent } from './public-home.component';

import { FooterComponent } from '../shared/components/footer/footer.component';

import { MatSidenavModule } from '@angular/material/sidenav';

import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { DailiesComponent } from './dailies/dailies.component';
import { DailiesSearchDialogComponent } from './dailies/dailies-search-dialog/dailies-search-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HeaderComponent } from '../shared/components/header/header.component';
import { SidebarComponent } from '../shared/components/sidebar/sidebar.component';
import { AddDailyComponent } from './dailies/add-daily/add-daily.component';
import { DeleteDialogComponent } from '../shared/components/dialog/delete-dialog/delete-dialog.component';
import { FormComponent } from './form/form.component';
import { AddFormComponent } from './form/add-form/add-form.component';
import { AccountInfoDialogComponent } from './account/account-info-dialog/account-info-dialog.component';
import { DailiesReportDialogComponent } from './dailies/dailies-report-dialog/dailies-report-dialog.component';
import { FundHomeComponent } from './fund-home/fund-home.component';
import { AddFundDialogComponent } from './fund-home/add-fund-dialog/add-fund-dialog.component';
import { SubsidaryDailiesComponent } from './subsidary-dailies/subsidary-dailies.component';
import { SubsidaryDailyComponent } from './subsidary-dailies/subsidary-daily/subsidary-daily.component';
import { AddSubsidaryFormDetailsDialogComponent } from './subsidary-dailies/subsidary-daily/add-subsidary-form-details-dialog/add-subsidary-form-details-dialog.component';
import { DownloadExcelTemplateDialogComponent } from './form/download-excel-template-dialog/download-excel-template-dialog.component';
import { UploadExcelFormDialogComponent } from './form/upload-excel-form-dialog/upload-excel-form-dialog.component';



@NgModule({

  declarations: [

    PublicHomeComponent,
    DailiesComponent,
    DailiesSearchDialogComponent,
    DailiesReportDialogComponent,
    AddDailyComponent,

    FormComponent,
    AddFormComponent,
    DownloadExcelTemplateDialogComponent,
    AccountInfoDialogComponent,
    FundHomeComponent,
    AddFundDialogComponent,
    SubsidaryDailiesComponent,
    SubsidaryDailyComponent,
    AddSubsidaryFormDetailsDialogComponent, UploadExcelFormDialogComponent

  ],
  imports: [
    CommonModule,
    PublicRoutingModule,
    MatSidenavModule,
    FormsModule,
    ReactiveFormsModule,
    HeaderComponent,
    SidebarComponent,
    FooterComponent,
    SharedModule,
    DeleteDialogComponent,



  ]
})
export class PublicModule { }
