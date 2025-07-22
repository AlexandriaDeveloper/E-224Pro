import { Component, inject, OnInit } from '@angular/core';
import { FormService } from '../../../shared/services/form.service';
import { SearchFormRequest } from '../../../shared/_requests/getFormRequest';
import { SearchFormModel } from '../../../shared/_models/forms/FormDto.model';
import { PaginatorModel } from '../../../shared/_models/paginator.model';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { SearchFileDialogComponent } from './Search-File-Dialog/Search-File-Dialog.component';
import { AddFormComponent } from '../../form/add-form/add-form.component';

@Component({
  selector: 'app-search-forms',
  standalone: false,
  templateUrl: './search-forms.component.html',
  styleUrl: './search-forms.component.scss'
})
export class SearchFormsComponent implements OnInit {
  // Define any properties or methods needed for the component
  formService: FormService = inject(FormService); // Example service, replace with actual service type

  searchRequest: SearchFormRequest = new SearchFormRequest(); // Example property to hold search request data
  searchForms: SearchFormModel[] = []; // Example property to hold search forms data

  displayedColumns: string[] = ['id', 'num224', 'num55', 'formName', 'collageName', 'fundName', 'dailyType', 'dailyName', 'dailyDate', 'entryType']; // Columns to display in the table
  dataSource; // Data source for the table
  paginator: PaginatorModel = new PaginatorModel();
  pageEvent: PageEvent;
  readonly dialog = inject(MatDialog);

  constructor() {
    // Initialize any properties if necessary
  }

  ngOnInit(): void {
    // Perform any initialization logic here

    this.loadSearchForms();
  }
  // Add any additional methods or lifecycle hooks as needed
  loadSearchForms(): void {
    console.log('Loading search forms with request:', this.searchRequest);

    // Logic to load search forms can be implemented here
    this.formService.searchForm(this.searchRequest).subscribe(
      (forms: any) => {
        this.dataSource = forms.items; // Assuming the service returns an array of forms
        this.paginator.length = forms.totalCount;

      },
      (error) => {
        console.error('Error loading search forms:', error);
      }
    );
  }
  handlePageEvent(e: PageEvent) {

    console.log('Page Event:', e);
    this.pageEvent = e;
    this.paginator.length = e.length;
    this.searchRequest.pageSize = e.pageSize;
    this.searchRequest.pageIndex = e.pageIndex;
    this.loadSearchForms();
  }


  openSearchFileDialog(): void {
    // Logic to open the search file dialog can be implemented here
    const dialogRef = this.dialog.open(SearchFileDialogComponent, {
      data: {        // Pass any data needed for the dialog here
        searchRequest: this.searchRequest,
        paginator: this.paginator
      },
      disableClose: true,
      hasBackdrop: true,
      minWidth: '40vw'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log(result);

        this.searchRequest = result;



        this.loadSearchForms();
      }
      console.log('The dialog was closed');
    });
  }
  openAddFormDialog(element: any = null) {

    console.log(element);

    const dialogRef = this.dialog.open(AddFormComponent, {
      data: {
        param: this.searchForms,
        element: element
      },
      disableClose: true,
      hasBackdrop: true,
      minWidth: '60vw',
      maxHeight: '90vh'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadSearchForms();

    });
  }

  // Add any additional methods or lifecycle hooks as needed  
}


