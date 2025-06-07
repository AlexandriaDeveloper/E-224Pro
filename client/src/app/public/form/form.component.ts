import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormService } from '../../shared/services/form.service';
import { GetFormRequest } from '../../shared/_requests/getFormRequest';
import { PageEvent } from '@angular/material/paginator';
import { PaginatorModel } from '../../shared/_models/paginator.model';
import { AddFormComponent } from './add-form/add-form.component';
import { MatDialog } from '@angular/material/dialog';
import { DeleteDialogComponent } from '../../shared/components/dialog/delete-dialog/delete-dialog.component';
import { FormSearchDialogComponent } from './form-search-dialog/form-search-dialog.component';
import { FormResponse } from './form-response.interface';

@Component({
  selector: 'app-form',
  standalone: false,
  templateUrl: './form.component.html',
  styleUrl: './form.component.scss'
})
export class FormComponent implements OnInit {
  router = inject(ActivatedRoute);
  formService = inject(FormService);
  readonly dialog = inject(MatDialog);
  params = new GetFormRequest();
  readonly panelOpenState = signal(false);

  displayedColumns: string[] = ['action', 'id', 'num224', 'num55', 'formName', 'collageName', 'fundName', 'totalDebit', 'totalCredit', 'isBalanced', 'auditorName'];
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
    this.params.DailyId = this.id;
    this.loadForms(this.params);
  }

  loadForms(param: GetFormRequest) {
    this.formService.getForms(param).subscribe({
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
            this.loadForms(this.params);
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

  openSearchDialog() {
    // Determine which data to pass to search dialog
    const dataToSearch = this.dataSource.length !== this.originalDataSource.length
      ? this.dataSource
      : (this.lastSearchedDataSource.length > 0 ? this.lastSearchedDataSource : this.originalDataSource);

    const dialogRef = this.dialog.open(FormSearchDialogComponent, {
      width: '500px',
      data: {
        formData: this.originalDataSource, // Always pass original data
        currentData: dataToSearch, // Pass last searched or current filtered data
        // If we have previous search values, pass them to pre-fill the form
        previousSearchValues: this.lastSearchedDataSource.length > 0 ? this.lastSearchedDataSource[0] : null
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // If result is the original data source, reset filtering
        if (result === this.originalDataSource) {
          this.dataSource = [...this.originalDataSource];
          this.lastSearchedDataSource = []; // Clear last searched data
        } else {
          // Destructure the result to get search results and search values
          const { results, searchValues } = result;

          // Store the search result as last searched data with search values
          this.lastSearchedDataSource = results.map(item => ({ ...item, searchValues }));
          this.dataSource = results;
        }
      }
    });
  }

  handlePageEvent(e: PageEvent) {
    this.paginator.pageEvent = e;
    this.paginator.length = e.length;
    this.params.pageSize = e.pageSize;
    this.params.pageIndex = e.pageIndex;
    this.loadForms(this.params);
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
      this.loadForms(this.params);

    });
  }

  downloadTemplate() {
    this.formService.downloadDailyPdfFormTemplate(this.id).subscribe({
      next: (response: any) => {

        //dowload pdf file 

        const blob = new Blob([response], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = this.data.daily.name + '.pdf';
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        a.remove();
      },
      error: (error) => {
        console.error('Error downloading template:', error);
      }
    });
  }
}
