import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  registerForm: FormGroup;

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.registerForm = new FormGroup({
      username: new FormControl('Podaj nazwę użytkownika', Validators.required),
      // tslint:disable-next-line: max-line-length
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(15)]), // ze jest wymagane oraz ze min to 4 i max to 15
      confirmPassword: new FormControl('', Validators.required),
    }, this.passwordMatchWalidator);
  }


  passwordMatchWalidator(fg: FormControl)
  {
    return fg.get('password').value === fg.get('confirmPassword').value ? null : { missmatch: true };
    // tslint:disable-next-line:max-line-length
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
