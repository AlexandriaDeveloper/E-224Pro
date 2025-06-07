import { Component, inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { accountItems, dailyTypes } from '../../../shared/_models/constants';
import { FormGroup } from '@angular/forms';
import { GetDailiesRequest } from '../../../shared/_requests/getDailiesRequest';

@Component({
  selector: 'app-dailies-report-dialog',
  standalone: false,
  templateUrl: './dailies-report-dialog.component.html',
  styleUrl: './dailies-report-dialog.component.scss'
})
export class DailiesReportDialogComponent {
  readonly dialogRef = inject(MatDialogRef<DailiesReportDialogComponent>);
  readonly data = inject<any>(MAT_DIALOG_DATA);
  dailyTypes = dailyTypes
  accountItems = accountItems;
  searchForm: FormGroup;
  dailyParam: GetDailiesRequest = new GetDailiesRequest();

  onSubmit(): void {
    this.dialogRef.close(this.data);
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
}
