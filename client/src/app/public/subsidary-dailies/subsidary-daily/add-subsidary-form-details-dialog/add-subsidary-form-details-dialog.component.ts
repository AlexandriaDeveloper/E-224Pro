import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AddFormComponent } from '../../../form/add-form/add-form.component';
import { SubaccountService } from '../../../../shared/services/subaccount.service';
import { SubAccount } from '../../../../shared/_models/fund.model';
import { GetSubAccountRequest } from '../../../../shared/_requests/GetSubAccountRequest';

@Component({
  selector: 'app-add-subsidary-form-details-dialog',
  standalone: false,
  templateUrl: './add-subsidary-form-details-dialog.component.html',
  styleUrl: './add-subsidary-form-details-dialog.component.scss'
})
export class AddSubsidaryFormDetailsDialogComponent implements OnInit {
  ngOnInit(): void {
    console.log(this.data);
    this.loadSubAccounts();
  }
  private readonly fb = inject(FormBuilder);
  private readonly dialog = inject(MatDialog);
  private readonly dialogRef = inject(MatDialogRef<AddFormComponent>);
  public readonly data = inject<any>(MAT_DIALOG_DATA);
  public subAccountService = inject(SubaccountService)
  subAccounts: SubAccount[] = []

  subAccountRequest: GetSubAccountRequest = new GetSubAccountRequest();
  //private readonly subAccountService = inject(SubAccountService);


  loadSubAccounts() {
    this.subAccountService.getAccounts(41, this.subAccountRequest).subscribe((res: SubAccount[]) => {
      console.log(res);

      this.subAccounts = res;
    })
  }
  initilizeForm() {
    return this.fb.group({
      id: [''],
      num224: [''],
      num55: [''],
      formName: [''],
      formDetailsId: [''],
      collageName: [''],
      fundName: [''],
      totalCredit: [''],
      totalDebit: ['']
    })
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

}
