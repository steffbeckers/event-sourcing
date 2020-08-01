import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, switchMap, catchError, exhaustMap, mapTo, tap } from 'rxjs/operators';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as AccountsActions from '../actions/accounts.actions';
import { AccountsClient, AccountDto, AccountDto2 } from 'src/app/crm-api';
import { Router } from '@angular/router';

@Injectable()
export class AccountsEffects {
  constructor(private actions$: Actions, private accountsClient: AccountsClient, private router: Router) {}

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
      switchMap(({ id }) =>
        this.accountsClient.getById(id).pipe(
          map((account: AccountDto2) => AccountsActions.loadAccountSuccess({ account })),
          catchError((error) => of(AccountsActions.loadAccountFailure(error)))
        )
      )
    )
  );

  createAccount$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountsActions.createAccount),
      switchMap(({ account }) =>
        this.accountsClient.create(account).pipe(
          map(() => AccountsActions.createAccountSuccess({ account })),
          catchError((error) => of(AccountsActions.createAccountFailure(error)))
        )
      )
    )
  );

  // TODO: Can't this be combined in the createAccount$ effect? with tap()?
  navigateAfterCreateAccountSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AccountsActions.createAccountSuccess),
        tap(({ account }) => {
          this.router.navigateByUrl('/accounts/' + account.id);
        })
      ),
    { dispatch: false }
  );

  activateAccount$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountsActions.activateAccount),
      switchMap(({ accountId }) =>
        this.accountsClient.activate(accountId).pipe(
          map(() => AccountsActions.activateAccountSuccess({ accountId })),
          catchError((error) => of(AccountsActions.activateAccountFailure(error)))
        )
      )
    )
  );

  deactivateAccount$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountsActions.deactivateAccount),
      switchMap(({ accountId }) =>
        this.accountsClient.deactivate(accountId).pipe(
          map(() => AccountsActions.deactivateAccountSuccess({ accountId })),
          catchError((error) => of(AccountsActions.deactivateAccountFailure(error)))
        )
      )
    )
  );
}
