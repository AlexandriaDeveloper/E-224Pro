import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormService } from '../../shared/services/form.service';
import { GetFormRequest } from '../../shared/_requests/getFormRequest';
import { PageEvent } from '@angular/material/paginator';
import { PaginatorModel } from '../../shared/_models/paginator.model';
import { AddFormComponent } from './add-form/add-form.component';
import { MatDialog } from '@angular/material/dialog';

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
  dataSource;
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

    this.formService.getForms(param).subscribe((res: any) => {
      this.data = res;
      this.dataSource = res.formDtos;
      this.paginator.length = res.totalCount;
    });

  }

  handlePageEvent(e: PageEvent) {
    this.paginator.pageEvent = e;
    this.paginator.length = e.length;
    this.params.pageSize = e.pageSize;
    this.params.pageIndex = e.pageIndex;
    this.loadForms(this.params);
  }
  openAddFormDialog() {
    const dialogRef = this.dialog.open(AddFormComponent, {
      data: {
        param: this.params
      },
      disableClose: true,
      hasBackdrop: true,
      minWidth: '60vw',
      maxHeight: '90vh'
    });

    dialogRef.afterClosed().subscribe(result => {

    });
  }
}
