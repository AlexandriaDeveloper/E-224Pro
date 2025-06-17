import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PublicHomeComponent } from './public-home.component';
import { HomeComponent } from './home/home.component';
import { AddDailyComponent } from './dailies/add-daily/add-daily.component';
import { DailiesComponent } from './dailies/dailies.component';

import { FormComponent } from './form/form.component';
import { FundHomeComponent } from './fund-home/fund-home.component';
import { SubsidaryDailiesComponent } from './subsidary-dailies/subsidary-dailies.component';
import { SubsidaryDailyComponent } from './subsidary-dailies/subsidary-daily/subsidary-daily.component';


const routes: Routes = [
  {
    path: '',
    component: PublicHomeComponent,
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'dailies', component: DailiesComponent },
      { path: 'dailies/add', component: AddDailyComponent },
      //path with param 
      { path: 'dailies/:id', component: FormComponent },
      { path: 'funds', component: FundHomeComponent },
      { path: 'subsidary-dailies/subId/:subaccountId', component: SubsidaryDailiesComponent },
      { path: 'subsidary-dailies/subId/:subAccountId/dailyId/:dailyId', component: SubsidaryDailyComponent },
      { path: '', redirectTo: 'public/home', pathMatch: 'full' }


    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule { }
