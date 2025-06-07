import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { DragDropModule } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-form-search-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    DragDropModule
  ],
  templateUrl: './form-search-dialog.component.html',
  styleUrls: ['./form-search-dialog.component.scss']
})
export class FormSearchDialogComponent {
  searchForm: FormGroup;
  originalData: any[];
  currentData: any[];
  searchResults: any[] = [];

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<FormSearchDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { 
      formData: any[], 
      currentData?: any[], 
      previousSearchValues?: any 
    }
  ) {
    this.originalData = [...data.formData];
    this.currentData = data.currentData || this.originalData;
    this.searchForm = this.fb.group({
      id: [''],
      num224: [''],
      num55: [''],
      formName: [''],
      collageName: [''],
      fundName: [''],
      totalCredit: [''],
      totalDebit: ['']
    });

    // Pre-fill form with previous search values if available
    if (data.previousSearchValues && data.previousSearchValues.searchValues) {
      this.searchForm.patchValue(data.previousSearchValues.searchValues);
    } 
    // Fallback: Pre-fill form with current data if it's different from original
    else if (this.currentData !== this.originalData) {
      this.prefillSearchForm(this.currentData[0]);
    }
  }

  // Helper method to prefill search form with first item's data
  private prefillSearchForm(item: any) {
    if (!item) return;

    Object.keys(this.searchForm.controls).forEach(key => {
      if (item[key]) {
        this.searchForm.get(key).setValue(item[key]);
      }
    });
  }

  onSearch() {
    const searchValues = this.searchForm.value;
    this.searchResults = this.originalData.filter(item => {
      // Filter out empty search criteria
      const activeSearchKeys = Object.keys(searchValues)
        .filter(key => searchValues[key] !== null && searchValues[key] !== '');

      // If no search criteria, return all items
      if (activeSearchKeys.length === 0) return true;

      // Check each active search key
      return activeSearchKeys.every(key => {
        const searchValue = String(searchValues[key]).toLowerCase().trim();
        const itemValue = String(item[key]).toLowerCase().trim();

        // Specific handling for different types of fields
        switch (key) {
          case 'totalCredit':
          case 'totalDebit':
            // Numeric field: support partial and exact matches
            return itemValue.includes(searchValue) || 
                   Number(itemValue) === Number(searchValue);
          
          case 'id':
          case 'num224':
          case 'num55':
            // Exact match for ID-like fields
            return itemValue === searchValue;
          
          default:
            // Text fields: partial, case-insensitive match
            return itemValue.includes(searchValue);
        }
      });
    });

    // Close dialog with search results and current form values
    this.dialogRef.close({
      results: this.searchResults,
      searchValues: searchValues
    });
  }

  onClear() {
    // Reset form
    this.searchResults = [];
    this.searchForm.reset();

    // Close dialog with original data
    this.dialogRef.close(this.originalData);
  }
}
