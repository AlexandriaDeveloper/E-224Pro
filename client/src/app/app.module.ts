import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { AuthErrorInterceptor } from './shared/interceptors/auth-error.interceptor';

const routes: Routes = [
    // ...define your routes here
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes),

    ],
    exports: [RouterModule],
    providers: [
        //  { provide: HTTP_INTERCEPTORS, useClass: AuthErrorInterceptor, multi: true },
    ]
})
export class AppRoutingModule { }