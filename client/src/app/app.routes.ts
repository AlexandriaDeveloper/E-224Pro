import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: 'public',
        loadChildren: () => import('./public/public.module').then(m => m.PublicModule)

    },
    // {
    //     path: 'auth',
    //     loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
    // },

    {
        path: '',
        redirectTo: '/public',
        pathMatch: 'full'
    },
    {
        path: '**',
        redirectTo: '/public',
        pathMatch: 'full'
    }

];
