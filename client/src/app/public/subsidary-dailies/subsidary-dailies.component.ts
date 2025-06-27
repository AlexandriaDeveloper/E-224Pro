import { Component, inject, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
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

@Component({
  selector: 'app-subsidary-dailies',
  standalone: false,
  templateUrl: './subsidary-dailies.component.html',
  styleUrl: './subsidary-dailies.component.scss'
})
export class SubsidaryDailiesComponent implements OnInit {
  displayedColumns: string[] = ['action', 'id', 'name', 'dailyDate', 'dailyType', 'totalCredit', 'totalDebit', 'accountItem'];
  dataSource;
  params: GetSubsidiaryFormsByDailyIdRequest = new GetSubsidiaryFormsByDailyIdRequest();
  route = inject(ActivatedRoute);
  subsidaryService = inject(SubsidiaryService);
  collageService = inject(CollageService);
  fundService = inject(FundService);
  daily: Daily = null;
  range: { start: Date | null, end: Date | null } = { start: null, end: null };
  length = 50;
  pageSize = 30;
  pageIndex = 0;
  pageSizeOptions = [5, 15, 30];
  pageEvent: PageEvent;

  collages: Collage[] = [];
  funds: Fund[] = []



  constructor() {

  }
  ngOnInit(): void {
    this.loadCollages();
    this.route.paramMap.subscribe((p: any) => { this.params.accountId = p.get('subaccountId'); this.params.dailyId = p.get('dailyId'); this.loadDailies(this.params); });
    //  this.params.accountId = this.subsidaryId;

    //  this.loadDailies(this.params);
  }

  loadDailies(param: GetDailiesRequest) {
    this.subsidaryService.getSubsidaryDailies(this.params.accountId, param).subscribe((dailies: any) => {

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
    this.subsidaryService.downloadSubsidaryDailyPdf(this.params).subscribe((response: any) => {
      const blob = new Blob([response], { type: 'application/pdf' });
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    });

  }


}
