import { Component, inject, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { GetDailiesRequest } from '../../shared/_requests/getDailiesRequest';
import { Collage } from '../../shared/_models/collage.model';
import { Daily } from '../../shared/_models/Daily.model';
import { CollageService } from '../../shared/services/collage.service';
import { DailiesService } from '../../shared/services/dailies.service';
import { ActivatedRoute } from '@angular/router';
import { SubsidiaryService } from '../../shared/services/subsidiary.service';

@Component({
  selector: 'app-subsidary-dailies',
  standalone: false,
  templateUrl: './subsidary-dailies.component.html',
  styleUrl: './subsidary-dailies.component.scss'
})
export class SubsidaryDailiesComponent implements OnInit {
  displayedColumns: string[] = ['action', 'id', 'name', 'dailyDate', 'dailyType', 'totalCredit', 'totalDebit', 'accountItem'];
  dataSource;
  params: GetDailiesRequest = new GetDailiesRequest();
  route = inject(ActivatedRoute);
  subsidaryService = inject(SubsidiaryService);
  daily: Daily = null;
  range: { start: Date | null, end: Date | null } = { start: null, end: null };
  length = 50;
  pageSize = 30;
  pageIndex = 0;
  pageSizeOptions = [5, 15, 30];
  pageEvent: PageEvent;

  collages: Collage[] = [];
  collageService = inject(CollageService);


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
  handlePageEvent(e: PageEvent) {

    console.log('Page Event:', e);
    this.pageEvent = e;
    this.length = e.length;
    this.params.pageSize = e.pageSize;
    this.params.pageIndex = e.pageIndex;
    this.loadDailies(this.params);
  }

  onDailyTypeChange(fundId) {
    this.params.dailyType = fundId;
    //this.filterdFunds = this.funds.filter(x => x.collageId == this.params.CollageId);
    //this.loadDailies(this.params);
  }
  submit() {

    this.params.startDate = this.range.start;
    this.params.endDate = this.range.end;
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






}
