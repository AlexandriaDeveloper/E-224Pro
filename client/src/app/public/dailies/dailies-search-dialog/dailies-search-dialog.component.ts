import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DailiesService } from '../../../shared/services/dailies.service';
import { GetDailiesRequest } from '../../../shared/_requests/getDailiesRequest';
import { accountItems, dailyTypes } from '../../../shared/_models/constants';

@Component({
  selector: 'app-dailies-search-dialog',
  standalone: false,
  templateUrl: './dailies-search-dialog.component.html',
  styleUrl: './dailies-search-dialog.component.scss'
})
export class DailiesSearchDialogComponent implements OnInit {
  dailyService = inject(DailiesService);
  fb = inject(FormBuilder);
  readonly dialogRef = inject(MatDialogRef<DailiesSearchDialogComponent>);
  readonly data = inject<any>(MAT_DIALOG_DATA);
  dailyTypes = dailyTypes
  accountItems = accountItems;
  searchForm: FormGroup;
  dailyParam: GetDailiesRequest = new GetDailiesRequest();
  //range: { start: Date | null, end: Date | null } = { start: null, end: null };
  /**
   *
   */
  constructor() {


  }
  ngOnInit(): void {
    console.log(this.data.param);

    Object.assign(this.dailyParam, this.data.param);

    this.searchForm = this.initialForm();
  }
  initialForm() {
    return this.fb.group({
      name: [this.dailyParam.name],
      dailyType: [this.dailyParam.dailyType],
      accountItem: [this.dailyParam.accountItem],
      startDate: [this.dailyParam.startDate],
      endDate: [this.dailyParam.endDate]
    });
  }

  onSubmit() {
    this.dailyParam = this.searchForm.value;
    // if (this.range.start && this.range.end) {
    //   this.dailyParam.startDate = this.range.start;
    //   this.dailyParam.endDate = this.range.end;
    // }


    console.log(this.dailyParam);

    this.dialogRef.close(this.dailyParam);

  }
  onNoClick(): void {
    this.dialogRef.close();
  }
  clear() {
    this.dailyParam = new GetDailiesRequest();
    this.searchForm.reset();

  }

}
