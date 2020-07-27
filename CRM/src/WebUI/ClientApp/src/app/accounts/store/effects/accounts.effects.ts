import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, switchMap, catchError, exhaustMap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as AccountsActions from '../actions/accounts.actions';
import { AccountsClient, AccountDto, AccountDto2 } from 'src/app/crm-api';

@Injectable()
export class AccountsEffects {
  constructor(private actions$: Actions, private accountsClient: AccountsClient) {}

  loadAccounts$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountsActions.loadAccounts),
      switchMap(() =>
        this.accountsClient.get().pipe(
          map((accounts: AccountDto[]) => AccountsActions.loadAccountsSuccess({ accounts })),
          catchError((error) => of(AccountsActions.loadAccountsFailure(error)))
        )
      )
    )
  );

  loadAccount$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountsActions.loadAccount),
      exhaustMap(({ id }) =>
        this.accountsClient.getById(id).pipe(
          map((account: AccountDto2) => AccountsActions.loadAccountSuccess({ account })),
          catchError((error) => of(AccountsActions.loadAccountFailure(error)))
        )
      )
    )
  );
}
