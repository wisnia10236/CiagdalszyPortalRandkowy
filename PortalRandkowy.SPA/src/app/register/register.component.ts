import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
  ) {}

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    },
    this.createRegisterForm();
  }

  // inny sposob formgroup
  createRegisterForm() {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(15)]],  // ze jest wymagane oraz ze min to 4 i max to 15
      confirmPassword: ['', Validators.required],
      gender: ['female'],
      dateOfBirth: [null, Validators.required],
      zodiacSign: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
    }, {validator: this.passwordMatchWalidator});     // dodatkowy walidator dla hasla (sprawdza czy pasuja do siebie hasla)
  }


  passwordMatchWalidator(fg: FormControl) {
    return fg.get('password').value === fg.get('confirmPassword').value ? null : { missmatch: true };
    // pobieramy wartosc password i confirm password i sprawdzamy je, jesli sa takie same to zwracamy null jak nie to zwracamy obiekt missmatch
  }

  register() {
    // this.authService.register(this.model).subscribe(
    //   () => {
    //     this.alertify.success('rejestracja udana');
    //   },
    //   (error) => {
    //     this.alertify.error(error);
    //   }
    // );
    console.log(this.registerForm.value);
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
