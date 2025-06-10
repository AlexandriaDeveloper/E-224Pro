import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Collage } from '../../../shared/_models/collage.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Fund } from '../../../shared/_models/fund.model';
import { FundService } from '../../../shared/services/fund.service';

@Component({
  selector: 'app-add-fund-dialog',
  standalone: false,
  templateUrl: './add-fund-dialog.component.html',
  styleUrl: './add-fund-dialog.component.scss'
})
export class AddFundDialogComponent implements OnInit {
  addFundForm: FormGroup
  fb = inject(FormBuilder)
  fundService = inject(FundService)
  private readonly dialogRef = inject(MatDialogRef<AddFundDialogComponent>);
  readonly data = inject<any>(MAT_DIALOG_DATA);
  collages: Collage[] = [];
  Fund = new Fund();
  updateForm = false;
  get f() { return this.addFundForm.controls; }
  ngOnInit(): void {
    if (this.data.element != null) {
      this.Fund = this.data.element;
      this.updateForm = true;
    }
    this.collages = this.data.collages;
    console.log(this.data);

    this.addFundForm = this.initilizeForm();
  }

  initilizeForm(): FormGroup {
    return this.fb.group({
      fundName: [this.Fund.fundName, Validators.required],
      collageId: [this.Fund.collageId, Validators.required],
      fundCode: [this.Fund.fundCode, Validators.required],

    });
  }
  getCollageId(collageId: any): string {
    console.log(collageId);

    this.Fund.collageId = collageId;
    const collage = this.collages.find(c => c.id === collageId);
    return collage ? collage.collageName : 'Unknown Collage'; // Return a default value if not found
  }
  /*************  ✨ Windsurf Command ⭐  *************/
  /**
   * Submit the add fund form.
   *
   * If the form is valid, it will update or add the fund to the server.
   * The form values are passed to the fund service and if the update is
   * successful, the dialog is closed with the form values.
   */
  /*******  40b00d4d-e6ef-46d3-a6d9-f1e91acd7e80  *******/
  onSubmit() {

    console.log(this.Fund);
    this.Fund = { ...this.Fund, ...this.addFundForm.value };

    console.log(this.Fund);

    if (this.addFundForm.valid) {
      if (this.updateForm) {
        this.fundService.putFund(this.Fund.id, this.Fund).subscribe((response) => {
          console.log(response);
        })
      }
      else {
        this.fundService.postFund(this.Fund).subscribe((response) => {
          console.log(response);
        })

      }

      this.dialogRef.close();
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
  clear() {
    this.dialogRef.close();
  }
}
