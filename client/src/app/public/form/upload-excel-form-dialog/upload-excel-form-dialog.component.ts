import { Component, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog'; // Import MatDialogRef
import { DailiesService } from '../../../shared/services/dailies.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-upload-excel-form-dialog',
  standalone: false,
  templateUrl: './upload-excel-form-dialog.component.html',
  styleUrl: './upload-excel-form-dialog.component.scss'
})
export class UploadExcelFormDialogComponent implements OnInit {
  // Inject HttpClient for making HTTP requests
  // Inject MatDialogRef to control the dialog behavior
  dailyService = inject(DailiesService); // Inject your service here
  data = inject<any>(MAT_DIALOG_DATA); // Inject data passed to the dialog
  selectedFile: File | null = null;
  fileName = '';
  uploading = false;
  uploadSuccess = false;
  uploadError = false;
  dailyId: number; // Example dailyId, replace with actual value as needed

  constructor(private http: HttpClient, private dialogRef: MatDialogRef<UploadExcelFormDialogComponent>) { } // Inject MatDialogRef
  ngOnInit(): void {
    //getDailyId from route or service if needed
    console.log(this.data);
    this.dailyId = Number(this.data.dailyId);

  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.fileName = this.selectedFile.name;
      this.uploadSuccess = false;
      this.uploadError = false;
    }
  }

  onUpload() {
    console.log('Uploading file:', this.selectedFile);

    if (!this.selectedFile) return;
    this.uploading = true;
    this.uploadSuccess = false;
    this.uploadError = false;

    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('fileName', this.fileName);


    // TODO: Replace with your actual API endpoint
    this.dailyService.uploadExcelForm(formData, this.dailyId).subscribe({
      next: () => {
        this.uploading = false;
        this.uploadSuccess = true;
        this.selectedFile = null;
        this.fileName = '';

        // Optionally close the dialog on success
        this.dialogRef.close(true);
      },
      error: () => {
        this.uploading = false;
        this.uploadError = true;
      }
    });
  }

  onNoClick(): void {
    this.dialogRef.close(); // Method to close the dialog
  }
}
