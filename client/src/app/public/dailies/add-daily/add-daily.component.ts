import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { accountItems, dailyTypes } from '../../../shared/_models/constants';
import { DailiesService } from '../../../shared/services/dailies.service';
import { Daily } from '../../../shared/_models/Daily.model';
import { StringToDateOnlyProviderService } from '../../../shared/_helper/string-to-date-only-provider.service';
@Component({
  selector: 'app-add-daily',
  standalone: false,

  templateUrl: './add-daily.component.html',
  styleUrl: './add-daily.component.scss'
})
export class AddDailyComponent implements OnInit {

  dailyService = inject(DailiesService);
  fb = inject(FormBuilder);
  readonly data = inject<any>(MAT_DIALOG_DATA);
  dateService = inject(StringToDateOnlyProviderService);
  readonly dialogRef = inject(MatDialogRef<AddDailyComponent>);
  dailyTypes = dailyTypes
  accountItems = accountItems;
  addForm: FormGroup;
  daily = new Daily();
  isUpdateMode = false;

  ngOnInit(): void {
    console.log(this.data);

    if (this.data.daily !== null) {
      this.daily = this.data.daily;
      this.isUpdateMode = true;
    }
    console.log(this.daily.name);

    this.addForm = this.initialForm();
  }
  initialForm() {
    return this.fb.group({
      name: [this.daily.name, Validators.required],
      dailyType: [this.daily.dailyType, Validators.required],
      accountItem: [this.daily.accountItem, Validators.required],
      dailyDate: [this.daily.dailyDate, Validators.required]

    });
  }

  onNoClick(): void {

    this.dialogRef.close();

  }
  onSubmit() {
    this.addForm.value.dailyDate = this.dateService.stringToDateOnlyProvider(this.addForm.value.dailyDate);
    if (!this.isUpdateMode) {

      this.dailyService.addDaily(this.addForm.value).subscribe(() => this.dialogRef.close());
    }
    else {
      this.dailyService.updateDaily(this.daily.id, this.addForm.value).subscribe(() => this.dialogRef.close());
    }


  }
}
