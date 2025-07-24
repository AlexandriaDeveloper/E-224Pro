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
import { ToasterService } from '../services/toaster.service';


export function AuthErrorInterceptor(req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> {
    const authService: AuthService = inject(AuthService);
    const toasterService: ToasterService = inject(ToasterService);

    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === 401) {
                // Unauthorized: log out and redirect to login
                //  authService.logout();
                toasterService.error('ليس لديك صلاحية للوصول إلى هذا المورد.', 'خطأ في المصادقة');
            } else if (error.status === 403) {
                // Forbidden: show a message or redirect
                toasterService.error('ليس لديك صلاحية للوصول إلى هذا المورد.', 'خطأ في الصلاحيات');
            } else if (error.status === 0) {
                toasterService.error('تعذر الاتصال بالخادم. تحقق من الاتصال بالإنترنت.', 'خطأ في الاتصال');
            } else if (error.error && error.error.message) {
                toasterService.error(error.error.message, 'خطأ');
            } else {
                toasterService.error('حدث خطأ غير متوقع', 'خطأ');
            }
            return throwError(() => error);
        })
    );
}

