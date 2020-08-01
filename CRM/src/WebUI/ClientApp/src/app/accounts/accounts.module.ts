import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountsRoutingModule } from './accounts-routing.module';
import { AccountsComponent } from './accounts.component';
import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { AccountDetailComponent } from './account-detail/account-detail.component';
import { AccountEditComponent } from './account-edit/account-edit.component';
import { AccountCreateComponent } from './account-create/account-create.component';
import { StoreModule } from '@ngrx/store';
import * as fromAccounts from './store/reducers/accounts.reducer';
import { EffectsModule } from '@ngrx/effects';
import { AccountsEffects } from './store/effects/accounts.effects';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AccountsComponent,
    AccountsListComponent,
    AccountDetailComponent,
    AccountEditComponent,
    AccountCreateComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AccountsRoutingModule,
    StoreModule.forFeature(fromAccounts.accountsFeatureKey, fromAccounts.reducer),
    EffectsModule.forFeature([AccountsEffects]),
  ],
})
export class AccountsModule {}
