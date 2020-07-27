import * as fromAccounts from './accounts.actions';

describe('loadAccounts', () => {
  it('should return an action', () => {
    expect(fromAccounts.loadAccounts().type).toBe('[Accounts] Load Accounts');
  });
});
