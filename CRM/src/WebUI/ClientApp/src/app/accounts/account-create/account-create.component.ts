import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { CreateAccountDto, ICreateAccountDto } from 'src/app/crm-api';
import { Store } from '@ngrx/store';
import { createAccount } from '../store/actions/accounts.actions';
import { selectCreateAccountError } from '../store/selectors/accounts.selectors';

@Component({
  selector: 'app-account-create',
  templateUrl: './account-create.component.html',
  styleUrls: ['./account-create.component.css'],
})
export class AccountCreateComponent implements OnInit {
  public accountForm: FormGroup = this.fb.group({
    name: [null, Validators.required],
    website: null,
    email: null,
    phoneNumber: null,
  });

  private createAccountError$ = this.store.select(selectCreateAccountError);
  public errors = null;

  constructor(private store: Store, private fb: FormBuilder) {}

  ngOnInit(): void {
    this.createAccountError$.subscribe((error) => {
      this.errors = error?.errors;
    });
    this.accountForm.valueChanges.subscribe(() => {
      this.errors = null;
    });
    this.errors = null;
  }

  save(): void {
    // Validation
    if (this.accountForm.invalid) {
      return;
    }

    const account = new CreateAccountDto(this.accountForm.value as ICreateAccountDto);

    this.store.dispatch(createAccount({ account }));
  }
}
