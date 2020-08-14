import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  zodiacSignList: any[] = [ {value: 'wszystkie', display: 'wszystkie'},
                  {value: 'Baran', display: 'baran'},
                  {value: 'Byk', display: 'byk'},
                  {value: 'Bliźnięta', display: 'bliznieta'},
                  {value: 'Rak', display: 'rak'},
                  {value: 'Lew', display: 'lew'},
                  {value: 'Panna', display: 'panna'},
                  {value: 'Waga', display: 'waga'},
                  {value: 'Skorpion', display: 'skorpion'},
                  {value: 'Strzelec', display: 'strzelec'},
                  {value: 'Koziorożec', display: 'koziorozec'},
                  {value: 'Wodnik', display: 'wodnik'},
                  {value: 'Ryby', display: 'ryby'}];
  userParams: any = {};

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
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
      gender: ['kobieta'],
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
    if (this.registerForm.valid) {

      this.user = Object.assign({}, this.registerForm.value );    // z formularza przypisujemy wartosci do usera

      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('rejestracja udana');
      },
      (error) => {
        this.alertify.error(error);
      }, () => {            // jesli zarejestruje nas to subskrybujemy uztk i przerzucamy go do strony  automatycznie z uzytkownikam,i
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/uzytkownicy']);
        });
      });
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
