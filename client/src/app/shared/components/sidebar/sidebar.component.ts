import { Component, inject, Input, OnInit } from '@angular/core';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { Observable } from 'rxjs/internal/Observable';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { LayoutService } from '../../services/layout.service';
import { MatExpansionModule } from '@angular/material/expansion'
import { RouterModule } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';



@Component({
  selector: '[app-sidebar]',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatExpansionModule,
    RouterModule,
    CommonModule

  ]
})

export class SidebarComponent implements OnInit {

  @Input("isOpened") isOpened: boolean = false; // حالة القائمة الجانبية من المكون الأب
  subAccountsService = inject(AccountService);
  public auth = inject(AuthService);
  layoutService = inject(LayoutService);
  isHandset$: Observable<boolean>;
  subAccounts: any = [];
  
  // متغيرات للتصميم المتجاوب
  isCompactView = false;
  layoutClass = '';

  constructor(
    private breakpointObserver: BreakpointObserver,

  ) {
    // استخدام خدمة LayoutService للحصول على حالة الشاشة
    this.isHandset$ = this.layoutService.isHandset$;
    // if (!this.auth.isAuthenticated()) {
    //   //navigate to login page
    //   this.router.navigate(['/account/login']);
    // }

  }
  ngOnInit(): void {
    // إعداد التصميم المتجاوب
    this.setupResponsiveLayout();
    
    console.log(this.auth.getUserAccounRoles());
    this.subAccountsService.getAccountsHasSubAccounts().subscribe({
      next: (res: []) => {
        console.log(res);
        //add to subAccounts only if res contain ids in auth.getUserAccounRoles
        this.subAccounts = res.filter((account: any) => this.auth.getUserAccounRoles().includes(account.id.toString()));
        console.log(this.subAccounts);
      },
      error: (err) => {
        console.log(err);

      }
    })
  }

  logout() {

    console.log('تسجيل الخروج');
  }

  /**
   * إعداد التخطيط المتجاوب للقائمة الجانبية
   */
  setupResponsiveLayout() {
    // مراقبة حجم الشاشة لتعديل عرض القائمة الجانبية
    this.layoutService.isHandset$.subscribe(isHandset => {
      this.isCompactView = isHandset;
    });
    
    // الحصول على فئة CSS مناسبة بناءً على حجم الشاشة
    this.layoutService.getResponsiveClass().subscribe(className => {
      this.layoutClass = className;
    });
  }
}
