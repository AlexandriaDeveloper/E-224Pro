import { Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AddFormComponent } from '../add-form/add-form.component';
import { AccountService } from '../../../shared/services/account.service';
import { FormService } from '../../../shared/services/form.service';
export class Account {
  id: number;
  accountName: string;
  accountNumber: string;
}

@Component({
  selector: 'app-download-excel-template-dialog',
  standalone: false,
  templateUrl: './download-excel-template-dialog.component.html',
  styleUrl: './download-excel-template-dialog.component.scss'
})
export class DownloadExcelTemplateDialogComponent implements OnInit {

  private readonly fb = inject(FormBuilder);
  private readonly dialog = inject(MatDialog);
  private readonly dialogRef = inject(MatDialogRef<AddFormComponent>);
  private readonly data = inject<any>(MAT_DIALOG_DATA);
  private readonly accountService = inject(AccountService)
  private readonly formService = inject(FormService)
  downloadForm: FormGroup;
  get f() { return this.downloadForm.controls; }
  get fAccounts() { return this.downloadForm.get('accounts') as FormArray; }
  accounts: Account[] = [];


  ngOnInit(): void {
    this.downloadForm = this.initilizeForm(); // Initialize downloadForm immediately
    this.loadAccounts();
  }

  private loadAccounts(): void {
    this.accountService.getAccounts({}).subscribe(
      (accounts: Account[]) => {
        this.accounts = accounts;
        console.log(this.accounts);
      },
      error => {
        console.error('Error loading accounts:', error);
      }
    );
  }
  initilizeForm(): FormGroup {
    return this.fb.group({
      accounts: this.fb.array([this.createEmptyAccountGroup()]), // Initialize with one empty group
    });
  }

  private createEmptyAccountGroup(): FormGroup {
    return this.fb.group({

      debitAccountNumber: [],
      debitAccountName: [],
      creditAccountNumber: [],
      creditAccountName: [],
    });
  }


  addAccountRow() {
    this.fAccounts.push(this.createEmptyAccountGroup());
  }


  private createAccount(account: any): FormGroup {


    return this.fb.group({

      creditAccountNumber: [account.accountNumber || 0],
      creditAccountName: [account.accountName || ''],
      debitAccountNumber: [account.accountNumber || 0],
      debitAccountName: [account.accountName || ''],

    });

  }

  getAccountName(accountType: string, index: number, accountId: string): void {
    console.log(accountType, accountId);

    const account = this.accounts.find(acc => acc.accountNumber === accountId);
    const control = this.fAccounts.at(index);
    console.log(account);

    if (!account) {
      control.patchValue({
        accountName: '',
        accountId: null
      });
      return;
    }
    if (accountType === 'credit') {

      control.patchValue({
        creditAccountName: account.accountName,
        creditAccountNumber: account.accountNumber,
      });

    } else {

      control.patchValue({
        debitAccountName: account.accountName,
        debitAccountNumber: account.accountNumber,
      });

    }

  }

  onSubmit() {
    //this.dialogRef.close(this.downloadForm.value);

    this.formService.downloadDailyExcelFormTemplate(this.downloadForm.value).subscribe({
      next: (response: any) => {

        //dowload excel file 

        const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        window.open(url);
      },
      error: (error) => {
        console.error('Error downloading template:', error);
      }
    });
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

}
