import { createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromAccounts from '../reducers/accounts.reducer';

export const selectAccountsState = createFeatureSelector<fromAccounts.State>(fromAccounts.accountsFeatureKey);

// Added these custom selectors

export const { selectIds, selectEntities, selectAll, selectTotal } = fromAccounts.adapter.getSelectors(
  selectAccountsState
);

// export const selectEntityById = createSelector(selectEntities, (entities, props) => entities[props.id]);
// export const selectEntitiesById = createSelector(selectEntities, (entities, props) =>
//   props.ids.map((id) => entities[id])
// );

// with factory functions
export const selectEntityById = (id) => createSelector(selectEntities, (entities) => entities[id]);
export const selectEntitiesById = (ids) => createSelector(selectEntities, (entities) => ids.map((id) => entities[id]));

export const selectAllSortedByName = createSelector(selectAll, (all) =>
  all.sort((a, b) => (a.name.toLowerCase() > b.name.toLowerCase() ? 1 : -1))
);

export const selectCreateAccountError = createSelector(selectAccountsState, (state) => state.createAccountError);
