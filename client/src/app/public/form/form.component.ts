import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormService } from '../../shared/services/form.service';
import { GetFormRequest, SearchFormRequest } from '../../shared/_requests/getFormRequest';
import { PageEvent } from '@angular/material/paginator';
import { PaginatorModel } from '../../shared/_models/paginator.model';
import { AddFormComponent } from './add-form/add-form.component';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../../shared/components/dialog/delete-dialog/delete-dialog.component';
import { FormSearchDialogComponent } from './form-search-dialog/form-search-dialog.component';
import { FormResponse } from './form-response.interface';
import { DownloadExcelTemplateDialogComponent } from './download-excel-template-dialog/download-excel-template-dialog.component';
import { UploadExcelFormDialogComponent } from './upload-excel-form-dialog/upload-excel-form-dialog.component';
import { DailiesSearchDialogComponent } from '../dailies/dailies-search-dialog/dailies-search-dialog.component';
import { SearchFileDialogComponent } from '../dailies/search-forms/Search-File-Dialog/Search-File-Dialog.component';
import { ToasterService } from '../../shared/services/toaster.service';

@Component({
  selector: 'app-form',
  standalone: false,
  templateUrl: './form.component.html',
  styleUrl: './form.component.scss'
})
export class FormComponent implements OnInit {
  router = inject(ActivatedRoute);
  formService = inject(FormService);
  toast = inject(ToasterService);
  readonly dialog = inject(MatDialog);
  params = new SearchFormRequest();
  readonly panelOpenState = signal(false);

  displayedColumns: string[] = ['action', 'num224', 'num55', 'formName', 'collageName', 'fundName', 'entryType', 'totalDebit', 'totalCredit', 'isBalanced'];
  dataSource: any[] = [];
  originalDataSource: any[] = [];
  lastSearchedDataSource: any[] = []; // Track last searched data
  id;
  data: any;

  paginator: PaginatorModel = new PaginatorModel();
  constructor() {
    this.router.params.subscribe(params => this.id = params['id']);

  }
  ngOnInit(): void {
    this.params.dailyId = this.id;
    this.loadForms();
  }

  loadForms() {
    this.formService.getForms(this.params).subscribe({
      next: (response: FormResponse) => {
        this.dataSource = response.formDtos;
        this.originalDataSource = [...response.formDtos];
        this.data = response;
        this.paginator.length = response.totalCount;
      },
      error: (error) => {
        // Handle error
        console.error('Error loading forms', error);
      }
    });
  }

  deleteForm(form: any) {
    console.log('Attempting to delete form:', form);
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {
        message: `انت على وشك حذف يوميه ${form.formName} هل انت متأكد ؟!`
      },
      disableClose: true,
      hasBackdrop: true
    });

    dialogRef.afterClosed().subscribe(result => {
      // Only proceed if user confirmed deletion
      if (result === true) {
        this.formService.deleteForm(form.id).subscribe({
          next: (response) => {
            console.log('Form deleted successfully:', response);
            // Reload forms with current params
            this.loadForms();
          },
          error: (error) => {
            console.error('Error deleting form:', error);
            // Optional: Show error notification to user
            // this.snackBar.open('Failed to delete form', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }

  openSearchFileDialog(): void {
    // Logic to open the search file dialog can be implemented here
    const dialogRef = this.dialog.open(SearchFileDialogComponent, {
      data: {        // Pass any data needed for the dialog here
        searchRequest: this.params,
        paginator: this.paginator
      },
      disableClose: true,
      hasBackdrop: true,
      minWidth: '40vw'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {

        this.params = result;



        this.loadForms();
      }
      console.log('The dialog was closed');
    });
  }

  handlePageEvent(e: PageEvent) {
    this.paginator.pageEvent = e;
    this.paginator.length = e.length;
    this.params.pageSize = e.pageSize;
    this.params.pageIndex = e.pageIndex;
    this.loadForms();
  }
  openAddFormDialog(element: any = null) {


    const dialogRef = this.dialog.open(AddFormComponent, {
      data: {
        param: this.params,
        element: element
      },
      disableClose: true,
      hasBackdrop: true,
      minWidth: '60vw',
      maxHeight: '90vh'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.toast.info('تم تحديث الملف  بنجاح');

      }
      else if (result == false) {
        this.toast.success('تم اضافة الملف بنجاح');

      }
      else {
        return;
      }
      this.loadForms();


    });
  }

  downloadTemplate() {
    this.formService.downloadDailyPdfFormTemplate(this.id).subscribe({
      next: (response: any) => {

        //dowload pdf file 

        const blob = new Blob([response], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      },
      error: (error) => {
        console.error('Error downloading template:', error);
      }
    });
  }

  openExcelTemplateDialog() {
    const dialogRef = this.dialog.open(DownloadExcelTemplateDialogComponent, {
      data: {
        message: 'هل تريد تحميل نموذج اكسل لادخال بيانات اليوميه؟'
      },
      disableClose: true,
      minHeight: '40vh',
      minWidth: '60vw',
      hasBackdrop: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.downloadExcelTemplate(result);
      }
    });
  }


  downloadExcelTemplate(result) {
    this.formService.downloadDailyExcelFormTemplate(result).subscribe({
      next: (response: any) => {

        //dowload excel file 

        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      },
      error: (error) => {
        console.error('Error downloading template:', error);
      }
    });
  }
  downloadDailyExcelForms(dailyId: number) {
    console.log(dailyId);

    this.formService.downloadDailyExcelForms(dailyId).subscribe({
      next: (response: any) => {

        //dowload excel file 

        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      },
      error: (error) => {
        console.error('Error downloading daily excel forms:', error);
      }
    });
  }
  openUploadExcelDialog() {
    const dialogRef = this.dialog.open(UploadExcelFormDialogComponent, {
      data: {
        dailyId: this.id // Pass the dailyId
      },
      disableClose: true,
      minHeight: '40vh',
      minWidth: '60vw',
      hasBackdrop: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadForms();
      }
    });
  }
  onNoClick(): void {
    this.dialog.closeAll();
  }

}
