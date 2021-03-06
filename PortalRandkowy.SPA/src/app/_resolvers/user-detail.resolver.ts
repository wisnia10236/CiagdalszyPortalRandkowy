import { Injectable } from "@angular/core";
import { Resolve, Router , ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class UserDetailResolver implements Resolve<User>{

  constructor(private userService: UserService,
              private router:Router ,
              private alerify:AlertifyService){}
  resolve(route: ActivatedRouteSnapshot): User | Observable<User> | Promise<User> {
    return this.userService.getUser(route.params.id).pipe(
      catchError(error => {
        this.alerify.error('Problem z pobraniem danych');
        this.router.navigate(['/uzytkownicy']);
        return of(null);
      })
    );
  }
}
