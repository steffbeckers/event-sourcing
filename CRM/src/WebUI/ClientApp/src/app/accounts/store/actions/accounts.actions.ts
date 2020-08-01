import { createAction, props } from '@ngrx/store';
import { AccountDto, AccountDto2, CreateAccountDto } from 'src/app/crm-api';

export const loadAccounts = createAction('[Accounts] Load accounts');
export const loadAccountsSuccess = createAction(
  '[Accounts] Load accounts Success',
  props<{ accounts: AccountDto[] }>()
);
export const loadAccountsFailure = createAction('[Accounts] Load accounts Failure', props<any>());

export const loadAccount = createAction('[Accounts] Load account', props<{ id: string }>());
export const loadAccountSuccess = createAction('[Accounts] Load account Success', props<{ account: AccountDto2 }>());
export const loadAccountFailure = createAction('[Accounts] Load account Failure', props<any>());

export const createAccount = createAction('[Accounts] Create account', props<{ account: CreateAccountDto }>());
export const createAccountSuccess = createAction(
  '[Accounts] Create account Success',
  props<{ account: AccountDto2 }>()
);
export const createAccountFailure = createAction('[Accounts] Create account Failure', props<any>());
