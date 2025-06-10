import { Component, inject, Input, OnInit } from '@angular/core';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { Observable } from 'rxjs/internal/Observable';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { MatExpansionModule } from '@angular/material/expansion'
import { RouterModule } from '@angular/router';
import { AccountService } from '../../services/account.service';



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



  ]
})

export class SidebarComponent implements OnInit {

  @Input("isOpened") isOpened: boolean = false; // حالة القائمة الجانبية من المكون الأب
  subAccountsService = inject(AccountService)
  username: string = '';
  isHandset$: Observable<boolean>
  subAccounts: any = [];

  constructor(
    private breakpointObserver: BreakpointObserver,

  ) {
    // if (!this.auth.isAuthenticated()) {
    //   //navigate to login page
    //   this.router.navigate(['/account/login']);
    // }

  }
  ngOnInit(): void {
    // this.isHandset$ == this.breakpointObserver.observe(Breakpoints.Handset)
    //   .pipe(
    //     map(result => result.matches),
    //     shareReplay()
    //   );
    this.subAccountsService.getAccountsHasSubAccounts().subscribe({
      next: (res) => {
        console.log(res);

        this.subAccounts = res;
      }
    })
  }

  logout() {

    console.log('تسجيل الخروج');
  }


}
