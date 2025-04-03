import { Component, ElementRef, inject, OnInit } from '@angular/core';
import { FormService } from '../../../shared/services/form.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { accountItems } from '../../../shared/_models/constants';
import { AccountService } from '../../../shared/services/account.service';
import { GetAccountRequest } from '../../../shared/_requests/getAccountRequest';
import { AccountInfoDialogComponent } from '../../account/account-info-dialog/account-info-dialog.component';

@Component({
  selector: 'app-add-form',
  standalone: false,
  templateUrl: './add-form.component.html',
  styleUrl: './add-form.component.scss'
})
export class AddFormComponent implements OnInit {
  totalCredit: number;
  totalDebit: number;
  readonly dialog2 = inject(MatDialog);
  ngOnInit(): void {
    console.log(this.data);

    this.addForm = this.initialForm();
    this.loadAccounts();
  }
  //dailyService = inject(FormService);

  accountService = inject(AccountService);
  formService = inject(FormService);
  fb = inject(FormBuilder);
  readonly data = inject<any>(MAT_DIALOG_DATA);
  addForm: FormGroup;
  getAccountRequest = new GetAccountRequest();
  accounts = [];

  readonly dialogRef = inject(MatDialogRef<AddFormComponent>);
  initialForm() {
    return this.fb.group({
      formName: ['', Validators.required],
      collageId: ['',],
      fundId: ['', Validators.required],
      num224: ['', Validators.required],
      num55: ['',],
      auditorName: ['',],
      details: ['',],
      dailyId: [this.data.param.DailyId, Validators.required],
      formDetails: this.fb.array([this.initialFormDetails()])
      // I need Form Array for account id - credit -debit- account type


    });
  }
  // I need Form Array for account id - credit -debit- account type
  initialFormDetails() {
    return this.fb.group({
      accountId: ['', Validators.required],
      credit: [null,],
      debit: [null,],
      accountName: ['', Validators.required],
      accountNumber: ['', Validators.required],
    });
  }

  loadAccounts() {
    return this.accountService.getAccounts(this.getAccountRequest).subscribe((x: []) => {
      this.accounts = x
    });
  }
  addFormDetail() {
    this.fDetails.push(this.initialFormDetails());
  }
  removeFormDetail(index: number) {
    this.fDetails.removeAt(index);
  }

  get f() { return this.addForm.controls; }
  get fDetails() {


    return this.addForm.controls['formDetails'] as FormArray;
  }

  updateTotalCredit() {
    this.fDetails.controls.forEach(control => {
      this.totalCredit = this.fDetails.value.reduce((acc, curr) => acc + curr.credit, 0);
      this.totalDebit = this.fDetails.value.reduce((acc, curr) => acc + curr.debit, 0);
    })
  }
  onSubmit() {
    console.log(this.addForm.value);

    if (this.addForm.valid) {
      this.addForm.value.formDetails.forEach(x => {
        x.credit = Number(x.credit);
        x.debit = Number(x.debit);
      });
      this.addForm.value.totalCredit = this.totalCredit;
      this.addForm.value.totalDebit = this.totalDebit;
      this.formService.addForm(this.addForm.value).subscribe(() => this.dialogRef.close());
    }

  }
  onNoClick() {
    this.dialogRef.close();
  }
  getAccountName(index, accountId: string) {
    console.log(this.fDetails);
    let acc = this.accounts.find(x => x.accountNumber == accountId);
    if (acc == null) {
      this.fDetails.controls[index].get('accountName').setValue('');
      this.fDetails.controls[index].get('accountId').setValue(null);
    }
    this.fDetails.controls[index].get('accountName').setValue(acc.accountName);
    this.fDetails.controls[index].get('accountId').setValue(acc.id);
  }

  getAccount(index) {
    // let acc = this.accounts[1]
    // console.log(this.fDetails);
    // this.fDetails.controls[index].get('accountNumber').setValue(acc.accountNumber);
    // this.fDetails.controls[index].get('accountName').setValue(acc.accountName);
    const dialogRef2 = this.dialog2.open(AccountInfoDialogComponent, {
      data: {

      },
      disableClose: true,
      hasBackdrop: true,
      minWidth: '80vw'
    });

    dialogRef2.afterClosed().subscribe(result => {
      console.log(result);
      this.fDetails.controls[index].get('accountNumber').setValue(result.accountNumber);
      this.fDetails.controls[index].get('accountName').setValue(result.accountName);
      this.fDetails.controls[index].get('accountId').setValue(result.id);

    });

  }


}
