import { ChangeDetectionStrategy, Component, inject, type OnInit } from '@angular/core';
import { SearchFormRequest } from '../../../../shared/_requests/getFormRequest';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CollageService } from '../../../../shared/services/collage.service';
import { Collage } from '../../../../shared/_models/collage.model';
import { Fund } from '../../../../shared/_models/fund.model';
import { FundService } from '../../../../shared/services/fund.service';

@Component({
  selector: 'app-search-file-dialog',
  standalone: false,
  templateUrl: './Search-File-Dialog.component.html',
  styleUrl: './Search-File-Dialog.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchFileDialogComponent implements OnInit {
  // Define any properties or methods needed for the component
  searchRequest: SearchFormRequest; // Example property to hold search request data
  paginator: any; // Example property to hold paginator data
  readonly dialogRef = inject(MatDialogRef<SearchFileDialogComponent>);
  readonly data = inject<any>(MAT_DIALOG_DATA);
  // Example property to hold data passed to the dialog
  searchForm: FormGroup;
  collages: Collage[] = [];
  collageService: CollageService = inject(CollageService);
  funds: Fund[] = [];
  fundService: FundService = inject(FundService);
  fb = inject(FormBuilder);

  constructor() {
    // Initialize any properties if necessary
    this.searchRequest = this.data.searchRequest;
    this.paginator = this.data.paginator;
  }

  ngOnInit(): void {
    this.loadCollages();
    this.searchForm = this.initilizeForm();
  }

  loadCollages() {
    this.collageService.getCollages().subscribe(
      (collages: Collage[]) => {
        this.collages = collages;
      },
      (error) => {
        console.error('Error loading collages:', error);
      }
    );
  }
  loadFunds(ev) {

    // Logic to load funds based on selected collage
    if (!ev) {
      this.funds = [];
      this.searchRequest.collageId = null;

      return;
    }

    this.searchRequest.collageId = ev;
    this.searchRequest.fundId = null; // Reset FundId when CollageId changes
    this.searchForm.patchValue({ fundId: null }); // Reset form control value


    if (this.searchRequest.collageId) {
      this.fundService.getFundsByCollageId({ collageId: this.searchRequest.collageId }).subscribe(
        (funds: Fund[]) => {
          console.log('Funds loaded:', funds);

          this.funds = funds;

        },
        (error) => {
          console.error('Error loading funds:', error);
        }
      );
    } else {
      console.log('No collage selected, resetting funds.');
      this.funds = [];
    }
  }
  onSelectFund(ev) {
    console.log('Selected Fund:', ev);

    this.searchRequest.fundId = ev;
    this.searchForm.patchValue({ fundId: ev }); // Update form control value
  }
  onSelectDailyType(ev) {
    console.log('Selected Daily Type:', ev);
    this.searchRequest.dailyType = ev;
    this.searchForm.patchValue({ dailyType: ev }); // Update form control value
  }
  initilizeForm() {
    return this.fb.group({
      formName: [this.searchRequest.formName],
      formNum224: [this.searchRequest.formNum224],
      formNum55: [this.searchRequest.formNum55],
      collageId: [this.searchRequest.collageId],
      fundId: [this.searchRequest.fundId],
      dailyType: [this.searchRequest.dailyType],
      auditorName: [this.searchRequest.auditorName],
      startFrom: [this.searchRequest.startFrom],
      entryType: [this.searchRequest.entryType],

      endTo: [this.searchRequest.endTo]
    });


  }
  clear() {
    this.searchRequest = new SearchFormRequest();
    this.searchForm.reset();
  }
  onSubmit() {
    console.log('Form submitted:', this.searchForm.value);
    this.searchRequest = { ...this.searchRequest, ...this.searchForm.value };
    console.log('Updated Search Request:', this.searchRequest);

    // Update searchRequest with form values
    // Logic to handle form submission can be implemented here
    this.dialogRef.close(this.searchRequest);
  }
  onNoClick(): void {
    this.dialogRef.close();
  }

}
