import { Routes } from '@angular/router';
import { ToasterExampleComponent } from './shared/components/toaster-example.component';

export const routes: Routes = [
    {
        path: '',
        loadChildren: () => import('./public/public.module').then(m => m.PublicModule)
    },
    // {
    //     path: 'auth',
    //     loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
    // },
    {
        path: 'toast-example',
        component: ToasterExampleComponent
    },
    {
        path: '',
        redirectTo: '/',
        pathMatch: 'full'
    }
];
