import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../../shared/services/account.service';
import { GetAccountRequest } from '../../../shared/_requests/getAccountRequest';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-account-info-dialog',
  standalone: false,
  templateUrl: './account-info-dialog.component.html',
  styleUrl: './account-info-dialog.component.scss'
})
export class AccountInfoDialogComponent implements OnInit {
  ngOnInit(): void {
    this.loadAccounts();
  }
  accountService = inject(AccountService);
  getAccountRequest = new GetAccountRequest();
  dataSource = [];

  credit = [];
  debit = [];
  displayedColumns: string[] = ['actions', 'accountNumber', 'accountName',];
  readonly data = inject<any>(MAT_DIALOG_DATA);
  readonly dialogRef = inject(MatDialogRef<AccountInfoDialogComponent>);
  loadAccounts() {
    return this.accountService.getAccounts(this.getAccountRequest).subscribe((x: any) => {
      console.log(x);
      this.dataSource = x
      this.credit = this.dataSource.filter(x => x.accountStatus === 'Credit');
      this.debit = this.dataSource.filter(x => x.accountStatus === 'Debit');
      console.log(this.dataSource);
    });
  }
  searchDebitByName(ev) {
    console.log(ev.srcElement.value);

    let search = ev.srcElement.value;
    //filter debit

    this.debit = this.dataSource.filter(x => x.accountStatus === 'Debit');

    this.debit = this.debit.filter(x => x.accountName.includes(search));
    // this.debit = this.dataSource.filter(x => x.accountStatus === 'Debit');


  }
  searchCreditByName(ev) {
    let search = ev.srcElement.value;
    //filter credit

    this.credit = this.dataSource.filter(x => x.accountStatus === 'Credit');

    this.credit = this.credit.filter(x => x.accountName.includes(search));
    // this.debit = this.dataSource.filter(x => x.accountStatus === 'Debit');


  }
  copyAccount(account) {
    this.dialogRef.close(account);
  }
  onNoClick() {
    this.dialogRef.close();
  }
}
