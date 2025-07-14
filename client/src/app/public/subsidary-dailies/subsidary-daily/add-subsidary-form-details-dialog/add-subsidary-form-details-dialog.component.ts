import { AfterViewInit, ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AddFormComponent } from '../../../form/add-form/add-form.component';
import { SubaccountService } from '../../../../shared/services/subaccount.service';
import { SubAccount } from '../../../../shared/_models/fund.model';
import { GetSubAccountRequest } from '../../../../shared/_requests/GetSubAccountRequest';
import { FormDto, SubsidaryFormDetailDto } from '../../../../shared/_models/forms/FormDto.model';
import { ActivatedRoute } from '@angular/router';
import { SubsidiaryService } from '../../../../shared/services/subsidiary.service';

@Component({
  selector: 'app-add-subsidary-form-details-dialog',
  standalone: false,
  templateUrl: './add-subsidary-form-details-dialog.component.html',
  styleUrl: './add-subsidary-form-details-dialog.component.scss'
})
export class AddSubsidaryFormDetailsDialogComponent implements OnInit, AfterViewInit {
  ngAfterViewInit(): void {
    // this.addSubsidaryForm = this.initilizeForm();


  }

  private readonly fb = inject(FormBuilder);
  private readonly dialog = inject(MatDialog);
  private readonly dialogRef = inject(MatDialogRef<AddFormComponent>);
  public readonly data = inject<any>(MAT_DIALOG_DATA);
  public subAccountService = inject(SubaccountService)
  public subSidatyService = inject(SubsidiaryService)
  addSubsidaryForm: FormGroup;
  router = inject(ActivatedRoute);

  private cdr = inject(ChangeDetectorRef);


  subsidaryDetails: any = [];
  accountId: number;
  funds = [];
  //accounts;
  totalCredit;
  totalDebit;
  subsidaryFormDetails: SubsidaryFormDetailDto[] = [];
  subAccountRequest: GetSubAccountRequest = new GetSubAccountRequest();
  form: any = {};
  get f() { return this.addSubsidaryForm.controls; }
  get fDetails() { return this.addSubsidaryForm.get('formDetails') as FormArray; }
  //private readonly subAccountService = inject(SubAccountService);
  ngOnInit(): void {
    console.log(this.data);



    if (this.data) {
      console.log(this.data);

      this.form = this.data.element

    }
    //loadSubsidaryFormDetails then initialize form
    this.loadSubsidaryFormDetails();
    // this.addSubsidaryForm = this.initilizeForm();
    // this.fDetails.valueChanges.subscribe(() => {
    //   this.updateTotals();
    // });


  }


  loadSubsidaryFormDetails() {



    this.subSidatyService.getSubsidartFormDetails(this.data.accountId, this.data.element.formDetailsId).subscribe(
      (res: SubsidaryFormDetailDto[]) => {
        this.subsidaryDetails = res;
        console.log(res);
      },
      (error) => {
        console.error('Error loading subsidary form details:', error);
      },
      () => {
        this.addSubsidaryForm = this.initilizeForm();
        this.totalCredit = this.form.totalCredit;
        this.totalDebit = this.form.totalDebit;
        // Move the subscription here:
        this.fDetails.valueChanges.subscribe(() => {
          this.updateTotals();
          this.cdr.detectChanges();
        });
        console.log(this.addSubsidaryForm);
      }
    );
  }
  initilizeForm() {

    return this.fb.group({
      // id: [this.form.id],
      // num224: [this.form.num224],
      // num55: [this.form.num55],
      // formName: [this.form.formName],
      // formDetailsId: [this.form.formDetailsId],
      // collageName: [this.form.collageName],
      // fundName: [this.form.fundName],
      // fundId: [this.form.fundId],
      // auditorName: [this.form.auditorName],
      // collageId: [this.form.collageId],
      // totalCredit: [this.form.totalCredit],
      // totalDebit: [this.form.totalDebit],
      // details: [this.form.details],
      formDetailsId: [this.form.formDetailsId],
      formDetails: this.fb.array(this.subsidaryDetails.map(detail => this.createFormDetail(detail))),
    })
  }
  addFormDetail(): void {

    this.fDetails.push(this.createFormDetail(new SubAccount()));

  }

  private createFormDetail(detail: any): FormGroup {
    return this.fb.group({
      id: [detail.id || 0],
      accountId: [detail.subAccountId, Validators.required],
      credit: [detail.credit || 0],
      debit: [detail.debit || 0],
      accountName: [detail.subAccountName, Validators.required],
      accountNumber: [detail.subAccountNumber, Validators.required],
    });


  }


  // Totals calculation
  updateTotals(): string {
    const values = this.fDetails.value;
    this.totalCredit = values.reduce((sum, curr) => sum + (Number(curr.credit) || 0), 0);
    this.totalDebit = values.reduce((sum, curr) => sum + (Number(curr.debit) || 0), 0);
    return `فرق دائن ${this.form.totalCredit - this.totalCredit} -  فرق  مدين ${this.form.totalDebit - this.totalDebit}`;
  }
  isBalanced(): boolean {
    return this.form.totalDebit === this.totalDebit && this.form.totalCredit === this.totalCredit;
  }
  getFunds(value) {

  } onSubmit(): void {
    // debugger
    // if (this.addSubsidaryForm.invalid) {
    //   return;
    // }

    // Prepare the request object
    const request = {
      formDetailsId: this.form.formDetailsId,
      subsidaryFormDetailsDtos: this.fDetails.value.map((detail: any) => ({
        id: detail.id || 0,
        subAccountId: detail.accountId,
        subAccountNumber: detail.accountNumber,
        subAccountName: detail.accountName,
        debit: detail.debit || 0,
        credit: detail.credit || 0
      }))
    };

    console.log('Submitting request:', request);

    // Call the service to save data
    this.subSidatyService.addOrUpdateSubsidaryFormDetails(request).subscribe({
      next: (response) => {
        console.log('Success:', response);
        // Close dialog with success result
        this.dialogRef.close({ success: true, data: response });
      },
      error: (error) => {
        console.error('Error saving subsidary form details:', error);
        // You can add error handling here (show toast, alert, etc.)
        // For now, just log the error
      }
    });
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

}
