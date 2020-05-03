import { Injectable } from "@angular/core";
import { Resolve, Router , ActivatedRouteSnapshot,  } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';


@Injectable()
export class UserEditResolver implements Resolve<User>{

  constructor(private userService: UserService,
              private router:Router ,
              private alerify:AlertifyService,
              private authService: AuthService) {}
  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    return this.userService.getUser(this.authService.decodedToken.nameid).pipe(
      catchError(error => {
        this.alerify.error('Problem z pobraniem danych');
        this.router.navigate(['/uzytkownicy']);
        return of(null);
      })
    );
  }
}
