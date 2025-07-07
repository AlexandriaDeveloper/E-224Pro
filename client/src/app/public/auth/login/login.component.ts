import { Component } from '@angular/core';
import { AuthService } from '../../../shared/services/auth.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-login',
    standalone: false,
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss'
})
export class LoginComponent {
    username = 'admin';
    password = 'Fr33tim3#';
    error = '';

    constructor(private authService: AuthService, private router: Router) { }

    login() {
        this.authService.login(this.username, this.password).subscribe({
            next: (res: any) => {
                console.log(res);

                this.authService.saveToken(res.token);
                this.authService.setCurrentUser({ id: res.id, username: res.displayName, email: res.email });
                this.router.navigate(['/']);
            },
            error: () => {
                this.error = 'Invalid username or password';
            }
        });
    }
}
