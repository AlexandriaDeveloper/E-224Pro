import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-upload-subsidiary',
    templateUrl: './upload-subsidiary.component.html',
    styleUrls: ['./upload-subsidiary.component.scss'],
    standalone: false
})
export class UploadSubsidiaryComponent {
    accountId: number | null = null;
    dailyId: number | null = null;
    file: File | null = null;
    uploading = false;
    message = '';

    constructor(private http: HttpClient) { }

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
        const formData = new FormData();
        formData.append('file', this.file);
        formData.append('accountId', this.accountId.toString());
        formData.append('dailyId', this.dailyId.toString());

        this.http.post('/SubsidiaryJournal/UploadSubsidiaryDailyToExcel', formData)
            .subscribe({
                next: () => {
                    this.message = 'تم رفع الملف بنجاح!';
                    this.file = null;
                },
                error: () => {
                    this.message = 'حدث خطأ أثناء رفع الملف!';
                },
                complete: () => {
                    this.uploading = false;
                }
            });
    }

    onClose() {
        // If using MatDialogRef, inject and call close()
        // Otherwise, emit event or hide dialog as needed
        // Placeholder for dialog close logic
    }
}
