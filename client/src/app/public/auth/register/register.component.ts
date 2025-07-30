import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environment';

@Component({
    selector: 'app-register',
    standalone: false,
    templateUrl: './register.component.html',
    styleUrl: './register.component.scss'
})
export class RegisterComponent {
    username = '';
    email = '';
    password = '';
    error = '';
    success = '';

    constructor(private http: HttpClient, private router: Router) { }

    register() {
        this.http.post(environment.apiUrl + 'auth/register', {
            username: this.username,
            email: this.email,
            password: this.password
        }).subscribe({
            next: () => {
                this.success = 'Registration successful! Please login.';
                this.error = '';
            },
            error: () => {
                this.error = 'Registration failed. Try again.';
                this.success = '';
            }
        });
    }
}
