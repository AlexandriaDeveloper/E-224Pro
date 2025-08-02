import { Component, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { RoleService } from '../../../shared/services/role.service';

@Component({
    selector: 'app-register',
    standalone: false,
    templateUrl: './register.component.html',
    styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
    username = '';
    email = '';
    password = '';
    selectedRole = '';
    error = '';
    success = '';

    roles = [];
    roleService = inject(RoleService);

    loadRoles() {
        this.roleService.getRoles().subscribe({
            next: (roles) => {
                console.log('Roles loaded:', roles);
                this.roles = roles;
            },
            error: (err) => {
                console.error('Error loading roles:', err);
            }
        });
    }
    ngOnInit() {
        this.loadRoles();
    }
    constructor(private http: HttpClient, private router: Router) { }

    register() {
        this.http.post(environment.apiUrl + 'auth/register', {
            username: this.username,
            email: this.email,
            password: this.password,
            role: this.selectedRole
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
    onRoleChange(event: any) {
        console.log('Selected role:', event.target.value);
        this.selectedRole = event.target.value;
        // Handle role change logic here if needed
    }
}
