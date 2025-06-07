import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormSearchDialogComponent } from './form-search-dialog.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

describe('FormSearchDialogComponent', () => {
  let component: FormSearchDialogComponent;
  let fixture: ComponentFixture<FormSearchDialogComponent>;
  const mockDialogRef = {
    close: jasmine.createSpy('close')
  };
  const mockData = {
    formData: [
      { 
        id: 1, 
        num224: '224-001', 
        num55: '55-001', 
        formName: 'Test Form', 
        collageName: 'Test College',
        fundName: 'Test Fund',
        totalCredit: 1000,
        totalDebit: 1000
      },
      { 
        id: 2, 
        num224: '224-002', 
        num55: '55-002', 
        formName: 'Another Form', 
        collageName: 'Another College',
        fundName: 'Another Fund',
        totalCredit: 2000,
        totalDebit: 1500
      }
    ]
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        FormSearchDialogComponent,
        ReactiveFormsModule,
        NoopAnimationsModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule
      ],
      providers: [
        { provide: MatDialogRef, useValue: mockDialogRef },
        { provide: MAT_DIALOG_DATA, useValue: mockData }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(FormSearchDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should filter data by form name', () => {
    component.searchForm.patchValue({ formName: 'Test' });
    component.onSearch();
    expect(mockDialogRef.close).toHaveBeenCalledWith([mockData.formData[0]]);
  });

  it('should filter data by college name', () => {
    component.searchForm.patchValue({ collageName: 'Another' });
    component.onSearch();
    expect(mockDialogRef.close).toHaveBeenCalledWith([mockData.formData[1]]);
  });

  it('should return all data when cleared', () => {
    component.onClear();
    expect(mockDialogRef.close).toHaveBeenCalledWith(mockData.formData);
  });
});
