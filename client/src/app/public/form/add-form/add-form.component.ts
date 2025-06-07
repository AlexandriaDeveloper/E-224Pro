import { Component, ElementRef, inject, OnInit } from '@angular/core';
import { FormService } from '../../../shared/services/form.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AccountService } from '../../../shared/services/account.service';
import { GetAccountRequest } from '../../../shared/_requests/getAccountRequest';
import { AccountInfoDialogComponent } from '../../account/account-info-dialog/account-info-dialog.component';
import { FundService } from '../../../shared/services/fund.service';
import { FormDetailDto, FormDto } from '../../../shared/_models/forms/FormDto.model';

interface Account {
  id: number;
  accountName: string;
  accountNumber: string;
}

interface Fund {
  id: number;
  fundName: string;
}

@Component({
  selector: 'app-add-form',
  standalone: false,
  templateUrl: './add-form.component.html',
  styleUrl: './add-form.component.scss'
})
export class AddFormComponent implements OnInit {
  // Services
  private readonly fundService = inject(FundService);
  private readonly accountService = inject(AccountService);
  private readonly formService = inject(FormService);
  private readonly fb = inject(FormBuilder);
  private readonly dialog = inject(MatDialog);
  private readonly dialogRef = inject(MatDialogRef<AddFormComponent>);
  private readonly data = inject<any>(MAT_DIALOG_DATA);

  // Form related
  addForm: FormGroup;
  formDetails: FormDetailDto[] = [];
  private formDetail: FormDetailDto | null = null;

  // Data
  accounts: Account[] = [];
  funds: Fund[] = [];
  totalCredit = 0;
  totalDebit = 0;
  update = false;

  // Request objects
  private getAccountRequest = new GetAccountRequest();

  ngOnInit(): void {
    this.loadAccounts();
    this.initializeForm();
  }

  private initializeForm(): void {
    console.log(this.data.element);

    if (this.data.element) {
      this.update = true;
      this.formDetails = [];
      this.loadFunds(this.data.element.collageId);
      this.loadFormDetails();
    }
    this.addForm = this.createForm();
  }

  private createForm(): FormGroup {
    console.log(this.formDetails);
    return this.fb.group({
      formName: [this.data.element?.formName || '', Validators.required],
      collageId: [this.data.element?.collageId || null],
      fundId: [this.data.element?.fundId || null, Validators.required],
      num224: [this.data.element?.num224 || '', Validators.required],
      num55: [this.data.element?.num55 || ''],
      auditorName: [this.data.element?.auditorName || ''],
      details: [this.data.element?.details || ''],
      dailyId: [this.data.param.DailyId, Validators.required],
      formDetails: this.fb.array(this.formDetails.map(detail => this.createFormDetail(detail)))
    });
  }

  private createFormDetail(detail: FormDetailDto): FormGroup {
    const account = this.accounts.find(acc => acc.id === detail.accountId) || {
      id: 0,
      accountName: '',
      accountNumber: ''
    };

    return this.fb.group({
      id: [detail.id || 0],
      accountId: [detail.accountId, Validators.required],
      credit: [detail.credit || 0],
      debit: [detail.debit || 0],
      accountName: [account.accountName, Validators.required],
      accountNumber: [account.accountNumber, Validators.required],
    });
  }

  // Form getters
  get f() { return this.addForm.controls; }
  get fDetails() { return this.addForm.get('formDetails') as FormArray; }

  // Form actions
  addFormDetail(): void {
    this.fDetails.push(this.createFormDetail(new FormDetailDto()));
  }

  removeFormDetail(index: number): void {
    this.fDetails.removeAt(index);


    this.updateTotals();
  }

  // Data loading
  private loadAccounts(): void {
    this.accountService.getAccounts(this.getAccountRequest).subscribe(
      (accounts: Account[]) => this.accounts = accounts
    );
  }

  private loadFunds(collageId: number): void {
    this.fundService.getFundsByCollageId({ collageId }).subscribe(
      (funds: Fund[]) => this.funds = funds
    );
  }

  private loadFormDetails(): void {


    if (!this.data.element?.id) return;

    this.formService.getFormDetails(this.data.element.id).subscribe(
      (details: FormDetailDto[]) => {
        console.log(details);
        this.formDetails = details;
        details.forEach(detail => {
          this.fDetails.push(this.createFormDetail(detail));
        });
      }
    );
  }

  // Account handling
  getAccountName(index: number, accountId: string): void {
    const account = this.accounts.find(acc => acc.accountNumber === accountId);
    const control = this.fDetails.at(index);

    if (!account) {
      control.patchValue({
        accountName: '',
        accountId: null
      });
      return;
    }

    control.patchValue({
      accountName: account.accountName,
      accountId: account.id
    });
  }

  getAccount(index: number): void {
    const dialogRef = this.dialog.open(AccountInfoDialogComponent, {
      data: {},
      disableClose: true,
      hasBackdrop: true,
      minWidth: '80vw'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const control = this.fDetails.at(index);
        control.patchValue({
          accountNumber: result.accountNumber,
          accountName: result.accountName,
          accountId: result.id
        });
      }
    });
  }

  // Fund handling
  getFunds(collageId: number): void {
    this.loadFunds(collageId);
  }

  // Totals calculation
  updateTotals(): void {
    const values = this.fDetails.value;
    this.totalCredit = values.reduce((sum, curr) => sum + (Number(curr.credit) || 0), 0);
    this.totalDebit = values.reduce((sum, curr) => sum + (Number(curr.debit) || 0), 0);
  }

  // Form submission
  onSubmit(): void {
    if (!this.addForm.valid) return;

    const formValue = this.addForm.value;

    formValue.formDetails = this.fDetails.value.map(detail => ({
      ...detail,
      credit: Number(detail.credit) || 0,
      debit: Number(detail.debit) || 0
    }));
    formValue.totalCredit = this.totalCredit;
    formValue.totalDebit = this.totalDebit;
    console.log(formValue);
    if (this.update) {

      this.formService.updateForm(this.data.element.id, formValue).subscribe(() => this.dialogRef.close());

    }
    else {

      this.formService.addForm(formValue).subscribe(() => this.dialogRef.close());
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
