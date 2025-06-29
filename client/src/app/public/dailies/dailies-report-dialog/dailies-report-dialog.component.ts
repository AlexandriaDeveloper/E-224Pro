import { Component, inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { accountItems, dailyTypes } from '../../../shared/_models/constants';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GetDailiesRequest } from '../../../shared/_requests/getDailiesRequest';
import { Collage } from '../../../shared/_models/collage.model';
import { FundService } from '../../../shared/services/fund.service';
import { GetFundsRequest } from '../../../shared/_requests/getFundsRequest';
import { DailiesService } from '../../../shared/services/dailies.service';

@Component({
  selector: 'app-dailies-report-dialog',
  standalone: false,
  templateUrl: './dailies-report-dialog.component.html',
  styleUrl: './dailies-report-dialog.component.scss'
})
export class DailiesReportDialogComponent implements OnInit {
  readonly dialogRef = inject(MatDialogRef<DailiesReportDialogComponent>);
  readonly data = inject<any>(MAT_DIALOG_DATA);
  fundService = inject(FundService);
  dailyService = inject(DailiesService);
  fb = inject(FormBuilder);
  dailyTypes = dailyTypes
  accountItems = accountItems;
  searchForm: FormGroup;
  dailyParam: GetDailiesRequest = new GetDailiesRequest();
  collages: Collage[] = [];
  funds: any[] = [];


  ngOnInit(): void {
    this.collages = this.data.collages;
    this.searchForm = this.initilizeForm();
    // this.dailyParam = this.data.param;
    // this.searchForm.patchValue(this.dailyParam);
  }
  initilizeForm() {
    return this.fb.group({

      startDate: [this.data.param.startDate, Validators.required],
      endDate: [this.data.param.endDate, Validators.required],
      entryType: [this.data.param.entryType],

      dailyType: [this.data.param.dailyType],
      collageId: [this.data.param.collageId],
      fundId: [this.data.param.fundId],
    });
  }
  getFunds(collageId: number) {

    if (collageId) {
      this.fundService.getFundsByCollageId({ collageId: collageId } as GetFundsRequest).subscribe((funds: any[]) => {
        this.funds = funds;
      });
    }
    else {
      this.funds = [];
    }

  }

  onSubmit(): void {
    let formValue = this.searchForm.value as GetDailiesRequest;
    console.log(formValue);

    this.dailyService.downloadDailieRportPdf(formValue).subscribe({
      next: (response: any) => {

        //dowload pdf file 

        // const blob = new Blob([response], { type: 'application/pdf' });
        // const url = window.URL.createObjectURL(blob);
        // const a = document.createElement('a');
        // a.href = url;
        // a.download = 'Report.pdf';
        // document.body.appendChild(a);
        // a.click();
        // window.URL.revokeObjectURL(url);
        // a.remove();

        const blob = new Blob([response], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      },
      error: (error) => {
        console.error('Error downloading template:', error);
      }
    });

    // this.dialogRef.close(this.data);
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
  clear() {
    this.dailyParam = new GetDailiesRequest();
    this.searchForm.reset();
    this.data.param = this.dailyParam;
    this.dialogRef.close(this.data);
  }
}
