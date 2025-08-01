import { Component, inject, OnInit, signal } from '@angular/core';
import { LayoutService } from '../../../shared/services/layout.service';
import { PaginatorModel } from '../../../shared/_models/paginator.model';
import { GetFormRequest } from '../../../shared/_requests/getFormRequest';
import { ActivatedRoute } from '@angular/router';
import { FormService } from '../../../shared/services/form.service';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { DeleteDialogComponent } from '../../../shared/components/dialog/delete-dialog/delete-dialog.component';
import { AddFormComponent } from '../../form/add-form/add-form.component';
import { FormResponse } from '../../form/form-response.interface';
import { SubsidarySearchDialogComponent } from './subsidary-search-dialog/subsidary-search-dialog.component';
import { SubsidiaryService } from '../../../shared/services/subsidiary.service';
import { Collage } from '../../../shared/_models/collage.model';
import { CollageService } from '../../../shared/services/collage.service';
import { GetSubsidiaryFormsByDailyIdRequest, SubsidaryToExcelRequest } from '../../../shared/_requests/getSubsidiaryFormsByDailyIdRequest';
import { Fund } from '../../../shared/_models/fund.model';
import { FundService } from '../../../shared/services/fund.service';
import { AddSubsidaryFormDetailsDialogComponent } from './add-subsidary-form-details-dialog/add-subsidary-form-details-dialog.component';
import { SubaccountService } from '../../../shared/services/subaccount.service';
import { UploadExcelFormDialogComponent } from '../../form/upload-excel-form-dialog/upload-excel-form-dialog.component';
import { UploadSubsidiaryComponent } from './upload-subsidiary/upload-subsidiary.component';

@Component({
  selector: 'app-subsidary-daily',
  standalone: false,
  templateUrl: './subsidary-daily.component.html',
  styleUrl: './subsidary-daily.component.scss'
})
export class SubsidaryDailyComponent implements OnInit {
  router = inject(ActivatedRoute);
  subsidaryService = inject(SubsidiaryService);
  subAccountService = inject(SubaccountService);
  collageService = inject(CollageService);
  fundService = inject(FundService);
  readonly dialog = inject(MatDialog);
  layoutService = inject(LayoutService);
  params = new GetSubsidiaryFormsByDailyIdRequest();
  exportParam = new SubsidaryToExcelRequest();
  readonly panelOpenState = signal(false);

  displayedColumns: string[] = ['action', 'num224', 'num55', 'formName', 'collageName', 'fundName', 'totalDebit', 'totalCredit', 'SubsidaryTotalDebit', 'SubsidaryTotalCredit', 'isBalanced'];

  // متغيرات للتصميم المتجاوب
  isHandset = false;
  responsiveDisplayedColumns: string[] = [];
  layoutClass = '';
  dataSource: any[] = [];
  originalDataSource: any[] = [];
  lastSearchedDataSource: any[] = []; // Track last searched data
  dailyId;
  subsidaryId;
  data: any;
  collages: Collage[] = []
  funds: Fund[] = [];
  filterdFunds: Fund[] = [];
  hasActiveSearch = false;

  paginator: PaginatorModel = new PaginatorModel();
  constructor() {
    this.router.params.subscribe(params => {
      this.dailyId = params['dailyId'];
      this.subsidaryId = params['subAccountId'];
    });

  }
  ngOnInit(): void {
    this.loadCollages();
    this.loadFunds();

    this.params.dailyId = this.dailyId;
    this.loadForms(this.params);

    // إعداد التصميم المتجاوب
    this.setupResponsiveLayout();
  }

  loadForms(param: GetSubsidiaryFormsByDailyIdRequest) {
    this.subsidaryService.GetSubsidaryDailyFormsByDailyId(this.subsidaryId, this.dailyId, this.params).subscribe({
      next: (response: any) => {


        this.dataSource = response.items;

        this.originalDataSource = [...response.items];


        this.data = response.items;
        this.paginator.length = response.totalCount;
      },
      error: (error) => {
        // Handle error
        console.error('Error loading forms', error);
      }
    });
  }

  loadCollages() {
    this.collageService.getCollages().subscribe({
      next: (response: Collage[]) => {

        this.collages = response;
      },
      error: (error) => {
        // Handle error
        console.error('Error loading collages', error);
      }
    })
  }
  loadFunds() {
    this.fundService.getFundsByCollageId({}).subscribe({
      next: (response: Fund[]) => {
        this.funds = response;
        this.filterdFunds = response;
      },
      error: (error) => {
        // Handle error
        console.error('Error loading funds', error);
      }
    })
  }
  getCollageById(collageId) {

    return this.collages.find(x => x.id == collageId).collageName;
  }
  getFundById(fundId) {

    //check befor return
    return this.funds.find(x => x.id == fundId)?.fundName;
  }





  handlePageEvent(e: PageEvent) {
    this.paginator.pageEvent = e;
    this.paginator.length = e.length;
    this.params.pageSize = e.pageSize;
    this.params.pageIndex = e.pageIndex;
    this.loadForms(this.params);
  }

  onCollageIdChange(collageId) {
    console.log(collageId);
    this.filterdFunds = [];
    this.params.collageId = collageId;

    if (collageId !== 0) {
      this.filterdFunds = this.funds.filter(x => x.collageId == collageId);

    }
    else {
      this.filterdFunds = []
    }

  }
  onFundIdChange(fundId) {
    this.params.fundId = fundId;
    //this.filterdFunds = this.funds.filter(x => x.collageId == this.params.CollageId);

  }

  openAddSubsidaryDailyDialog(element) {

    const dialogRef = this.dialog.open(AddSubsidaryFormDetailsDialogComponent, {
      data: {

        element: element,
        accountId: this.subsidaryId
      },
      disableClose: true,
      hasBackdrop: true,
      minWidth: '60vw',
      maxHeight: '90vh'
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadForms(this.params);

    });
  }
  openSearchDialog() {
    // تحضير معلمات البحث الحالية
    const currentParams = {
      formName: this.params.formName || '',
      num224: this.params.num224 || '',
      num55: this.params.num55 || '',
      collageId: this.params.collageId || 0,
      fundId: this.params.fundId || 0,
      auditorName: this.params.auditorName || '',
      isBalanced: this.params.isBalanced || ''
    };

    const dialogRef = this.dialog.open(SubsidarySearchDialogComponent, {
      width: '700px',
      disableClose: false,
      data: { searchParams: currentParams },
      direction: 'rtl'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.applied) {
        const searchParams = result.searchParams;

        // تحديث معلمات البحث
        this.params.formName = searchParams.formName;
        this.params.num224 = searchParams.num224;
        this.params.num55 = searchParams.num55;
        this.params.collageId = searchParams.collageId;
        this.params.fundId = searchParams.fundId;
        this.params.auditorName = searchParams.auditorName;
        this.params.isBalanced = searchParams.isBalanced;

        // التحقق مما إذا كانت هناك معايير بحث نشطة
        this.hasActiveSearch = !!(searchParams.formName || searchParams.num224 ||
          searchParams.num55 || (searchParams.collageId && searchParams.collageId > 0) ||
          (searchParams.fundId && searchParams.fundId > 0) ||
          searchParams.auditorName || searchParams.isBalanced !== '');

        // تحميل البيانات بناءً على معايير البحث الجديدة
        this.loadForms(this.params);
      }
    });
  }
  onPrint() {
    this.params.dailyId = this.dailyId;
    this.params.accountId = this.subsidaryId;
    this.subsidaryService.downloadSubsidaryDailyPdf(this.params).subscribe((response: any) => {
      const blob = new Blob([response], { type: 'application/pdf' });
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    });

  }


  deleteSubsidary(element) {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: {
        message: 'هل تريد حذف هذا النموذج؟',
        title: 'حذف نموذج'
      },
      disableClose: true,
      hasBackdrop: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.subsidaryService.deleteSubsidaryDailyForm(element.formDetailsId).subscribe({
          next: () => {
            this.loadForms(this.params);
          },
          error: (error) => {
            console.error('Error deleting form', error);
          }
        });
      }
    });



  }

  /**
   * إعداد التخطيط المتجاوب للجدول والواجهة
   * يقوم بضبط أعمدة الجدول بناءً على حجم الشاشة
   */
  setupResponsiveLayout() {
    // مراقبة حجم الشاشة وتعديل عرض الأعمدة وفقًا لذلك
    this.layoutService.isHandset$.subscribe(isHandset => {
      this.isHandset = isHandset;
      this.updateDisplayColumns(isHandset);
    });

    // الحصول على فئة CSS مناسبة بناءً على حجم الشاشة
    this.layoutService.getResponsiveClass().subscribe(className => {
      this.layoutClass = className;
    });
  }

  /**
   * تحديث الأعمدة المعروضة بناءً على حجم الشاشة
   */
  updateDisplayColumns(isHandset: boolean) {
    if (isHandset) {
      // للشاشات الصغيرة، نعرض عدد أقل من الأعمدة
      this.responsiveDisplayedColumns = ['action', 'num224', 'formName', 'totalDebit', 'totalCredit', 'isBalanced'];
    } else {
      // للشاشات الكبيرة، نعرض كل الأعمدة
      this.responsiveDisplayedColumns = this.displayedColumns;
    }
  }
  onExport() {
    this.exportParam.dailyId = this.dailyId;
    this.exportParam.accountId = this.subsidaryId;
    this.subsidaryService.exportSubsidaryDailyExcel(this.exportParam).subscribe((response: any) => {
      const blob = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      const url = window.URL.createObjectURL(blob);
      window.open(url);
    });
  }
  onUploadExcel() {
    //open upload dialog
    const dialogRef = this.dialog.open(UploadSubsidiaryComponent, {
      width: '60%',
      disableClose: false,
      data: {
        dailyId: this.dailyId,
        accountId: this.subsidaryId

      }
    });
    dialogRef.afterClosed().subscribe(result => {
      this.loadForms(this.params);
    });

  }
}

