import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountsRoutingModule } from './accounts-routing.module';
import { AccountsComponent } from './accounts.component';
import { AccountsListComponent } from './accounts-list/accounts-list.component';
import { AccountDetailComponent } from './account-detail/account-detail.component';
import { AccountEditComponent } from './account-edit/account-edit.component';

@NgModule({
  declarations: [AccountsComponent, AccountsListComponent, AccountDetailComponent, AccountEditComponent],
  imports: [CommonModule, AccountsRoutingModule],
})
export class AccountsModule {}
