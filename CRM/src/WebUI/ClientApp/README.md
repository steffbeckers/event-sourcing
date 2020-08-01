## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

Ran commands for NgRx and modules

NgRx
https://ngrx.io/guide/schematics
`npm install @ngrx/schematics @ngrx/store @ngrx/effects @ngrx/entity @ngrx/store-devtools`

`ng generate @ngrx/schematics:store State --root --statePath store --module app.module.ts`

`ng generate @ngrx/schematics:effect store/App --root --module app.module.ts --group`

`ng generate module dashboard --route dashboard --module app.module.ts`

`ng generate module accounts --route accounts --module app.module.ts`
`ng generate component accounts/accounts-list --module accounts/accounts.module.ts`
`ng generate component accounts/account-detail --module accounts/accounts.module.ts`
`ng generate component accounts/account-edit --module accounts/accounts.module.ts`
`ng generate component accounts/account-create --module accounts/accounts.module.ts`
