import { TestBed } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable } from 'rxjs';

import { AccountsEffects } from './accounts.effects';

describe('AccountsEffects', () => {
  let actions$: Observable<any>;
  let effects: AccountsEffects;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AccountsEffects,
        provideMockActions(() => actions$)
      ]
    });

    effects = TestBed.inject(AccountsEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});
