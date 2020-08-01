import { Action, createReducer, on } from '@ngrx/store';
import { EntityState, EntityAdapter, createEntityAdapter, Update } from '@ngrx/entity';
import * as AccountsActions from '../actions/accounts.actions';
import { AccountDto, AccountDto2 } from 'src/app/crm-api';

export const accountsFeatureKey = 'accounts';

export interface State extends EntityState<AccountDto> {
  // additional state property
  loading: boolean;
  error: any;
  createAccountError: any;
}

export const adapter: EntityAdapter<AccountDto> = createEntityAdapter<AccountDto>();

export const initialState: State = adapter.getInitialState({
  // additional entity state properties
  loading: false,
  error: null,
  createAccountError: null,
});

export const reducer = createReducer(
  initialState,
  // Load accounts
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
  // Load account
  on(AccountsActions.loadAccount, (state) => {
    return {
      ...state,
      loading: true,
      error: null,
    };
  }),
  on(AccountsActions.loadAccountSuccess, (state, { account }) => {
    if (!account) {
      return {
        ...state,
        loading: false,
      };
    }
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
  }),
  // Create account
  on(AccountsActions.createAccount, (state) => {
    return {
      ...state,
      loading: true,
      error: null,
      createAccountError: null,
    };
  }),
  on(AccountsActions.createAccountSuccess, (state, { account }) => {
    return adapter.upsertOne(account, {
      ...state,
      loading: false,
    });
  }),
  on(AccountsActions.createAccountFailure, (state, error) => {
    return {
      ...state,
      loading: false,
      createAccountError: JSON.parse(error.response),
    };
  }),
  // Activate account
  on(AccountsActions.activateAccount, (state) => {
    return {
      ...state,
      loading: true,
      error: null,
    };
  }),
  on(AccountsActions.activateAccountSuccess, (state, { accountId }) => {
    return adapter.updateOne(
      {
        id: accountId,
        changes: {
          isActive: true,
        },
      },
      {
        ...state,
        loading: false,
      }
    );
  }),
  on(AccountsActions.activateAccountFailure, (state, error) => {
    return {
      ...state,
      loading: false,
    };
  }),
  // Deactivate account
  on(AccountsActions.deactivateAccount, (state) => {
    return {
      ...state,
      loading: true,
      error: null,
    };
  }),
  on(AccountsActions.deactivateAccountSuccess, (state, { accountId }) => {
    return adapter.updateOne(
      {
        id: accountId,
        changes: {
          isActive: false,
        },
      },
      {
        ...state,
        loading: false,
      }
    );
  }),
  on(AccountsActions.deactivateAccountFailure, (state, error) => {
    return {
      ...state,
      loading: false,
    };
  })
);
