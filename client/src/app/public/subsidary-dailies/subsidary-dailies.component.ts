import { Component, inject, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { GetDailiesRequest } from '../../shared/_requests/getDailiesRequest';
import { Collage } from '../../shared/_models/collage.model';
import { Daily } from '../../shared/_models/Daily.model';
import { CollageService } from '../../shared/services/collage.service';
import { DailiesService } from '../../shared/services/dailies.service';
import { ActivatedRoute } from '@angular/router';
import { SubsidiaryService } from '../../shared/services/subsidiary.service';
import { Fund } from '../../shared/_models/fund.model';
import { FundService } from '../../shared/services/fund.service';
import { GetSubsidiaryFormsByDailyIdRequest } from '../../shared/_requests/getSubsidiaryFormsByDailyIdRequest';
import { SubsidaryFilterDialogComponent } from './subsidary-filter-dialog/subsidary-filter-dialog.component';

@Component({
  selector: 'app-subsidary-dailies',
  standalone: false,
  templateUrl: './subsidary-dailies.component.html',
  styleUrl: './subsidary-dailies.component.scss'
})
export class SubsidaryDailiesComponent implements OnInit {
  displayedColumns: string[] = ['action', 'id', 'name', 'dailyDate', 'dailyType', 'totalCredit', 'totalDebit', 'SubsidaryTotalCredit', 'SubsidaryTotalDebit', 'isBalanced'];
  dataSource;
  params: GetSubsidiaryFormsByDailyIdRequest = new GetSubsidiaryFormsByDailyIdRequest();
  route = inject(ActivatedRoute);
  subsidaryService = inject(SubsidiaryService);
  collageService = inject(CollageService);
  fundService = inject(FundService);
  daily: Daily = null;
  range: { start: Date | null, end: Date | null } = { start: null, end: null };
  hasActiveFilters = false;
  length = 50;
  pageSize = 30;
  pageIndex = 0;
  pageSizeOptions = [5, 15, 30];
  pageEvent: PageEvent;

  collages: Collage[] = [];
  funds: Fund[] = []



  constructor(private dialog: MatDialog) {

  }
  ngOnInit(): void {
    this.loadCollages();
    this.route.paramMap.subscribe((p: any) => { this.params.accountId = p.get('subaccountId'); this.params.dailyId = p.get('dailyId'); this.loadDailies(this.params); });
    //  this.params.accountId = this.subsidaryId;

    //  this.loadDailies(this.params);
  }

  loadDailies(param: GetDailiesRequest) {
    this.subsidaryService.getSubsidaryDailies(this.params.accountId, param).subscribe((dailies: any) => {
      console.log(dailies);

      this.dataSource = dailies.items;
      this.length = dailies.totalCount;
    });
  }
  loadCollages() {
    this.collageService.getCollages().subscribe((collages: Collage[]) => {
      console.log(collages);
      this.collages = collages;

    });
  }
  loadFunds(collageId) {
    this.fundService.getFundsByCollageId(collageId).subscribe((funds: Fund[]) => {
      console.log(funds);
      this.funds = funds;
    });

  }
  handlePageEvent(e: PageEvent) {

    console.log('Page Event:', e);
    this.pageEvent = e;
    this.length = e.length;
    this.params.pageSize = e.pageSize;
    this.params.pageIndex = e.pageIndex;
    this.loadDailies(this.params);
  }

  onDailyTypeChange(dailyType) {
    this.params.dailyType = dailyType;

    //this.loadDailies(this.params);
  }
  onCollageChange(collageId) {
    this.params.collageId = collageId;
    this.loadFunds(collageId);
  }
  onFundsChange(fundId) {

    this.params.fundId = fundId;

  }
  onEntryTypeChange(entryType) {
    console.log('Selected Entry Type:', entryType);

    this.params.entryType = entryType;
    //this.loadDailies(this.params);
  }
  submit() {

    // if (this.range.start && this.range.end) {
    //   console.log('Start Date:', this.range.start);
    //   console.log('End Date:', this.range.end);

    //   // Example: Get number of days
    //   const timeDiff = this.range.end.getTime() - this.range.start.getTime();
    //   const days = timeDiff / (1000 * 3600 * 24) + 1;
    //   console.log('Selected range is', days, 'days');
    // } else {
    //   console.warn('Please select both start and end dates');
    // }

    this.loadDailies(this.params);
  }



  onPrint() {

    if (this.range.start && this.range.end) {
      this.params.startDate = this.range.start;
      this.params.endDate = this.range.end;
    }
    console.log(this.params);

    this.subsidaryService.downloadSubsidaryDailyPdf(this.params).subscribe((response: any) => {
      const blob = new Blob([response], { type: 'application/pdf' });
      const url = window.URL.createObjectURL(blob);
      window.open(url);
      URL.revokeObjectURL(url);
      this.loadDailies(this.params);
    });

  }
  
  openFilterDialog() {
    // Prepare current filter values to pass to the dialog
    const currentFilters = {
      dailyType: this.params.dailyType || '',
      entryType: this.params.entryType || '',
      collageId: this.params.collageId || '',
      fundId: this.params.fundId || '',
      startDate: this.range.start,
      endDate: this.range.end
    };
    
    const dialogRef = this.dialog.open(SubsidaryFilterDialogComponent, {
      width: '600px',
      disableClose: false,
      data: { filters: currentFilters }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.applied) {
        const filters = result.filters;
        
        // Update params with the filter values
        this.params.dailyType = filters.dailyType || null;
        this.params.entryType = filters.entryType || null;
        this.params.collageId = filters.collageId || null;
        this.params.fundId = filters.fundId || null;
        
        // Update date range
        this.range.start = filters.startDate || null;
        this.range.end = filters.endDate || null;
        
        if (this.range.start && this.range.end) {
          this.params.startDate = this.range.start;
          this.params.endDate = this.range.end;
        }
        
        // Check if any filters are active
        this.hasActiveFilters = !!(filters.dailyType || filters.entryType || 
                               filters.collageId || filters.fundId || 
                               filters.startDate || filters.endDate);
        
        // Load data with the new filters
        this.loadDailies(this.params);
      }
    });
  }
  
  openSearchDialog() {
    // Existing search dialog functionality
  }
  
  openDailiesReportDialog() {
    // Existing reports dialog functionality
  }


}
