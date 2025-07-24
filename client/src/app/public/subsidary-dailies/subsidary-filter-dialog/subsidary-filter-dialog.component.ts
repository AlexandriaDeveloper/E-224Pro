import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CollageService } from '../../../shared/services//collage.service';
import { FundService } from '../../../shared/services/fund.service';
import { GetFundsRequest } from '../../../shared/_requests/getFundsRequest';

@Component({
  selector: 'app-subsidary-filter-dialog',
  templateUrl: './subsidary-filter-dialog.component.html',
  styleUrls: ['./subsidary-filter-dialog.component.scss'],
  standalone: false
})
export class SubsidaryFilterDialogComponent implements OnInit {
  filterForm: FormGroup;
  collages: any[] = [];
  funds: any[] = [];

  constructor(
    public dialogRef: MatDialogRef<SubsidaryFilterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private collageService: CollageService,
    private fundService: FundService
  ) {
    this.filterForm = this.fb.group({
      dailyType: [''],
      entryType: [''],
      collageId: [''],
      fundId: [''],
      startDate: [''],
      endDate: ['']
    });

    // If filter data is passed, populate the form
    if (data && data.filters) {
      this.filterForm.patchValue(data.filters);
    }
  }

  ngOnInit(): void {
    this.loadCollages();

    // If a collage ID is already selected, load its funds
    const collageId = this.filterForm.get('collageId')?.value;
    if (collageId) {
      this.loadFunds(collageId);
    } else {
      this.loadAllFunds();
    }
  }

  loadCollages(): void {
    this.collageService.getCollages().subscribe(
      (res: any) => {
        this.collages = res;
      },
      (error: any) => {
        console.error('Error loading collages:', error);
      }
    );
  }

  loadFunds(collageId: string): void {
    if (!collageId) {
      this.loadAllFunds();
      return;
    }

    this.fundService.getFundsByCollageId({ collageId: +collageId } as GetFundsRequest).subscribe(
      (res: any) => {
        this.funds = res;
      },
      (error: any) => {
        console.error('Error loading funds for collage:', error);
      }
    );
  }

  loadAllFunds(): void {
    this.fundService.getFundsByCollageId(null).subscribe(
      (res: any) => {
        this.funds = res;
      },
      (error: any) => {
        console.error('Error loading all funds:', error);
      }
    );
  }

  onSubmit(): void {
    const filters = this.filterForm.value;
    this.dialogRef.close({ filters: filters, applied: true });
  }

  clear(): void {
    this.filterForm.reset();
    // Reset all form fields
    Object.keys(this.filterForm.controls).forEach(key => {
      this.filterForm.get(key)?.setValue('');
    });
  }

  onNoClick(): void {
    this.dialogRef.close({ applied: false });
  }
}
