import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as AccountsActions from '../store/actions/accounts.actions';
import { selectAccountsState, selectAllSortedByName } from '../store/selectors/accounts.selectors';

@Component({
  selector: 'app-accounts-list',
  templateUrl: './accounts-list.component.html',
  styleUrls: ['./accounts-list.component.css'],
})
export class AccountsListComponent implements OnInit {
  accountsState$ = this.store.select(selectAccountsState);
  accounts$ = this.store.select(selectAllSortedByName);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.loadAccounts();
  }

  loadAccounts(): void {
    this.store.dispatch(AccountsActions.loadAccounts());
  }
}
