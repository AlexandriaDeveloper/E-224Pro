import { Component, inject, OnInit, signal } from '@angular/core';
import { PaginatorModel } from '../../../shared/_models/paginator.model';
import { GetFormRequest } from '../../../shared/_requests/getFormRequest';
import { ActivatedRoute } from '@angular/router';
import { FormService } from '../../../shared/services/form.service';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { DeleteDialogComponent } from '../../../shared/components/dialog/delete-dialog/delete-dialog.component';
import { AddFormComponent } from '../../form/add-form/add-form.component';
import { FormResponse } from '../../form/form-response.interface';
import { FormSearchDialogComponent } from '../../form/form-search-dialog/form-search-dialog.component';
import { SubsidiaryService } from '../../../shared/services/subsidiary.service';
import { Collage } from '../../../shared/_models/collage.model';
import { CollageService } from '../../../shared/services/collage.service';
import { GetSubsidiaryFormsByDailyIdRequest } from '../../../shared/_requests/getSubsidiaryFormsByDailyIdRequest';
import { Fund } from '../../../shared/_models/fund.model';
import { FundService } from '../../../shared/services/fund.service';

@Component({
  selector: 'app-subsidary-daily',
  standalone: false,
  templateUrl: './subsidary-daily.component.html',
  styleUrl: './subsidary-daily.component.scss'
})
export class SubsidaryDailyComponent implements OnInit {
  router = inject(ActivatedRoute);
  subsidaryService = inject(SubsidiaryService);
  collageService = inject(CollageService);
  fundService = inject(FundService);
  readonly dialog = inject(MatDialog);
  params = new GetSubsidiaryFormsByDailyIdRequest();
  readonly panelOpenState = signal(false);

  displayedColumns: string[] = ['action', 'id', 'num224', 'num55', 'formName', 'collageName', 'fundName', 'totalDebit', 'totalCredit', 'SubsidaryTotalDebit', 'SubsidaryTotalCredit', 'isBalanced', 'auditorName'];
  dataSource: any[] = [];
  originalDataSource: any[] = [];
  lastSearchedDataSource: any[] = []; // Track last searched data
  dailyId;
  subsidaryId;
  data: any;
  collages: Collage[] = []
  funds: Fund[] = [];
  filterdFunds: Fund[] = [];

  paginator: PaginatorModel = new PaginatorModel();
  constructor() {
    this.router.params.subscribe(params => {
      this.dailyId = params['dailyId'];
      this.subsidaryId = params['subAccountId'];
    });

  }
  ngOnInit(): void {
    this.loadCollages();
    this.loadFunds();
    this.params.DailyId = this.dailyId;
    this.loadForms(this.params);
  }

  loadForms(param: GetSubsidiaryFormsByDailyIdRequest) {
    this.subsidaryService.GetSubsidaryDailyFormsByDailyId(this.subsidaryId, this.params).subscribe({
      next: (response: any) => {

        this.dataSource = response.items;
        this.originalDataSource = [...response.items];
        this.data = response.items;
        this.paginator.length = response.totalCount;
      },
      error: (error) => {
        // Handle error
        console.error('Error loading forms', error);
      }
    });
  }
  loadCollages() {
    this.collageService.getCollages().subscribe({
      next: (response: Collage[]) => {

        this.collages = response;
      },
      error: (error) => {
        // Handle error
        console.error('Error loading collages', error);
      }
    })
  }
  loadFunds() {
    this.fundService.getFundsByCollageId({}).subscribe({
      next: (response: Fund[]) => {
        this.funds = response;
        this.filterdFunds = response;
      },
      error: (error) => {
        // Handle error
        console.error('Error loading funds', error);
      }
    })
  }
  getCollageById(collageId) {
    return this.collages.find(x => x.id == collageId).collageName;
  }
  getFundById(fundId) {

    //check befor return
    return this.funds.find(x => x.id == fundId)?.fundName;
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
  onCollageIdChange(collageId) {
    console.log(collageId);
    this.filterdFunds = [];
    this.params.CollageId = collageId;

    if (collageId !== 0) {
      this.filterdFunds = this.funds.filter(x => x.collageId == collageId);
      console.log(this.funds);
      console.log(this.filterdFunds);
    }
    else {
      this.filterdFunds = []
    }
    console.log(this.filterdFunds);
    this.loadForms(this.params);
  }
  onFundIdChange(fundId) {
    this.params.FundId = fundId;
    //this.filterdFunds = this.funds.filter(x => x.collageId == this.params.CollageId);
    this.loadForms(this.params);
  }

  downloadTemplate() {
    // this.formService.downloadDailyPdfFormTemplate(this.id).subscribe({
    //   next: (response: any) => {

    //     //dowload pdf file 

    //     const blob = new Blob([response], { type: 'application/pdf' });
    //     const url = window.URL.createObjectURL(blob);
    //     const a = document.createElement('a');
    //     a.href = url;
    //     a.download = this.data.daily.name + '.pdf';
    //     document.body.appendChild(a);
    //     a.click();
    //     window.URL.revokeObjectURL(url);
    //     a.remove();
    //   },
    //   error: (error) => {
    //     console.error('Error downloading template:', error);
    //   }
    // });
  }
}

