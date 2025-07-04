import { inject, Injectable } from '@angular/core';
import {
    HttpEvent,
    HttpInterceptor,
    HttpHandler,
    HttpRequest,
    HttpErrorResponse,
    HttpHandlerFn
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';


export function AuthErrorInterceptor(req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> {

    let authService: AuthService = inject(AuthService);
    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === 401) {
                // Unauthorized: log out and redirect to login
                //  authService.logout();
                alert('ليس لديك صلاحية للوصول إلى هذا المورد.');
            } else if (error.status === 403) {
                // Forbidden: show a message or redirect
                alert('ليس لديك صلاحية للوصول إلى هذا المورد.');
            } else if (error.status === 0) {
                alert('تعذر الاتصال بالخادم. تحقق من الاتصال بالإنترنت.');
            } else if (error.error && error.error.message) {
                alert(error.error.message);
            }
            return throwError(() => error);
        })
    );
}

