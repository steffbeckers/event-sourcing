import * as fromAccounts from '../reducers/accounts.reducer';
import { selectAccountsState } from './accounts.selectors';

describe('Accounts Selectors', () => {
  it('should select the feature state', () => {
    const result = selectAccountsState({
      [fromAccounts.accountsFeatureKey]: {},
    });

    expect(result).toEqual({});
  });
});
