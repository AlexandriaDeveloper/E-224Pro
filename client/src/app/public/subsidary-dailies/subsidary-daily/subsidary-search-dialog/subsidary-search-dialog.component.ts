import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CollageService } from '../../../../shared/services/collage.service';
import { FundService } from '../../../../shared/services/fund.service';
import { GetFundsRequest } from '../../../../shared/_requests/getFundsRequest';


@Component({
  selector: 'app-subsidary-search-dialog',
  templateUrl: './subsidary-search-dialog.component.html',
  styleUrls: ['./subsidary-search-dialog.component.scss'],
  standalone: false
})
export class SubsidarySearchDialogComponent implements OnInit {
  searchForm: FormGroup;
  collages: any[] = [];
  funds: any[] = [];

  constructor(
    public dialogRef: MatDialogRef<SubsidarySearchDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private collageService: CollageService,
    private fundService: FundService
  ) {
    this.searchForm = this.fb.group({
      formName: [''],
      num224: [''],
      num55: [''],
      collageId: [0],
      fundId: [0],
      auditorName: [''],
      isBalanced: ['']
    });

    // If search data is passed, populate the form
    if (data && data.searchParams) {
      this.searchForm.patchValue(data.searchParams);
    }
  }

  ngOnInit(): void {
    this.loadCollages();

    // If a collage ID is already selected, load its funds
    const collageId = this.searchForm.get('collageId')?.value;
    if (collageId && collageId > 0) {
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

  loadFunds(collageId: number): void {
    if (!collageId || collageId <= 0) {
      this.loadAllFunds();
      return;
    }

    this.fundService.getFundsByCollageId(
      { collageId: collageId } as GetFundsRequest
    ).subscribe(
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
    const searchParams = this.searchForm.value;
    this.dialogRef.close({ searchParams: searchParams, applied: true });
  }

  clear(): void {
    this.searchForm.reset();
    // Reset all form fields
    this.searchForm.patchValue({
      formName: '',
      num224: '',
      num55: '',
      collageId: 0,
      fundId: 0,
      auditorName: '',
      isBalanced: ''
    });
  }

  onNoClick(): void {
    this.dialogRef.close({ applied: false });
  }
}
