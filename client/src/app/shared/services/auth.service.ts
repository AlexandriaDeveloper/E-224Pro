import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
export interface User {
    id: string;
    username: string;
    email: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
    private apiUrl = environment.apiUrl + 'auth'; // Change to your API endpoint
    //get current logged user using signal to update the UI
    // This can be done using a BehaviorSubject or similar approach if needed
    private currentUserSubject: BehaviorSubject<User | null> = new BehaviorSubject<User | null>(null);
    currentUser$ = this.currentUserSubject.asObservable();

    setCurrentUser(user: User | null) {
        this.currentUserSubject.next(user);
    }

    getCurrentUser(): User | null {
        return this.currentUserSubject.value;
    }



    constructor(private http: HttpClient, private router: Router) { }

    login(username: string, password: string): Observable<{ token: string }> {
        return this.http.post<{ token: string }>(`${this.apiUrl}/login`, { username, password }).pipe(
            //tap(res => this.saveToken(res.token)),
            tap((res: any) => {
                this.saveToken(res.token);
                this.setCurrentUser({ id: res.id, username: res.displayName, email: res.email });
                this.router.navigate(['/']);
            })
        );

    }

    saveToken(token: string) {
        localStorage.setItem('token', token);
    }

    getToken() {
        return localStorage.getItem('token');
    }

    logout() {
        localStorage.removeItem('token');
        this.router.navigate(['/']);
    }

    isLoggedIn() {
        return !!this.getToken();
    }

    getUserRoles(): string[] {
        const token = this.getToken();
        if (!token) return [];
        const payload = JSON.parse(atob(token.split('.')[1]));
        return payload['role'] ? [payload['role']] : payload['roles'] || [];
    }
}
