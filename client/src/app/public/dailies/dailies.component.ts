import { Component, inject, OnInit } from '@angular/core';
import { DailiesService } from '../../shared/services/dailies.service';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { DailiesSearchDialogComponent } from './dailies-search-dialog/dailies-search-dialog.component';
import { GetDailiesRequest } from '../../shared/_requests/getDailiesRequest';
import { AddDailyComponent } from './add-daily/add-daily.component';
import { DeleteDialogComponent } from '../../shared/components/dialog/delete-dialog/delete-dialog.component';
import { Daily } from '../../shared/_models/Daily.model';
import { DailiesReportDialogComponent } from './dailies-report-dialog/dailies-report-dialog.component';
import { CollageService } from '../../shared/services/collage.service';
import { Collage } from '../../shared/_models/collage.model';
import { PaginatorModel } from '../../shared/_models/paginator.model';



export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}


@Component({
  selector: 'app-dailies',
  standalone: false,
  templateUrl: './dailies.component.html',
  styleUrl: './dailies.component.scss'
})
export class DailiesComponent implements OnInit {
  displayedColumns: string[] = ['action', 'id', 'name', 'dailyDate', 'dailyType', 'accountItem'];
  dataSource;
  params: GetDailiesRequest = new GetDailiesRequest();
  daily: Daily = null;

  paginator: PaginatorModel = new PaginatorModel();
  pageEvent: PageEvent;
  readonly dialog = inject(MatDialog);
  collages: Collage[] = [];
  collageService = inject(CollageService);


  constructor(private dailiesService: DailiesService) {

  }
  ngOnInit(): void {
    this.loadCollages();
    this.loadDailies(this.params);
  }

  loadDailies(param: GetDailiesRequest) {
    this.dailiesService.getDailies(param).subscribe((dailies: any) => {

      this.dataSource = dailies.items;
      this.paginator.length = dailies.totalCount;
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
    this.paginator.length = e.length;
    this.params.pageSize = e.pageSize;
    this.params.pageIndex = e.pageIndex;
    this.loadDailies(this.params);
  }

  openSearchDialog() {
    const dialogRef = this.dialog.open(DailiesSearchDialogComponent, {
      data: {
        param: this.params
      },
      disableClose: true,
      hasBackdrop: true
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      Object.assign(this.params, result);
      console.log(this.params);

      this.loadDailies(this.params);

      if (result !== undefined) {
        //this.animal.set(result);

      }
    });
  }

  openDailiesReportDialog() {
    const dialogRef = this.dialog.open(DailiesReportDialogComponent, {
      data: {
        param: this.params,
        collages: this.collages
      },
      disableClose: true,
      hasBackdrop: true,
      minWidth: '40vw'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  openAddDialog(element: Daily = null) {
    const dialogRef = this.dialog.open(AddDailyComponent, {
      data: {
        daily: element
      },
      disableClose: true,
      hasBackdrop: true,
      minWidth: '40vw'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadDailies(this.params);
    });
  }
  openDeleteDialog(element) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {
        message: `انت على وشك حذف يوميه ${element.name} هل انت متأكد ؟!`
      },
      disableClose: true,
      hasBackdrop: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.dailiesService.deleteDaily(element.id).subscribe(() => this.loadDailies(this.params));
        this.loadDailies(this.params);
      }


    });
  }

  editDaily(element) {

    this.openAddDialog(element);
  }


}
