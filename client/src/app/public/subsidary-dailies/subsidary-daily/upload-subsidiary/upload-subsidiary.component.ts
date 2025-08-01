import { Component, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SubsidiaryService } from '../../../../shared/services/subsidiary.service';

@Component({
    selector: 'app-upload-subsidiary',
    templateUrl: './upload-subsidiary.component.html',
    styleUrls: ['./upload-subsidiary.component.scss'],
    standalone: false
})
export class UploadSubsidiaryComponent implements OnInit {
    accountId: number | null = null;
    dailyId: number | null = null;
    file: File | null = null;
    uploading = false;
    message = '';
    public readonly data = inject<any>(MAT_DIALOG_DATA);
    private readonly dialogRef = inject(MatDialogRef<UploadSubsidiaryComponent>);
    subsidaryService = inject(SubsidiaryService);
    constructor(private http: HttpClient) { }
    ngOnInit(): void {
        this.accountId = this.data.accountId;
        this.dailyId = this.data.dailyId;
    }

    onFileChange(event: any) {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.file = input.files[0];
            this.message = '';
        }
    }

    upload() {
        
        if (!this.file || !this.accountId || !this.dailyId) {
            this.message = 'يرجى اختيار الملف وادخال رقم الحساب واليومي';
            return;
        }
        this.uploading = true;
        this.message = '';

        this.subsidaryService.uploadSubsidaryDailyFormAsExcel(this.dailyId, this.accountId, this.file).subscribe({
            next: (response) => {
                this.uploading = false;
                this.message = 'تم تحميل الملف بنجاح';
                this.dialogRef.close();
            },
            error: (error) => {
                this.uploading = false;
                this.message = 'حدث خطأ أثناء تحميل الملف';
            },
            complete: () => {
                this.uploading = false;
            }

        })
    }

    onClose() {

        this.dialogRef.close();

    }
}
