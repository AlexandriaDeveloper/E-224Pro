
import { Component } from '@angular/core';


@Component({
  selector: 'app-public-home',
  standalone: false,

  templateUrl: './public-home.component.html',
  styleUrl: './public-home.component.scss'
})
export class PublicHomeComponent {

  isSidenavOpened: boolean; // حالة القائمة الجانبية

  toggleSidenav() {

    this.isSidenavOpened = !this.isSidenavOpened; // تبديل حالة القائمة
  }
  darkMode(ev) {
    console.log(ev); //
  }
}