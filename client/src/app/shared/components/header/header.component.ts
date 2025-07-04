import { Component, EventEmitter, inject, Input, input, OnInit, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-header',
  imports: [
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatIconModule,
    MatToolbarModule,
    MatMenuModule,
    MatSlideToggle,
    CommonModule,
    RouterModule,
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements OnInit {
  ngOnInit(): void {
    this.isDarkMode = localStorage.getItem('isDarkMode') === 'true';
    //auth service


    if (this.isDarkMode === false) {
      localStorage.setItem('isDarkMode', 'false');

      document.body.classList.remove('dark-mode');
    }
    else {
      document.body.classList.add('dark-mode');

    }
    //get username if user is logged in
    // if (this.authService.isLoggedIn()) {
    //   this.authService.currentUser$.subscribe(user => {
    //     if (user) {
    //       console.log('user from auth service:', user);
    //       console.log(this.authService.getUserRoles());
    //       this.username = user.username;
    //     }
    //   });
    // } else {
    //   this.username = '';
    // }
  }
  constructor(public authService: AuthService) {



  }

  @Input() username: string = ''; // اسم المستخدم من المكون الأب

  @Output() toggleSidenav = new EventEmitter<void>();








  logout() {
    console.log('تسجيل الخروج');
    this.authService.logout();
    this.username = ''; // إعادة تعيين اسم المستخدم عند تسجيل الخروج
    // أضف منطق تسجيل الخروج هنا
  }
  toggleSidenavEmitter() {
    this.toggleSidenav.emit();
  }

  isDarkMode;

  toggleTheme() {
    this.isDarkMode = !this.isDarkMode;
    localStorage.setItem('isDarkMode', String(this.isDarkMode));
    document.body.classList.toggle('dark-mode', this.isDarkMode);

  }

}
