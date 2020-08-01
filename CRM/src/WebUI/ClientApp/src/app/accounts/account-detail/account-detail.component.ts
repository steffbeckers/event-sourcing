import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectAccountsState, selectEntityById } from '../store/selectors/accounts.selectors';
import { ActivatedRoute } from '@angular/router';
import { AccountDto2 } from 'src/app/crm-api';
import { Observable } from 'rxjs';
import { loadAccount, deactivateAccount, activateAccount } from '../store/actions/accounts.actions';

@Component({
  selector: 'app-account-detail',
  templateUrl: './account-detail.component.html',
  styleUrls: ['./account-detail.component.css'],
})
export class AccountDetailComponent implements OnInit {
  accountsState$ = this.store.select(selectAccountsState);
  account$: Observable<AccountDto2>;
  account: AccountDto2;

  constructor(private store: Store, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      if (params.id) {
        const id = params.id;
        this.account$ = this.store.select(selectEntityById(id));
        this.account$.subscribe((account) => {
          this.account = account;
        });
        this.store.dispatch(loadAccount({ id }));
      }
    });
  }

  toggleActiveState(): void {
    if (this.account.isActive) {
      this.store.dispatch(deactivateAccount({ accountId: this.account.id }));
    } else {
      this.store.dispatch(activateAccount({ accountId: this.account.id }));
    }
  }
}
