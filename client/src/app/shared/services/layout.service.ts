import { Injectable } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {
  // استعلامات أساسية للأجهزة المختلفة
  isHandset$: Observable<boolean>;
  isTablet$: Observable<boolean>;
  isWeb$: Observable<boolean>;

  // استعلامات إضافية مفيدة
  isSmallScreen$: Observable<boolean>;
  isMediumScreen$: Observable<boolean>;
  isLargeScreen$: Observable<boolean>;

  constructor(private breakpointObserver: BreakpointObserver) {
    // استعلامات الأجهزة
    this.isHandset$ = this.breakpointObserver.observe(Breakpoints.Handset)
      .pipe(
        map(result => result.matches),
        shareReplay()
      );

    this.isTablet$ = this.breakpointObserver.observe(Breakpoints.Tablet)
      .pipe(
        map(result => result.matches),
        shareReplay()
      );

    this.isWeb$ = this.breakpointObserver.observe([
      Breakpoints.Web,
      Breakpoints.WebLandscape
    ]).pipe(
      map(result => result.matches),
      shareReplay()
    );

    // استعلامات أحجام الشاشة
    this.isSmallScreen$ = this.breakpointObserver.observe([
      Breakpoints.XSmall,
      Breakpoints.Small
    ]).pipe(
      map(result => result.matches),
      shareReplay()
    );

    this.isMediumScreen$ = this.breakpointObserver.observe(Breakpoints.Medium)
      .pipe(
        map(result => result.matches),
        shareReplay()
      );

    this.isLargeScreen$ = this.breakpointObserver.observe([
      Breakpoints.Large,
      Breakpoints.XLarge
    ]).pipe(
      map(result => result.matches),
      shareReplay()
    );
  }

  /**
   * الحصول على كلاس CSS بناءً على حجم الشاشة الحالي
   * مفيد للتطبيق في قوالب HTML
   */
  getResponsiveClass(): Observable<string> {
    return this.breakpointObserver.observe([
      Breakpoints.XSmall,
      Breakpoints.Small,
      Breakpoints.Medium,
      Breakpoints.Large,
      Breakpoints.XLarge
    ]).pipe(
      map(result => {
        if (result.breakpoints[Breakpoints.XSmall]) {
          return 'xs-layout';
        } else if (result.breakpoints[Breakpoints.Small]) {
          return 'sm-layout';
        } else if (result.breakpoints[Breakpoints.Medium]) {
          return 'md-layout';
        } else if (result.breakpoints[Breakpoints.Large]) {
          return 'lg-layout';
        } else if (result.breakpoints[Breakpoints.XLarge]) {
          return 'xl-layout';
        }
        return '';
      }),
      shareReplay()
    );
  }

  /**
   * التحقق من اتجاه الشاشة (أفقي/عمودي)
   */
  isPortrait(): Observable<boolean> {
    return this.breakpointObserver.observe('(orientation: portrait)')
      .pipe(
        map(result => result.matches),
        shareReplay()
      );
  }

  /**
   * دالة مساعدة للحصول على عدد أعمدة مناسب لعرض العناصر بناءً على حجم الشاشة
   */
  getColumnCount(): Observable<number> {
    return this.breakpointObserver.observe([
      Breakpoints.XSmall,
      Breakpoints.Small,
      Breakpoints.Medium,
      Breakpoints.Large,
      Breakpoints.XLarge
    ]).pipe(
      map(result => {
        if (result.breakpoints[Breakpoints.XSmall]) {
          return 1; // عمود واحد للشاشات الصغيرة جدًا
        } else if (result.breakpoints[Breakpoints.Small]) {
          return 2; // عمودان للشاشات الصغيرة
        } else if (result.breakpoints[Breakpoints.Medium]) {
          return 3; // 3 أعمدة للشاشات المتوسطة
        } else if (result.breakpoints[Breakpoints.Large]) {
          return 4; // 4 أعمدة للشاشات الكبيرة
        } else {
          return 5; // 5 أعمدة للشاشات الكبيرة جدًا
        }
      }),
      shareReplay()
    );
  }
}
