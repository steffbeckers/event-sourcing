import { Action, createReducer, on } from '@ngrx/store';
import { EntityState, EntityAdapter, createEntityAdapter, Update } from '@ngrx/entity';
import * as AccountsActions from '../actions/accounts.actions';
import { AccountDto, AccountDto2 } from 'src/app/crm-api';

export const accountsFeatureKey = 'accounts';

export interface State extends EntityState<AccountDto> {
  // additional state property
  loading: boolean;
  error: any;
}

export const adapter: EntityAdapter<AccountDto> = createEntityAdapter<AccountDto>();

export const initialState: State = adapter.getInitialState({
  // additional entity state properties
  loading: false,
  error: null,
});

export const reducer = createReducer(
  initialState,
  on(AccountsActions.loadAccounts, (state) => {
    return {
      ...state,
      loading: true,
      error: null,
    };
  }),
  on(AccountsActions.loadAccountsSuccess, (state, { accounts }) => {
    return adapter.upsertMany(accounts, {
      ...state,
      loading: false,
    });
  }),
  on(AccountsActions.loadAccountsFailure, (state, error) => {
    return {
      ...state,
      loading: false,
      error,
    };
  }),
  on(AccountsActions.loadAccount, (state) => {
    return {
      ...state,
      loading: true,
      error: null,
    };
  }),
  on(AccountsActions.loadAccountSuccess, (state, { account }) => {
    return adapter.upsertOne(account, {
      ...state,
      loading: false,
    });
  }),
  on(AccountsActions.loadAccountFailure, (state, error) => {
    return {
      ...state,
      loading: false,
      error,
    };
  })
);
