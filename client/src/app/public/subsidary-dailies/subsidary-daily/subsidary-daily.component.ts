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
import { AddSubsidaryFormDetailsDialogComponent } from './add-subsidary-form-details-dialog/add-subsidary-form-details-dialog.component';
import { SubaccountService } from '../../../shared/services/subaccount.service';

@Component({
  selector: 'app-subsidary-daily',
  standalone: false,
  templateUrl: './subsidary-daily.component.html',
  styleUrl: './subsidary-daily.component.scss'
})
export class SubsidaryDailyComponent implements OnInit {
  router = inject(ActivatedRoute);
  subsidaryService = inject(SubsidiaryService);
  subAccountService = inject(SubaccountService);
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

    this.params.dailyId = this.dailyId;
    this.loadForms(this.params);
  }

  loadForms(param: GetSubsidiaryFormsByDailyIdRequest) {
    this.subsidaryService.GetSubsidaryDailyFormsByDailyId(this.subsidaryId, this.dailyId, this.params).subscribe({
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





  handlePageEvent(e: PageEvent) {
    this.paginator.pageEvent = e;
    this.paginator.length = e.length;
    this.params.pageSize = e.pageSize;
    this.params.pageIndex = e.pageIndex;
    this.loadForms(this.params);
  }

  onCollageIdChange(collageId) {
    console.log(collageId);
    this.filterdFunds = [];
    this.params.collageId = collageId;

    if (collageId !== 0) {
      this.filterdFunds = this.funds.filter(x => x.collageId == collageId);

    }
    else {
      this.filterdFunds = []
    }

  }
  onFundIdChange(fundId) {
    this.params.fundId = fundId;
    //this.filterdFunds = this.funds.filter(x => x.collageId == this.params.CollageId);

  }

  openAddSubsidaryDailyDialog(element) {

    const dialogRef = this.dialog.open(AddSubsidaryFormDetailsDialogComponent, {
      data: {

        element: element,
        accountId: this.subsidaryId
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
  onSearch() {
    this.loadForms(this.params);
  }
  onPrint() {
    this.params.dailyId = this.dailyId;
    this.params.accountId = this.subsidaryId;
    this.subsidaryService.downloadSubsidaryDailyPdf(this.params).subscribe((response: any) => {
      const blob = new Blob([response], { type: 'application/pdf' });
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    });

  }


}

